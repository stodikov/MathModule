using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.helpers
{
    internal class MinimizationNode
    {
        public static List<ElementNode> SortVariablesInConjunction(List<ElementNode> variables)
        {
            return variables.OrderBy(node => Math.Abs(node.ParameterNode.Index)).ToList();
        }

        private static bool IsEqualVariables(ElementNode first, ElementNode second)
        {
            if (first.Type != second.Type) return false;
            if (first.Type == 2 && second.Type == 2)
                return first.ParameterNode.Index == second.ParameterNode.Index;
            if (first.Type == 3 && second.Type == 3)
            {
                var firstVariables = first.OperationNode.Variables;
                var secondVariables = second.OperationNode.Variables;
                if (firstVariables.Count != secondVariables.Count) return false;

                for (int i = 0; i < firstVariables.Count; i++)
                {
                    if (firstVariables[i].ParameterNode.Index != secondVariables[i].ParameterNode.Index)
                        return false;
                }
            }
            return true;
        }

        public static List<ElementNode> MinimizationVariablesInDisjunction(List<ElementNode> variables)
        {
            variables = DeleteRepeatVariables(variables);
            variables = ReducingVariables(variables);
            return variables;
        }

        private static List<ElementNode> DeleteRepeatVariables(List<ElementNode> variables)
        {
            for (int i = 0; i < variables.Count; i++)
            {
                for (int j = 0; j < variables.Count; j++)
                {
                    if (i != j && IsEqualVariables(variables[i], variables[j]))
                        variables[i] = new ElementNode(1, 0);
                }
            }
            return variables.Where(node => node.Type != 1).ToList();
        }

        private static List<ElementNode> ReducingVariables(List<ElementNode> variables)
        {
            var minIndexes = new List<int>();
            bool running = true;
            while (running)
            {
                var minIndex = -1;
                for (int i = 0; i < variables.Count; i++)
                {
                    if (variables[i].Type == 2 && minIndexes.IndexOf(i) == -1)
                    {
                        minIndex = i;
                        break;
                    }
                    if (variables[i].Type == 3 && minIndexes.IndexOf(i) == -1)
                    {
                        if (minIndex != -1 && variables[minIndex].OperationNode.Variables.Count > variables[i].OperationNode.Variables.Count)
                            minIndex = i;
                        if (minIndex == -1) minIndex = i;
                    }
                }
                if (minIndex == -1)
                {
                    running = false;
                    continue;
                }
                minIndexes.Add(minIndex);
                var minNode = variables[minIndex];
                for (int i = 0; i < variables.Count; i++)
                {
                    if (i == minIndex) continue;
                    if (minNode.Type == 2 && variables[i].Type == 2 &&
                        minNode.ParameterNode.Index == variables[i].ParameterNode.Index)
                        variables[i] = new ElementNode(1, 0);
                    if (minNode.Type == 2 && variables[i].Type == 3)
                    {
                        var conjunctionVariables = variables[i].OperationNode.Variables;
                        for (int j = 0; j < conjunctionVariables.Count; j++)
                        {
                            if (conjunctionVariables[j].ParameterNode.Index == minNode.ParameterNode.Index)
                            {
                                variables[i] = new ElementNode(1, 0);
                                break;
                            }
                        }
                    }
                    if (minNode.Type == 3 && variables[i].Type == 3)
                    {
                        var isEqual = true;
                        var indexesMinNodeVariables = new List<int>();
                        foreach (var variable in minNode.OperationNode.Variables)
                            indexesMinNodeVariables.Add(variable.ParameterNode.Index);
                        var indexesConjunctionVariables = new List<int>();
                        foreach (var variable in variables[i].OperationNode.Variables)
                            indexesConjunctionVariables.Add(variable.ParameterNode.Index);
                        foreach (var index in indexesMinNodeVariables)
                        {
                            if (indexesConjunctionVariables.IndexOf(index) == -1)
                            {
                                isEqual = false;
                                break;
                            }
                        }
                        if (isEqual) variables[i] = new ElementNode(1, 0);
                    }
                }
            }
            return variables.Where(node => node.Type != 1).ToList();
        }
    }
}
