using MathModule.helpers;
using MathModule.node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MathModule
{
    internal class Program
    {
        private static int indexParams = 1;

        static void Main(string[] args)
        {
            var location = CreateParameters(3, 'l');
            var workmanship = CreateParameters(3, 'w');
            var asset = CreateParameters(3, 'a');
            var income = CreateParameters(3, 'i');
            var payment = CreateParameters(3, 'p');

            var parameters = new Dictionary<int, ParameterNode>();
            location.ToList().ForEach(param => parameters.Add(param.Key, param.Value));
            workmanship.ToList().ForEach(param => parameters.Add(param.Key, param.Value));
            asset.ToList().ForEach(param => parameters.Add(param.Key, param.Value));
            income.ToList().ForEach(param => parameters.Add(param.Key, param.Value));
            payment.ToList().ForEach(param => parameters.Add(param.Key, param.Value));

            var credits = CreateParameters(4, 'c');
            var arbitraries = CreateParameters(12, 'A');
            var formulas = CreateFormula(parameters);
            var domainsF = CreateFormulaDomains(credits);
            foreach (var formula in formulas)
                MatrixComposition.GetMatrixComposition(formula);
            var inequality = CreateInequality(formulas, domainsF);
            inequality.OperationNode.Variables = MinimizationNode.MinimizationVariablesInDisjunction(inequality.OperationNode.Variables);
            helpers.Print.ConsoleFormula(inequality);
            var indexesCredits = new List<int>();
            var indexesArbitraries = new List<int>();
            foreach (var credit in credits)
            {
                foreach (var subCredit in credit.Value.SubParameters)
                    indexesCredits.Add(subCredit.Index);
            }
            foreach (var arbitrary in arbitraries)
                indexesArbitraries.Add(arbitrary.Value.Index);
            var solution = Analytical.GetSolution(inequality, indexesCredits, arbitraries.Values.ToList());
            var analysis = Analysis.AnalysisSolution(solution, indexesArbitraries);
            foreach (var item in solution)
            {
                var indexVariable = item.Key;
                var designation = getDesignation(indexVariable, credits);
                Console.Write($"{designation} = ");
                helpers.Print.ConsoleFormula(item.Value);
                Console.WriteLine();
                Console.WriteLine($"Анализ для решения {designation} - {analysis[item.Key]}");
                Console.WriteLine();
                Console.WriteLine();
            }
            var a = 1;
        }
        //static void Main(string[] args)
        //{
        //    var print = new Print();
        //    var parameters = CreateParameters(3, 'x');
        //    var domains = CreateParameters(2, 'd');
        //    var arbitraries = CreateParameters(9, 'A');
        //    var formulas = CreateFormula(parameters);
        //    var domainsF = CreateFormulaDomains(domains);
        //    foreach (var formula in formulas)
        //    {
        //        MatrixComposition.GetMatrixComposition(formula);
        //        for (var i = 0; i < 3; i++)
        //        {
        //            helpers.Print.ConsoleFormula(formula.SpaceMatrix[i]);
        //            Console.WriteLine();
        //        }
        //        Console.WriteLine();
        //    }
        //    var inequality = CreateInequality(formulas, domainsF);
        //    helpers.Print.ConsoleFormula(inequality);
        //    var indexesDomains = new List<int>();
        //    var indexesArbitraries = new List<int>();
        //    foreach (var domain in domains)
        //    {
        //        foreach (var subDomain in domain.Value.SubParameters)
        //            indexesDomains.Add(subDomain.Index);
        //    }
        //    foreach (var arbitrary in arbitraries)
        //        indexesArbitraries.Add(arbitrary.Value.Index);
        //    var solution = Analytical.GetSolution(inequality, indexesDomains, arbitraries.Values.ToList());
        //    var analysis = Analysis.AnalysisSolution(solution, indexesArbitraries);
        //    foreach (var item in solution)
        //    {
        //        var indexVariable = item.Key;
        //        var designation = getDesignation(indexVariable, domains);
        //        Console.Write($"{designation} = ");
        //        helpers.Print.ConsoleFormula(item.Value);
        //        Console.WriteLine();
        //        Console.WriteLine($"Анализ для решения {designation} - {analysis[item.Key]}");
        //        Console.WriteLine();
        //        Console.WriteLine();
        //    }
        //    var a = 1;
        //}

        static string getDesignation(int indexVariable, Dictionary<int, ParameterNode> domains)
        {
            foreach (var domain in domains)
            {
                if (domain.Value.Index == indexVariable)
                    return domain.Value.Designation;
                foreach (var subDomain in domain.Value.SubParameters)
                    if (subDomain.Index == indexVariable)
                        return subDomain.Designation;
            }
            return "unknow";
        }

        static Dictionary<int, ParameterNode> CreateParameters(int amountParams, char symbol)
        {
            var parameters = new Dictionary<int, ParameterNode>();
            int i = 0;
            while (i < amountParams)
            {
                parameters.Add(indexParams, new ParameterNode(indexParams, $"{symbol}{i + 1}", true));
                indexParams += 4;
                i++;
            }
            return parameters;
        }

        static List<OperationNode> CreateFormula(Dictionary<int, ParameterNode> parameters)
        {
            var formulas = new List<OperationNode>();
            //formulas.Add(new OperationNode(1, new List<ElementNode>()
            //{
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>
            //    {
            //        new ElementNode(2, parameters[1]),
            //        new ElementNode(2, parameters[5]),
            //    })),
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>
            //    {
            //        new ElementNode(2, parameters[1]),
            //        new ElementNode(2, parameters[9]),
            //    })),
            //}));
            //formulas.Add(new OperationNode(0, new List<ElementNode>()
            //{
            //    new ElementNode(2, parameters[1]),
            //    new ElementNode(2, parameters[5]),
            //}));
            //formulas.Add(new OperationNode(0, new List<ElementNode>()
            //{
            //    new ElementNode(2, parameters[1]),
            //    new ElementNode(2, parameters[9]),
            //}));
            //1
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[37]),
                new ElementNode(2, parameters[53]),
            }));
            //2
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[37]),
                new ElementNode(2, parameters[57]),
            }));
            //3
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[41]),
                new ElementNode(2, parameters[57]),
            }));
            //4
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[25]),
                new ElementNode(2, parameters[37]),
            }));
            //5
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[25]),
                new ElementNode(2, parameters[41]),
            }));
            //6
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[29]),
                new ElementNode(2, parameters[41]),
            }));
            //7
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[13]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[25]),
                    new ElementNode(2, parameters[45]),
                })),
            }));
            //8
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[13]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[29]),
                    new ElementNode(2, parameters[41]),
                })),
            }));
            //9
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[13]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[33]),
                    new ElementNode(2, parameters[37]),
                })),
            }));
            //10
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[13]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[33]),
                    new ElementNode(2, parameters[41]),
                })),
            }));
            //11
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[25]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //12
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[25]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //13
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[5]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[25]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //14
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[9]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[25]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //15
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[41]),
                    })),
                })),
            }));
            //16
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[41]),
                    })),
                })),
            }));
            //17
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[5]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[41]),
                    })),
                })),
            }));
            //18
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[9]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[41]),
                    })),
                })),
            }));
            //19
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //20
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //21
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[5]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //22
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[9]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //23
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //24
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //25
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[5]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //26
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[9]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //27
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[5]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //28
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[5]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[33]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //29
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[9]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //30
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[9]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[33]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //31
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //32
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[21]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //33
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[5]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //34
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(2, parameters[9]),
                new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                {
                    new ElementNode(2, parameters[17]),
                    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
                    {
                        new ElementNode(2, parameters[29]),
                        new ElementNode(2, parameters[45]),
                    })),
                })),
            }));
            //35
            formulas.Add(new OperationNode(0, new List<ElementNode>()
            {
                new ElementNode(3, new OperationNode(1, new List<ElementNode>()
            {
                new ElementNode(2, parameters[1]),
                    new ElementNode(2, parameters[13]),
                })),
                new ElementNode(3, new OperationNode(1, new List<ElementNode>()
            {
                new ElementNode(2, parameters[9]),
                    new ElementNode(2, parameters[17]),
                })),
            }));

            //var formulas = new List<OperationNode>();
            //formulas.Add(new OperationNode(1, new List<ElementNode>()
            //{
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
            //    {
            //        new ElementNode(2, parameters[1]),
            //        new ElementNode(2, parameters[25]),
            //    })),
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
            //    {
            //        new ElementNode(2, parameters[1]),
            //        new ElementNode(2, parameters[5]),
            //    }))
            //}));
            //formulas.Add(new OperationNode(1, new List<ElementNode>()
            //{
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
            //    {
            //        new ElementNode(2, parameters[13]),
            //        new ElementNode(2, parameters[17]),
            //        new ElementNode(2, parameters[25]),
            //    })),
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
            //    {
            //        new ElementNode(2, parameters[13]),
            //        new ElementNode(2, parameters[17]),
            //        helpers.operations.Negative.CalculateNode(new ElementNode(2, parameters[21])),
            //    })),
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
            //    {
            //        new ElementNode(2, parameters[9]),
            //        new ElementNode(2, parameters[17]),
            //        new ElementNode(2, parameters[25]),
            //    })),
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
            //    {
            //        new ElementNode(2, parameters[9]),
            //        new ElementNode(2, parameters[17]),
            //        helpers.operations.Negative.CalculateNode(new ElementNode(2, parameters[21])),
            //    })),
            //}));
            //formulas.Add(new OperationNode(1, new List<ElementNode>()
            //{
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
            //    {
            //        new ElementNode(2, parameters[5]),
            //        helpers.operations.Negative.CalculateNode(new ElementNode(2, parameters[17])),
            //        new ElementNode(2, parameters[21]),
            //        new ElementNode(2, parameters[25]),
            //    })),
            //    new ElementNode(3, new OperationNode(0, new List<ElementNode>()
            //    {
            //        new ElementNode(2, parameters[1]),
            //        helpers.operations.Negative.CalculateNode(new ElementNode(2, parameters[17])),
            //        new ElementNode(2, parameters[21]),
            //        new ElementNode(2, parameters[25]),
            //    })),
            //}));

            return formulas;
        }

        static List<ParameterNode> CreateFormulaDomains(Dictionary<int, ParameterNode> domains)
        {
            var list = new List<ParameterNode>();
            foreach (var key in domains.Keys)
            {
                list.Add(domains[key]);
            }
            return list;
        }

        static ElementNode CreateInequality(List<OperationNode> formulas, List<ParameterNode> domains)
        {
            var inequalitySystem = new List<ElementNode>();
            var countD = 0;
            for (int i = 0; i < formulas.Count; i++)
            {
                if (i == 9 || i == 25 || i == 29) countD++;
                for (int j = 0; j < formulas[i].SpaceMatrix.Count; j++)
                {
                    var node = CreateInequaltySystem(formulas[i].SpaceMatrix[j], domains[countD].SubParameters[j]);
                    inequalitySystem.Add(node);
                }
            }
            //for (int i = 0; i < formulas.Count; i++)
            //{
            //    var node = CreateInequaltySystem(new ElementNode(3, formulas[i]), domains[i]);
            //    inequalitySystem.Add(node);
            //}
            var variablesGeneralInequality = new List<ElementNode>();
            foreach (var inequality in inequalitySystem)
            {
                if (inequality.OperationNode.Type == 1)
                    variablesGeneralInequality = variablesGeneralInequality.Concat(inequality.OperationNode.Variables).ToList();
                else
                    variablesGeneralInequality.Add(inequality);
            }
            return new ElementNode(3, new OperationNode(1, variablesGeneralInequality));
        }

        static ElementNode CreateInequaltySystem(ElementNode left, ParameterNode right)
        {
            var negRight = helpers.operations.Negative.CalculateNode(new ElementNode(2, right));

            //helpers.Print.ConsoleFormula(left);
            //Console.WriteLine();
            //helpers.Print.ConsoleFormula(negRight);
            //Console.WriteLine();
            //Console.WriteLine();

            return helpers.operations.Base.CalculateNode(left, negRight);
        }
    }
}
