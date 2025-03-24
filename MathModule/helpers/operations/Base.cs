using MathModule.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.helpers.operations
{
    internal class Base
    {
        public static ElementNode CalculateNode(ElementNode first, ElementNode second)
        {
            //first константа
            if (first.Type == 1)
                return ConstantAndNode(first, second);
            //first параметр
            else if (first.Type == 2)
            {
                if (second.Type == 1) return ConstantAndNode(second, first);
                else if (second.Type == 2) return VariableAndVariable(first, second);
                else if (second.Type == 3 && second.OperationNode.Type == 0)
                    return VariableAndConjunction(first, second);
                else
                    return VariableAndDisjunction(first, second);
            }
            //first выражение
            else
            {
                if (second.Type == 1) return ConstantAndNode(second, first);
                else if (first.OperationNode.Type == 0)
                {
                    if (second.Type == 2) return VariableAndConjunction(second, first);
                    else if (second.Type == 3 && second.OperationNode.Type == 0)
                        return ConjunctionAndConjunction(second, first);
                    else
                        return ConjunctionAndDisjunction(first, second);
                }
                else
                {
                    if (second.Type == 2) return VariableAndDisjunction(second, first);
                    else if (second.Type == 3 && second.OperationNode.Type == 0)
                        return ConjunctionAndDisjunction(second, first);
                    else
                        return DisjunctionAndDisjunction(second, first);
                }
            }
        }
        private static ElementNode ConstantAndNode(ElementNode constant, ElementNode node)
        {
            if (constant.ConstantNode.Value == 0) return new ElementNode(1, new ConstantNode(0));
            return helpers.Useful.CopyNode(node);
        }

        private static ElementNode VariableAndVariable(ElementNode variable1, ElementNode variable2)
        {
            if (variable1.ParameterNode.Index == variable2.ParameterNode.Index)
                return variable1;
            if (variable1.ParameterNode.Index + variable2.ParameterNode.Index == 0)
                return new ElementNode(1, new ConstantNode(0));
            var variables = new List<ElementNode>() { variable1, variable2 };
            variables = helpers.MinimizationNode.SortVariablesInConjunction(variables);
            return new ElementNode(3, new OperationNode(0, variables));
        }

        private static ElementNode VariableAndConjunction(ElementNode variable, ElementNode conjunction)
        {
            var variablesConjucntion = conjunction.OperationNode.Variables;
            foreach (var item in variablesConjucntion)
            {
                if (item.ParameterNode.Index + variable.ParameterNode.Index == 0)
                    return new ElementNode(1, new ConstantNode(0));
                if (item.ParameterNode.Index == variable.ParameterNode.Index)
                    return conjunction;
            }
            var variables = new List<ElementNode>();
            foreach (var item in conjunction.OperationNode.Variables) variables.Add(item);
            variables.Add(variable);
            variables = helpers.MinimizationNode.SortVariablesInConjunction(variables);
            return new ElementNode(3, new OperationNode(0, variables));
        }

        private static ElementNode ConjunctionAndConjunction(ElementNode conjunction1, ElementNode conjunction2)
        {
            var copyConjunction1 = helpers.Useful.CopyNode(conjunction1);
            var copyConjunction2 = helpers.Useful.CopyNode(conjunction2);
            var variablesConjunction1 = copyConjunction1.OperationNode.Variables;
            var variablesConjunction2 = copyConjunction2.OperationNode.Variables;

            var less = variablesConjunction1.Count > variablesConjunction2.Count ? variablesConjunction2 : variablesConjunction1;
            var more = variablesConjunction1.Count > variablesConjunction2.Count ? variablesConjunction1 : variablesConjunction2;

            foreach (var variable in more)
            {
                bool flag = false;
                foreach (var item in less)
                {
                    if (item.ParameterNode.Index + variable.ParameterNode.Index == 0)
                        return new ElementNode(1, new ConstantNode(0));
                    if (item.ParameterNode.Index == variable.ParameterNode.Index)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    less.Add(variable);
            }
            less = helpers.MinimizationNode.SortVariablesInConjunction(less);
            return new ElementNode(3, new OperationNode(0, less));
        }

        private static ElementNode VariableAndDisjunction(ElementNode variable, ElementNode disjunction)
        {
            var variables = new List<ElementNode>();
            foreach (var item in disjunction.OperationNode.Variables)
            {
                if (item.Type == 2)
                {
                    var node = VariableAndVariable(variable, item);
                    if (node.Type == 1 && node.ConstantNode.Value == 1) return new ElementNode(1, 1);
                    if (node.Type != 1)
                    {
                        variables.Add(node);
                        variables = MinimizationNode.MinimizationVariablesInDisjunction(variables);
                    }
                }
                if (item.Type == 3)
                {
                    var node = VariableAndConjunction(variable, item);
                    if (node.Type == 1 && node.ConstantNode.Value == 1) return new ElementNode(1, 1);
                    if (node.Type != 1)
                    {
                        variables.Add(node);
                        variables = MinimizationNode.MinimizationVariablesInDisjunction(variables);
                    }
                }
            }
            if (variables.Count == 0) return new ElementNode(1, 0);
            return new ElementNode(3, new OperationNode(1, variables));
        }

        private static ElementNode ConjunctionAndDisjunction(ElementNode conjunction, ElementNode disjunction)
        {
            var variables = new List<ElementNode>();
            foreach (var item in disjunction.OperationNode.Variables)
            {
                if (item.Type == 2)
                {
                    var node = VariableAndConjunction(item, conjunction);
                    if (node.Type == 1 && node.ConstantNode.Value == 1) return new ElementNode(1, 1);
                    if (node.Type != 1)
                    {
                        variables.Add(node);
                        variables = MinimizationNode.MinimizationVariablesInDisjunction(variables);
                    }
                }
                if (item.Type == 3)
                {
                    var node = ConjunctionAndConjunction(conjunction, item);
                    if (node.Type == 1 && node.ConstantNode.Value == 1) return new ElementNode(1, 1);
                    if (node.Type != 1)
                    {
                        variables.Add(node);
                        variables = MinimizationNode.MinimizationVariablesInDisjunction(variables);
                    }
                }
            }
            if (variables.Count == 0) return new ElementNode(1, 0);
            return new ElementNode(3, new OperationNode(1, variables));
        }

        private static ElementNode DisjunctionAndDisjunction(ElementNode disjunction1, ElementNode disjunction2)
        {
            var variables = new List<ElementNode>();
            foreach (var item in disjunction1.OperationNode.Variables)
            {
                if (item.Type == 2)
                {
                    var node = VariableAndDisjunction(item, disjunction2);
                    if (node.Type == 1 && node.ConstantNode.Value == 1) return new ElementNode(1, 1);
                    if (node.Type != 1)
                    {
                        variables = variables.Concat(node.OperationNode.Variables).ToList();
                        variables = MinimizationNode.MinimizationVariablesInDisjunction(variables);
                    }
                }
                if (item.Type == 3)
                {
                    var node = ConjunctionAndDisjunction(item, disjunction2);
                    if (node.Type == 1 && node.ConstantNode.Value == 1) return new ElementNode(1, 1);
                    if (node.Type != 1)
                    {
                        variables = variables.Concat(node.OperationNode.Variables).ToList();
                        variables = MinimizationNode.MinimizationVariablesInDisjunction(variables);
                    }
                }
            }
            if (variables.Count == 0) return new ElementNode(1, 0);
            return new ElementNode(3, new OperationNode(1, variables));
        }
    }
}
