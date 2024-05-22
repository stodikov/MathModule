using MathModule.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    internal class MatrixComposition
    {
        private static int rang = 3;

        public static void GetMatrixComposition(OperationNode formula)
        {
            var variables = formula.Variables;
            foreach (var variable in variables)
            {
                if (variable.Type == 3) GetMatrixComposition(variable.OperationNode);
            }
            var metaoperations = Metaoperations.getMetaoperations(3);
            var spaceMatrix = CalculateMatrix(metaoperations[formula.Type], variables);
            formula.SpaceMatrix = spaceMatrix;
        }

        private static List<ElementNode> CalculateMatrix(Multioperation metaoperation, List<ElementNode> variables)
        {
            var variablesReverse = variables;
            variablesReverse.Reverse();
            var tempMatrix = new ElementNode[0][];
            foreach (var variable in variablesReverse)
            {
                if (tempMatrix.Length == 0) tempMatrix = DoStepMatrixComposition(metaoperation.Matrix, variable);
                else tempMatrix = DoStepMatrixComposition(tempMatrix, variable);
            }
            var spaceMatrix = new List<ElementNode>();
            for (int i = 0; i < rang; i++) spaceMatrix.Add(tempMatrix[i][0]);
            return spaceMatrix;
        }

        private static ElementNode[][] DoStepMatrixComposition(ElementNode[][] matrix, ElementNode variable)
        {
            var newMatrix = new ElementNode[rang][];
            for (int i = 0; i < rang; i++)
            {
                newMatrix[i] = new ElementNode[matrix[i].Length / rang];
                for (int j = 0; j < matrix[i].Length; j += rang)
                {
                    var elements = CalculateElements(j, matrix[i], variable);
                    var index = j == 0 ? j : j / rang;
                    newMatrix[i][index] = elements;
                }
            }
            return newMatrix;
        }

        private static ElementNode CalculateElements(int index, ElementNode[] matrix, ElementNode variable)
        {
            ElementNode result = null;
            ElementNode node;
            ElementNode currentVariable;
            for (int i = index; i < index + rang; i++)
            {
                if (variable.Type == 2 && variable.ParameterNode.IsMultioperationParameter)
                    currentVariable = new ElementNode(2, variable.ParameterNode.SubParameters[i % rang]);
                else
                    currentVariable = variable.OperationNode.SpaceMatrix[i % rang];
                node = helpers.operations.Base.CalculateNode(matrix[i], currentVariable);
                if (node.Type == 1 && node.ConstantNode.Value != 0 || node.Type != 1)
                {
                    if (result == null) result = node;
                    else result = helpers.Useful.UnionNodes(result, node);
                }
            }
            if (result == null) result = new ElementNode(1, 0);
            return result;
        }
    }
}
