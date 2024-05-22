using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.node
{
    internal class ParameterNode
    {
        public int Index { get; set; }
        public string Designation { get; set; }
        public bool IsMultioperationParameter { get; set; }
        public List<ParameterNode> SubParameters { get; set; }

        public ParameterNode(int index, string designation = "", bool isMultioperationParameter = false)
        {
            Index = index;
            Designation = designation;
            IsMultioperationParameter = isMultioperationParameter;

            if (IsMultioperationParameter)
            {
                SubParameters = new List<ParameterNode>();
                for (int i = 1; i < 4; i++) SubParameters.Add(new ParameterNode(index + i, $"{Designation}{i}"));
            }
        }
    }
}
