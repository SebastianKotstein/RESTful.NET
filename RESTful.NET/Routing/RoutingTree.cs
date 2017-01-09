using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Routing
{
    /// <summary>
    /// Maps a URL path to the identifier of an specific routing entry.
    /// The Routing Tree starts with the HTTP Methods as the childs of the root element and continous with the path.
    /// </summary>
    public class RoutingTree
    {
        //root node
        private TreeNode<string> _root;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RoutingTree()
        {
            _root = new TreeNode<string>();
        }

        /// <summary>
        /// Adds a path or a sub path to this tree (depending on whether this path or parts of this path have already been added)
        /// and registers the ID as a lead
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="targetID">ID of the routing entry</param>
        public void AddPath(string path, string targetID)
        {
            //Path looks like GET/login/{id}/hello/

            //1. Split the path into its elements
            string[] elements = path.Split('/');
            //there must be at least two elements for a minimal path, e.g.: GET/ or POST/
            if (elements.Length < 2)
            {
                throw new Exception("Cannot add path due to invalid format");
            }
            //2. add path
            TreeNode<string> currentNode = _root;
            foreach (string element in elements)
            {
                string value;
                if (element.Contains("{"))
                {
                    value = "{}";
                }
                else
                {
                    value = element;
                }

                TreeNode<string> child = currentNode.GetChildWithValue(value);
                if (child == null)
                {
                    TreeNode<string> newChild = new TreeNode<string>(value);
                    currentNode.Add(newChild);
                    currentNode = newChild;
                }
                else
                {
                    currentNode = child;
                }
            }
            //3. add identifier
            if (!currentNode.HasChildWithValue(targetID))
            {
                currentNode.Add(new TargetIdNode(targetID));
            }
        }

        /// <summary>
        /// Returns the target identifiers for a resolved path
        /// </summary>
        /// <param name="path">path</param>
        /// <returns></returns>
        public IList<string> FindTargetIDs(string path)
        {
            //Prepare list of possible targetIDs
            IList<string> targetIDs = new List<string>();

            //1. Split the path into its elements
            string[] elements = path.Split('/');
            //there must be at least two elements for a minimal path, e.g.: GET/ or POST/
            if (elements.Length < 2)
            {
                throw new Exception("Cannot add path due to invalid format");
            }

            //2. search target IDs based on Breadth-First-Search (by iterating each url element, starting with the HTTP method)

            //toBeSearched containts the list of nodes whose childs should be searched in the current iteration
            IList<TreeNode<string>> toBeSearched = new List<TreeNode<string>>();
            //toBeSearchedNext containts the list of childs whose childs should be searched in the next iteration
            IList<TreeNode<string>> toBeSearchedNext = new List<TreeNode<string>>();
            //initially, the root is the node whose child must match the first elemement (i.e. should be searched)
            toBeSearched.Add(_root);

            //iterate over each element
            for (int i = 0; i < elements.Length + 1; i++)
            {
                //node is the TreeNode which has the value of the previous element (or it is the root node). Its children may contain the value of the current element,
                //hence we search the children whether they match the current element
                foreach (TreeNode<string> node in toBeSearched)
                {
                    //Exception: If we have searched fo the last element of the URL in the previous iteration, then we are now searching for the targets IDs
                    if (i == elements.Length)
                    {
                        //all childs may be targetID nodes (but this is not a must and must be checked)
                        for (int j = 0; j < node.Count; j++)
                        {
                            TreeNode<string> n = node.Get(j);
                            //only if they are target id nodes
                            if (n is TargetIdNode)
                            {
                                targetIDs.Add(n.Value);
                            }
                        }
                    }
                    //Normal case: We will now analyze every child of the node, whether its value matches
                    else
                    {
                        //There are two cases: The node is generic which means that every element matches execpt it is empty (= "")
                        //or the node is non-generic and the element must excactly (except capitalization, but not the first element = HTTP Method which is always in caps!!)

                        //add the nodes which might match the element
                        TreeNode<string> child1 = null;
                        TreeNode<string> child2 = null;


                        if (i != 0) //do not lower first element since it is the HTTP method
                        {
                            child1 = node.GetChildWithValue(elements[i].ToLower());
                        }
                        else
                        {
                            child1 = node.GetChildWithValue(elements[i]);
                        }

                        //add generic values (only if the element is not empty)
                        if (!elements[i].Equals(""))
                        {
                            child2 = node.GetChildWithValue("{}");
                        }
                        //only if found: add those elements
                        if (child1 != null)
                        {
                            toBeSearchedNext.Add(child1);
                        }
                        if (child2 != null)
                        {
                            toBeSearchedNext.Add(child2);
                        }
                    }
                }
                //copy list of child nodes into list of the nodes which should be searched in course of the next iteration
                toBeSearched = toBeSearchedNext;
                toBeSearchedNext = new List<TreeNode<string>>(); //clear next list

            }
            return targetIDs;

        }
    }
}

