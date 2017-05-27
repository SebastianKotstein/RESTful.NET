using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Routing
{
    /// <summary>
    /// Maps a URL path to a specific or mulitple routing entries.
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

        #region Add Path Methods
        /// <summary>
        /// Adds a path or a sub path to this tree (depending on whether this path or parts of this path have already been added)
        /// and registers the ID as a leaf
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="routingEntry">the routing entry</param>
        public void AddPath(string path, RoutingEntry routingEntry)
        {
            //Path looks like GET/login/{id}/hello/

            //1. Split the path into its elements
            string[] elements = path.Split('/');

            /*
            //1.a check for "*" which are not trailing
            if (path.TrimEnd('*').Contains("*"))
            {
                throw new Exception("'*' is only allowed as a trailer, use '{...}' instead to get generic values");
            }
            */

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
            if (!currentNode.HasChildWithValue(routingEntry.Identifier))
            {
                routingEntry.Metrics = elements.Length;
                currentNode.Add(routingEntry);
            }
        }
        #endregion
        #region Find Path Methods

        /// <summary>
        /// Returns the Routing Entries for a resolved (passed) path
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>list with matching routing entries</returns>
        public IList<RoutingEntry> FindRoutingEntries(string path)
        {
            //prepare list for found routing entries
            IList<RoutingEntry> routingEntries = new List<RoutingEntry>();

            //1. Split the path into elements
            string[] elements = path.Split('/');

            //2. Search Routing Entries based on Breadth-First-Search (by iterating each path segment, starting with the HTTP method)
            //2.1 prepare lists (after each iteration the nextIteration list will be copied into the current iteration list)
            //contains the list of nodes whose childs should be searched in the current iteration:
            IList<TreeNode<string>> curentIteration = new List<TreeNode<string>>();

            //contains the list of nodes whose childs should be searched in the next iteration:
            IList<TreeNode<string>> nextIteration = new List<TreeNode<string>>();

            //start with the root node:
            curentIteration.Add(_root);

            //2.2 iterate over each path element starting with the highest element (root element)
            for(int i = 0; i < elements.Length + 1; i++)
            {
                //iterate over all nodes matching the last element
                foreach (TreeNode<string> node in curentIteration)
                {

                    //if we have already analyzed all elements (and the underlying node has matched the last element)
                    if (i == elements.Length)
                    {
                        //all childs may be targetID nodes (but this is not a must and must be checked)
                        for (int j = 0; j < node.Count; j++)
                        {
                            TreeNode<string> n = node.Get(j);
                            //only if they are target id nodes
                            if (n is RoutingEntry)
                            {
                                routingEntries.Add((RoutingEntry)n);
                            }
                        }
                    }
                    //in case: elements[0]...elements[Lengths -1]
                    else
                    {
                        //1. Case: The node has an generic child node (which has no further Tree childs). Then the RoutingEntries of this generic child node are added, but the search will go one whether there are more specific Routing Entries.
                        if (node.HasChildWithValue("*"))
                        {
                            TreeNode<string> generic = node.GetChildWithValue("*");
                            //all childs may be RoutingEntries (but check this!!)
                            for (int j = 0; j < generic.Count; j++)
                            {
                                TreeNode<string> n = generic.Get(j);
                                //only if they are target id nodes
                                if (n is RoutingEntry)
                                {
                                    routingEntries.Add((RoutingEntry)n);
                                }
                            }
                        }
                        //2. and 3. Cases: There are nodes matching exactly the element or the there are nodes with a variable part
                        TreeNode<string> exactly = null;
                        TreeNode<string> variable = null;

                        //2. Case:
                        if (i != 0) //do not lower first element since it is the HTTP method
                        {
                            exactly = node.GetChildWithValue(elements[i].ToLower());
                        }
                        else
                        {
                            exactly = node.GetChildWithValue(elements[i]);
                        }

                        //3. Case:
                        //add generic values (only if the element is not empty)
                        if (!elements[i].Equals(""))
                        {
                            variable = node.GetChildWithValue("{}");
                        }

                        //add both cases (only one should match)
                        if (exactly != null)
                        {
                            nextIteration.Add(exactly);
                        }
                        if (variable != null)
                        {
                            nextIteration.Add(variable);
                        }
                    }
                }
                //copy list of child nodes into list of the nodes which should be searched in course of the next iteration
                curentIteration = nextIteration;
                nextIteration = new List<TreeNode<string>>(); //clear next list
            }
            return routingEntries;

        }
        #endregion
        #region Output Methods (Debug)

        /// <summary>
        /// Returns the routing tree in an XML representation.
        /// </summary>
        /// <returns>XML structure</returns>
        public string ToXmlRoot()
        {
            string xml = "";//"<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"yes\"?>\n";
            xml += ToXml(_root);
            return xml;
        }

        /// <summary>
        /// Returns an XML structure representing the passed sub tree.
        /// </summary>
        /// <param name="node">sub tree</param>
        /// <returns>XML structure</returns>
        private string ToXml(TreeNode<string> node)
        {
            if (node != null)
            {
                string xml = "<" + node.GetType().Name + " value=\"" + node.Value + "\">\n";
                for (int i = 0; i < node.Count; i++)
                {
                    xml += ToXml(node.Get(i));
                }
                xml += "</" + node.GetType().Name + ">\n";
                return xml;
            }
            else
            {
                return "";
            }
        }


        public override string ToString()
        {
            //load all routing entries
            IList<RoutingEntry> routingEntries = GetAllRoutingEntries();

            string entries = "";
            foreach(RoutingEntry routingEntry in routingEntries)
            {
                entries += "[ID: " + routingEntry.Identifier + "]\t[" + routingEntry.Path + "]\n";
            }
            return entries;
        }

        /// <summary>
        /// Returns a list of all <see cref="RoutingEntry"/> objects contained within this tree. Note that the routing entries are ordered
        /// by depth-first search algorithm.
        /// </summary>
        /// <returns>list containing all routing entry objects</returns>
        public IList<RoutingEntry> GetAllRoutingEntries()
        {
            IList<RoutingEntry> routingEntries = new List<RoutingEntry>();
            GetAllUnderlyingRoutingEntries(_root, routingEntries);
            return routingEntries;
        }

        /// <summary>
        /// Adds all <see cref="RoutingEntry"/> objects contained within the passed sub tree to the passed list. Note that the routing entries are ordered
        /// by depth-first search algorithm.
        /// </summary>
        /// <param name="node">sub tree</param>
        /// <param name="concatList">list in which the found entries should be added</param>
        private void GetAllUnderlyingRoutingEntries(TreeNode<string> node, IList<RoutingEntry> concatList)
        {
            for(int i = 0; i < node.Count; i++)
            {
                TreeNode<string> child = node.Get(i);
                //if it is a routing entry (leaf) then add it to the list
                if(child is RoutingEntry)
                {
                    concatList.Add((RoutingEntry)child);
                }
                //otherwise go deeper into tree 
                else
                {
                    GetAllUnderlyingRoutingEntries(child, concatList);
                }
            }
        }

        #endregion

    }


}
