using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.node
{
    internal class ConstantNode
    {
        public int Value { get; set; }

        public ConstantNode(int value)
        {
            Value = value;
        }
    }
}
