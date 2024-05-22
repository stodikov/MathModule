using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.analysis
{
    internal class DataSufficiency
    {
        public static List<string> analysis = new List<string>();
        public static string DataSufficiencyAnalysis(ElementNode node, List<int> indexesArbitraries)
        {
            analysis.Clear();
            if (node.Type == 2)
                AnalysisParameter(node, indexesArbitraries);
            if (node.Type == 3)
            {
                if (node.OperationNode.Type == 0)
                    AnalysisConjunction(node, indexesArbitraries);
                else
                    AnalysisDisjunction(node, indexesArbitraries);
            }

            analysis = analysis.Distinct().ToList();
            if (analysis.Count == 0) return "";
            return String.Join("\n", analysis);
        }


        private static void AnalysisParameter(ElementNode node, List<int> indexesArbitraries)
        {
            if (indexesArbitraries.IndexOf(node.ParameterNode.Index) != -1)
                analysis.Add($"В решении имеется произвольный параметер {node.ParameterNode.Designation}. Необходимо добавить ограничения, чтобы убрать произвольный параметер\n");
        }

        private static void AnalysisConjunction(ElementNode node, List<int> indexesArbitraries)
        {
            var variables = node.OperationNode.Variables;
            foreach (var variable in variables )
                AnalysisParameter(variable, indexesArbitraries);
        }

        private static void AnalysisDisjunction(ElementNode node, List<int> indexesArbitraries)
        {
            var variables = node.OperationNode.Variables;
            foreach (var variable in variables)
            {
                if (variable.Type == 2)
                    AnalysisParameter(variable, indexesArbitraries);
                else
                    AnalysisConjunction(variable, indexesArbitraries);
            }
        }
    }
}
