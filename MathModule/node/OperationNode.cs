using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.node
{
    internal class OperationNode
    {
        public int Type {  get; set; }

        public List<ElementNode> Variables {  get; set; }

        public List<ElementNode> SpaceMatrix { get; set; }
        
        public OperationNode (int type, List<ElementNode> variables)
        {
            Type = type;
            Variables = variables;
        }

    }
}
