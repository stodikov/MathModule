using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.helpers
{
    internal class Print
    {
        public static void ConsoleFormula(ElementNode formula)
        {
            if (formula.Type == 1) ConsoleConstant(formula);
            if (formula.Type == 2) ConsoleParametr(formula);
            if (formula.Type == 3)
            {
                var variables = formula.OperationNode.Variables;
                var operation = formula.OperationNode.Type == 0 ? '&' : 'V';
                foreach (var variable in variables)
                {
                    if (variable.Type == 1)
                        ConsoleConstant(variable, operation);
                    if (variable.Type == 2)
                        ConsoleParametr(variable, operation);
                    if (variable.Type == 3)
                    {
                        Console.Write($"(");
                        ConsoleFormula(variable);
                        Console.Write($"){operation}");
                    }
                }
            }
        }

        private static void ConsoleConstant(ElementNode node, char operation = ' ')
        {
            Console.Write($"{node.ConstantNode.Value}{operation}");
        }

        private static void ConsoleParametr(ElementNode node, char operation = ' ')
        {
            Console.Write($"{node.ParameterNode.Designation}{operation}");
        }
    }
}
