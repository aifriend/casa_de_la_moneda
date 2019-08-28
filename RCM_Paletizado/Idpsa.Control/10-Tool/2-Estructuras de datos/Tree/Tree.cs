using System;
using System.Collections.Generic;

namespace Idpsa.Control.Tool
{
    public class Tree<T> : TreeNode<T>
    {        
        public Tree(T rootValue)
            :base(rootValue){}
    }
}