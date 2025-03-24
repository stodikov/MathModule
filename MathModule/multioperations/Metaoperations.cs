using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    internal class Metaoperations
    {
        public static Dictionary<int, Multioperation> getMetaoperations(int rang)
        {
            return new Dictionary<int, Multioperation>()
            {
                { 0, new Multioperation(rang, new int[] {1, 1, 1, 1, 2, 4, 4, 4, 4}) },
                { 1, new Multioperation(rang, new int[] {1, 2, 4, 2, 2, 2, 4, 2, 4}) },
                { 2, new Multioperation(rang, new int[] {2, 1, 4}) },
            };
        }
    }
}
