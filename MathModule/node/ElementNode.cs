using MathModule.node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    internal class ElementNode
    {
        public int Type {  get; set; }
        public ConstantNode ConstantNode { get; set; }
        public ParameterNode ParameterNode {  get; set; }
        public OperationNode OperationNode { get; set; }

        public ElementNode(int type, int value)
        {
            Type = type;
            ConstantNode = new ConstantNode(value);
            ParameterNode = null;
            OperationNode = null;
        }
        public ElementNode(int type, ConstantNode constantNode)
        {
            Type = type;
            ConstantNode = constantNode;
            ParameterNode = null;
            OperationNode = null;
        }
        public ElementNode(int type, int index, string designation, bool isMultioperationParameter)
        {
            Type = type;
            ConstantNode = null;
            ParameterNode = new ParameterNode(index, designation, isMultioperationParameter);
            OperationNode = null;
        }
        public ElementNode(int type, ParameterNode parameterNode)
        {
            Type = type;
            ConstantNode = null;
            ParameterNode = parameterNode;
            OperationNode = null;
        }
        public ElementNode(int type, int typeOperation, List<ElementNode> variables)
        {
            Type = type;
            ConstantNode = null;
            ParameterNode = null;
            OperationNode = new OperationNode(typeOperation, variables);
        }
        public ElementNode(int type, OperationNode operationNode)
        {
            Type = type;
            ConstantNode = null;
            ParameterNode = null;
            OperationNode = operationNode;
        }
    }
}
