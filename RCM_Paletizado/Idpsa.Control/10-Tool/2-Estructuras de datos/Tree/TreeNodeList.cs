using System.Collections.Generic;

namespace Idpsa.Control.Tool
{

    public class TreeNodeList<T> : List<TreeNode<T>>
    {

        private TreeNode<T> _parent;

        public TreeNodeList(TreeNode<T> Parent):base()
        {
            _parent = Parent;
        }

        public new TreeNode<T> Add(TreeNode<T> node)
        {
            base.Add(node);
            node.Parent = _parent;
            return node;
        }

        public TreeNode<T> Add(T value)
        {
            return Add(new TreeNode<T>(value));
        }

        public override string ToString()
        {
            return "Count=" + Count.ToString();
        }

    }










}