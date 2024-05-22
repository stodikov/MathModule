using MathModule.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.helpers
{
    internal class Useful
    {
        public static ElementNode UnionNodes(ElementNode firstNode, ElementNode secondNode)
        {
            if (firstNode.Type == 3)
            {
                if (firstNode.OperationNode.Type == 1)
                {
                    if (secondNode.Type != 3 || secondNode.Type == 3 && secondNode.OperationNode.Type == 0)
                        firstNode.OperationNode.Variables.Add(secondNode);
                    else
                        firstNode.OperationNode.Variables = firstNode.OperationNode.Variables.Concat(secondNode.OperationNode.Variables).ToList();
                }
                else
                {
                    if (secondNode.Type == 3 && secondNode.OperationNode.Type == 1)
                    {
                        var variables = new List<ElementNode>() { firstNode };
                        variables = variables.Concat(secondNode.OperationNode.Variables).ToList();
                        firstNode = new ElementNode(3, 1, variables);
                    }
                    else
                        firstNode = new ElementNode(3, 1, new List<ElementNode>() { secondNode, firstNode });
                }
            }
            else if (secondNode.Type == 3 && secondNode.OperationNode.Type == 1)
            {
                var variables = new List<ElementNode>() { firstNode };
                variables = variables.Concat(secondNode.OperationNode.Variables).ToList();
                firstNode = new ElementNode(3, 1, variables);
            }
            else
                firstNode = new ElementNode(3, 1, new List<ElementNode>() { secondNode, firstNode });
            return firstNode;
        }

        public static ElementNode CopyNode(ElementNode node)
        {
            if (node.Type == 1 || node.Type == 2) return node;
            var variables = new List<ElementNode>();
            foreach (ElementNode variable in node.OperationNode.Variables)
                variables.Add(variable);
            return new ElementNode(node.Type, new OperationNode(node.OperationNode.Type, variables));
        }
    }
}
