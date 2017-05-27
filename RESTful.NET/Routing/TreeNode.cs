using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Routing
{
    /// <summary>
    /// TreeNode of a tree with a generic value
    /// </summary>
    /// <typeparam name="V">generic value</typeparam>
    public class TreeNode<V>
    {
        //private TreeNode<V> rootNode;
        private IList<TreeNode<V>> _childrenNodes = new List<TreeNode<V>>();
        private V _value;

        /// <summary>
        /// Value of this tree node
        /// </summary>
        public V Value
        {
            get
            {
                return _value;
            }
            set
            {
                this._value = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TreeNode()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">tree node value</param>
        public TreeNode(V value)
        {
            this._value = value;
        }

        /// <summary>
        /// Checks whether node has children
        /// </summary>
        public bool HasChildren { get { return _childrenNodes.Count > 0; } }

        /// <summary>
        /// Returns the number of children
        /// </summary>
        public int Count { get { return _childrenNodes.Count; } }

        /// <summary>
        /// Remove all children
        /// </summary>
        public void Clear() { _childrenNodes.Clear(); }

        /// <summary>
        /// Adds a children
        /// </summary>
        /// <param name="child">child node to be added</param>
        public void Add(TreeNode<V> child) { _childrenNodes.Add(child); }
        /// <summary>
        /// Removes a children
        /// </summary>
        /// <param name="child">child node to be removed</param>
        public void Remove(TreeNode<V> child) { _childrenNodes.Remove(child); }
        /// <summary>
        /// Removes children at index 
        /// </summary>
        /// <param name="index">index</param>
        public void RemoveAt(int index) { _childrenNodes.RemoveAt(index); }
        /// <summary>
        /// Returns the child node at index
        /// </summary>
        /// <param name="index">index</param>
        /// <returns></returns>
        public TreeNode<V> Get(int index) { return _childrenNodes[index]; }

        /// <summary>
        /// Returns the index of a specified child node
        /// </summary>
        /// <param name="child">child node</param>
        /// <returns></returns>
        public int GetIndex(TreeNode<V> child) { return _childrenNodes.IndexOf(child); }
        /// <summary>
        /// Checks whether this node has a children with the specified value
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public bool HasChildWithValue(V value)
        {
            foreach (TreeNode<V> node in _childrenNodes)
            {
                if (node.Value.Equals(value))
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Returns all children having the specified value
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public TreeNode<V> GetChildWithValue(V value)
        {
            foreach (TreeNode<V> node in _childrenNodes)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }

            }
            return null;
        }
    }
}
