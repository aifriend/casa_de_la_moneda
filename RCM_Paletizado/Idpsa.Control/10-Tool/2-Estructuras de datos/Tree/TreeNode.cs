using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Idpsa.Control.Tool
{

    public class TreeNode<T>:IEnumerable<TreeNode<T>>
    {
        private TreeNode<T> _parent;
        public TreeNodeList<T> Children { get; private set; }
        public T Value { get; set; }

        public TreeNode(T value)
        {
            Value = value;
            Parent = null;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode(T value, TreeNode<T> parent)
        {
            Value = value;
            Parent = parent;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode<T> Parent
        {
            get { return _parent; }

            set
            {
                if (value == _parent)
                {
                    return;
                }
                if (_parent != null)
                {
                    _parent.Children.Remove(this);
                }

                if (value != null && !value.Children.Contains(this))
                {
                    value.Children.Add(this);
                }

                _parent = value;
            }
        }

        public TreeNode<T> Root
        {
            get
            {
                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
                return node;
            }
        } 
        
        public void Execute(Action<T> action)
        {
            action(Value);            
            foreach (var child in Children)
                child.Execute(action);
        }


        public IEnumerable<TreeNode<T>> GetDescendantsNodes(Predicate<T> selector)
        {           
            var values = new List<TreeNode<T>>();
            GetDescendantsNodesHelper(selector, values);
            return values;
        }

        private void GetDescendantsNodesHelper(Predicate<T> selector,List<TreeNode<T>> values)
        {           
            foreach (var child in Children)
            {
                if (selector(child.Value))
                {
                    values.Add(child);
                    child.GetDescendantsNodesHelper(selector, values); 
                }
            }          
        }

        public IEnumerable<TreeNode<T>> GetAntecesorsNodes()
        {
            var values = new List<TreeNode<T>>();
            var aux= this;
            while (aux.Parent != null)
            {
                aux = aux.Parent;
                values.Add(aux);               
            }
           
            return values;
        }       

        public IEnumerable<T> GetDescendants(Predicate<T> selector)
        {
            return GetDescendantsNodes(selector).Select(n => n.Value);
        }


        public IEnumerable<TreeNode<T>> GetDescendantsNodes()
        {
            return GetDescendantsNodes(v => true);
        }               
   

        #region Miembros de IEnumerable<T>

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            foreach (var value in GetDescendantsNodes())
                yield return value;
        }      
        

        #endregion

        #region Miembros de IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();            
        }

        #endregion

    }  
}