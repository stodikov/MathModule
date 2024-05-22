using MathModule.analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    internal class Analysis
    {
        public static Dictionary<int, string> AnalysisSolution(Dictionary<int, ElementNode> solution, List<int> indexesArbitraries)
        {
            var analysis = new Dictionary<int, string>();
            foreach (var item in solution)
            {
                var analysisDataSufficience = DataSufficiency.DataSufficiencyAnalysis(item.Value, indexesArbitraries);
                if (analysisDataSufficience == "") analysisDataSufficience = "Решение удовлетворяет всем условиям";
                analysis.Add(item.Key, analysisDataSufficience);
            }
            return analysis;
        }
    }
}
