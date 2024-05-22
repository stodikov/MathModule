using MathModule.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.helpers.operations
{
    internal class Residual
    {
        public static ElementNode CalculateGeneralResidual(ElementNode formula, List<int> indexesVariables)
        {
            var residuals = new List<ElementNode>();
            var limit = (int)Math.Pow(2, indexesVariables.Count);
            for (int i = 0; i < limit; i++)
            {
                var binary = Convert.ToString(i, 2);
                while (binary.Length != indexesVariables.Count) binary = $"0{binary}";
                var residualNode = CalculateResidual(formula, indexesVariables, binary);
                if (residualNode.Type == 1 && residualNode.ConstantNode.Value == 0) return new ElementNode(1, 0);
                if (residualNode.Type != 1) residuals.Add(residualNode);
            }
            var currentNode = residuals[0];
            for (int i = 1; i < residuals.Count; i++)
                currentNode = helpers.operations.Base.CalculateNode(currentNode, residuals[i]);

            return currentNode;
        }

        public static ElementNode CalculateResidual(ElementNode formula, List<int> indexesVariables, string binary)
        {
            ElementNode copyFormula = helpers.Useful.CopyNode(formula);
            if (copyFormula.Type == 2) return ResidualVariable(copyFormula, indexesVariables, binary);
            else if (copyFormula.Type == 3 && copyFormula.OperationNode.Type == 0) return ResidualConjunction(copyFormula, indexesVariables, binary);
            else return ResidualDisjunction(copyFormula, indexesVariables, binary);
        }

        private static ElementNode ResidualVariable(ElementNode variable, List<int> indexesVariables, string binary)
        {
            var index = indexesVariables.IndexOf(variable.ParameterNode.Index);
            if (index != -1)
            {
                if (binary[index] == '0') return new ElementNode(1, 0);
                else return new ElementNode(1, 1);
            }

            index = indexesVariables.IndexOf(variable.ParameterNode.Index * -1);
            if (index != -1)
            {
                if (binary[index] == '1') return new ElementNode(1, 0);
                else return new ElementNode(1, 1);
            }

            return variable;

        }

        private static ElementNode ResidualConjunction(ElementNode conjunction, List<int> indexesVariables, string binary)
        {
            var variables = conjunction.OperationNode.Variables;
            var newVariables = new List<ElementNode>();
            for (int i = 0; i < variables.Count; i++)
            {
                var node = ResidualVariable(variables[i], indexesVariables, binary);
                if (node.Type == 1 && node.ConstantNode.Value == 0) return new ElementNode(1, 0);
                if (node.Type != 1) newVariables.Add(node);
            }
            if (newVariables.Count == 0) return new ElementNode(1, 1);
            else return new ElementNode(3, new OperationNode(0, newVariables));

        }

        private static ElementNode ResidualDisjunction(ElementNode disjunction, List<int> indexesVariables, string binary)
        {
            var variables = disjunction.OperationNode.Variables;
            var newVariables = new List<ElementNode>();
            for (int i = 0; i < variables.Count; i++)
            {
                if (variables[i].Type == 2)
                {
                    var node = ResidualVariable(variables[i], indexesVariables, binary);
                    if (node.Type == 1 && node.ConstantNode.Value == 1) return new ElementNode(1, 1);
                    if (node.Type != 1) newVariables.Add(node);
                }
                else if (variables[i].Type == 3 && variables[i].OperationNode.Type == 0)
                {
                    var node = ResidualConjunction(variables[i], indexesVariables, binary);
                    if (node.Type == 1 && node.ConstantNode.Value == 1) return new ElementNode(1, 1);
                    if (node.Type != 1) newVariables.Add(node);
                }
            }

            if (newVariables.Count == 0) return new ElementNode(1, 0);
            else return new ElementNode(3, new OperationNode(1, newVariables));
        }
    }
}
