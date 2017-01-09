using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Routing
{
    /// <summary>
    /// Represents a leaf node with a stored identifier
    /// </summary>
    public class TargetIdNode : TreeNode<string>
    {
        public TargetIdNode(string value) : base(value)
        {

        }
    }
}
