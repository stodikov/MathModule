using MathModule.node;
using System.Collections.Generic;
using System.Linq;

namespace MathModule
{
    internal class Multioperation
    {
        public int Rang;
        public ElementNode[][] Matrix;

        public Multioperation(int rang, int[] multioperation)
        {
            Rang = rang;
            Matrix = createMatrix(rang, multioperation);
        }

        private ElementNode[][] createMatrix(int rang, int[] multioperation)
        {
            Dictionary<int, int[]> elements = new Dictionary<int, int[]>()
            {
                { 0, new int[] { 1, 3, 5, 7 } },
                { 1, new int[] { 2, 3, 6, 7 } },
                { 2, new int[] { 4, 5, 6, 7 } },
            };
            var matrix = new ElementNode[rang][];
            for (int i = 0; i < rang; i++)
            {
                matrix[i] = new ElementNode[multioperation.Length];
                for (int j = 0; j < multioperation.Length; j++)
                {
                    if (elements[i].Contains(multioperation[j])) matrix[i][j] = new ElementNode(1, new ConstantNode(1));
                    else matrix[i][j] = new ElementNode(1, new ConstantNode(0));
                }
            }
            return matrix;
        }
    }
}
