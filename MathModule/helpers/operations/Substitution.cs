using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.helpers.operations
{
    internal class Substitution
    {
        public static ElementNode CalculateNode(ElementNode inequality, int indexUnknow, ElementNode node)
        {
            if (inequality.Type == 2)
                return node;
            else if (inequality.OperationNode.Type == 0)
                return getSubstitutionConjunction(inequality, indexUnknow, node);
            else
                return getSubstitutionDisjunction(inequality, indexUnknow, node);
        }

        private static ElementNode getSubstitutionParameter(ElementNode parameter, int indexUnknow, ElementNode node)
        {
            var index = parameter.ParameterNode.Index;
            if (Math.Abs(index) == indexUnknow)
            {
                if (index > 0) return node;
                else return helpers.operations.Negative.CalculateNode(node);
            }
            return parameter;
        }

        private static ElementNode getSubstitutionConjunction(ElementNode conjunction, int indexUnknow, ElementNode node)
        {
            var variables = conjunction.OperationNode.Variables;
            var index = -1;
            bool neg = false;
            for (var i = 0; i < variables.Count; i++)
            {
                var indexVariable = variables[i].ParameterNode.Index;
                if (Math.Abs(indexVariable) == indexUnknow)
                {
                    index = i;
                    neg = indexVariable < 0;
                    break;
                }
            }
            if (index == -1) return conjunction;
            variables.RemoveAt(index);
            if (neg) node = helpers.operations.Negative.CalculateNode(node);
            return helpers.operations.Base.CalculateNode(conjunction, node);
        }

        private static ElementNode getSubstitutionDisjunction(ElementNode disjunction, int indexUnknow, ElementNode node)
        {
            var variables = disjunction.OperationNode.Variables;
            for (var i = 0; i < variables.Count; i++)
            {
                if (variables[i].Type == 2 && variables[i].ParameterNode.Index == indexUnknow)
                    variables[i] = node;
                else variables[i] = getSubstitutionConjunction(variables[i], indexUnknow, node);
            }
            //??? Может быть вызов функции минимизации 2 раза. Тут и внутри operation.Base;
            disjunction.OperationNode.Variables = helpers.MinimizationNode.MinimizationVariablesInDisjunction(disjunction.OperationNode.Variables);
            return disjunction;
        }
    }
}
