using MathModule.node;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace MathModule
{
    internal class Analytical
    {
        public static Dictionary<int, ElementNode> GetSolution(ElementNode inequality, List<int> indexesDomains, List<ParameterNode> arbitraries)
        {
            var node = Solvability(inequality, indexesDomains);
            if (node.Type == 1 && node.ConstantNode.Value == 1) Console.WriteLine("НЕТ РЕШЕНИЯ");
            return GeneralSolution(inequality, indexesDomains, arbitraries);
        }

        private static ElementNode Solvability(ElementNode inequality, List<int> indexesDomains)
        {
            return helpers.operations.Residual.CalculateGeneralResidual(inequality, indexesDomains);
        }

        private static Dictionary<int, ElementNode> GeneralSolution(ElementNode inequality, List<int> indexesDomains, List<ParameterNode> arbitraries)
        {
            var result = new Dictionary<int, ElementNode>();
            while (indexesDomains.Count > 0)
            {
                var currentIndex = new List<int>() { indexesDomains.Last() };
                indexesDomains.RemoveAt(indexesDomains.Count - 1);
                var arbitrary = new ElementNode(2, arbitraries.Last());
                arbitraries.RemoveAt(arbitraries.Count - 1);

                var residualNode = indexesDomains.Count != 0 ?
                    helpers.operations.Residual.CalculateGeneralResidual(inequality, indexesDomains) :
                    inequality;
                var nodeWithZero = helpers.operations.Residual.CalculateResidual(residualNode, currentIndex, "0");
                var newNodeWithZero = helpers.operations.Base.CalculateNode(arbitrary, nodeWithZero);

                //helpers.Print.ConsoleFormula(newNodeWithZero);
                //Console.WriteLine();
                //Console.WriteLine();

                var nodeWithOne = helpers.operations.Residual.CalculateResidual(residualNode, currentIndex, "1");
                var negNodeWithOne = helpers.operations.Negative.CalculateNode(nodeWithOne);
                var negArbitrary = helpers.operations.Negative.CalculateNode(arbitrary);
                var newNodeWithOne = helpers.operations.Base.CalculateNode(negArbitrary, negNodeWithOne);

                //helpers.Print.ConsoleFormula(newNodeWithOne);
                //Console.WriteLine();
                //Console.WriteLine();

                result.Add(currentIndex[0], helpers.Useful.UnionNodes(newNodeWithZero, newNodeWithOne));
                inequality = helpers.operations.Substitution.CalculateNode(inequality, currentIndex[0], result[currentIndex[0]]);
            }
            //Console.Clear();
            //foreach (var solution in result)
            //{
            //    helpers.Print.ConsoleFormula(solution.Value);
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}
            return result;
        }
    }
}
