using MathModule.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.helpers.operations
{
    internal class Negative
    {
        public static ElementNode CalculateNode(ElementNode node)
        {
            if (node.Type == 1)
                return NegativeConstant(node);
            if (node.Type == 2)
            {
                return NegativeParameter(node);
            }
            if (node.Type == 3 && node.OperationNode.Type == 0)
                return NegativeConjunction(node);
            else
                return NegativeDisjunction(node);
        }

        private static ElementNode NegativeConstant(ElementNode node)
        {
            return node.ConstantNode.Value == 1 ?
                    new ElementNode(1, 0) :
                    new ElementNode(1, 1);
        }

        private static ElementNode NegativeParameter(ElementNode node)
        {
            return new ElementNode(2, new ParameterNode(
                    node.ParameterNode.Index * -1,
                    $"{(node.ParameterNode.Index < 0 ? "" : "-")}{node.ParameterNode.Designation}"));
        }

        private static ElementNode NegativeConjunction(ElementNode node)
        {
            var negVariables = new List<ElementNode>();
            foreach (var variable in node.OperationNode.Variables)
            {
                if (variable.Type == 1)
                {
                    var negVariable = NegativeConstant(variable);
                    if (negVariable.ConstantNode.Value == 1)
                        return new ElementNode(1, 1);
                }
                else
                {
                    var negVariable = NegativeParameter(variable);
                    negVariables.Add(negVariable);
                }
            }
            if (negVariables.Count == 0) return new ElementNode(1, 0);
            negVariables = helpers.MinimizationNode.MinimizationVariablesInDisjunction(negVariables);
            return new ElementNode(3, new OperationNode(1, negVariables));
        }

        private static ElementNode NegativeDisjunction(ElementNode node)
        {
            var negVariables = new List<ElementNode>();
            foreach (var variable in node.OperationNode.Variables)
            {
                if (variable.Type == 1)
                {
                    var negVariable = NegativeConstant(variable);
                    if (negVariable.ConstantNode.Value == 0)
                        return new ElementNode(1, 0);
                    else
                        negVariables.Add(negVariable);
                }
                else if (variable.Type == 2)
                {
                    var negVariable = NegativeParameter(variable);
                    negVariables.Add(negVariable);
                }
                else
                {
                    var negVariable = NegativeConjunction(variable);
                    if (negVariable.Type == 2 && negVariable.ConstantNode.Value == 0)
                        return new ElementNode(1, 0);
                    else
                        negVariables.Add(negVariable);
                }
            }
            if (negVariables.Count == 0) return new ElementNode(1, 0);
            var newNode = negVariables[0];
            for (var i = 1; i < negVariables.Count; i++) newNode = helpers.operations.Base.CalculateNode(newNode, negVariables[i]);
            return newNode;
        }
    }
}
