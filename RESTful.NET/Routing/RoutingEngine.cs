using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Routing
{
    public class RoutingEngine
    {
        private RoutingTree _routingTree = new RoutingTree();

        private int _counter = 0;

        public void AddEntry(string destinationPath, string processingGroup, HttpController httpController, MethodInfo methodInfo)
        {
            //1. check whether path is valid
            if (!IsPathValid(destinationPath))
            {
                throw new Exception("Destination Path is in an invalid format: " + destinationPath);
            }
            //2. check whether path is distinct
            if (!IsPathDisjunct(destinationPath))
            {
                throw new Exception("Destination Path is not distinct: " + destinationPath);
            }

            //3. create and add route   
            RoutingEntry routingEntry = new RoutingEntry(processingGroup, httpController, methodInfo, "Rx" + _counter++, destinationPath, destinationPath.Split('/').Length);
            _routingTree.AddPath(destinationPath, routingEntry);
        }

        /// <summary>
        /// Determines a routing target and returns the result as a routing entry. If null is returned, the target cannot be resolved.
        /// </summary>
        /// <param name="path">HTTP Method + URL</param>
        /// <returns>Routing entry or null</returns>
        public RoutingEntry GetEntry(string path)
        {
            //1. find target identifiers
            IList<RoutingEntry> routingEntries = _routingTree.FindRoutingEntries(path);

            if (routingEntries.Count == 0)
            {
                //no entries found
                return null;
            }
            else
            {
                //priorize Routing Entries with no generic element
                foreach (RoutingEntry routingEntry in routingEntries)
                {
                    //search for non generic first and return the first occurence:
                    if (!routingEntry.Path.Contains("*"))
                    {
                        return routingEntry;
                    }

                }
                //if only generic Routing Entries have been found, return the generic Routing Entry with the highest metric

                RoutingEntry generic = null;
                foreach (RoutingEntry routingEntry in routingEntries)
                {
                    if(generic == null || generic.Metrics < routingEntry.Metrics)
                    {
                        generic = routingEntry;
                    }

                }
                return generic;
            }

        }

        public bool IsPathValid(string path)
        {
            string[] elements = path.Split('/');

            //check minimum path length
            if(elements.Length < 2)
            {
                return false;
            }

            //check whether '*' is in between
            if (path.Substring(0, path.Length - 1).Contains("*"))
            {
                return false;
            }

            //otherwise the path is valid
            return true;

        }

        public bool IsPathDisjunct(string destinationPath)
        {
            /* TODO: Implement new approach:
             * - compare only routing entries of same lenght (step 1)
             * - compare each element, if all elements matches, the path is not distinct
             * 
             * COMMENT: A wildcard "*" is not relevant since coexistency is supported --> the highest metric rules (TODO: But this also means, that there could be two identical wildcards paths)
             * */

            

            //DO NOT COMPARE WILDCARDS --> Automatically true
            if (destinationPath.Contains('*'))
            {
                return true;
            }

            string[] elements = destinationPath.Split('/');
            IList<RoutingEntry> routingEntries = _routingTree.GetAllRoutingEntries();

            //for each routing entry
            foreach(RoutingEntry routingEntry in routingEntries)
            {
                //compare only routing entries of the same length AND the routing entry is free of any wildcards "*"
                if(routingEntry.Metrics == elements.Length && !routingEntry.Path.Contains("*"))
                {
                    //count all matches within one path (if all elements matches, the path is not disjunct)
                    int matchCounter = 0;

                    //check for each element of the origin destination path, whether the element at the same position matches
                    for(int i = 0; i < elements.Length; i++)
                    {
                        
                        //if the element of the destination path is a parameter
                        if (elements[i].Contains('{'))
                        {
                            //it automatically matches
                            matchCounter++;
                        }
                        else
                        {
                            //if the element of the routing entry is equals or is a parameter
                            if(elements[i].CompareTo(routingEntry.Path.Split('/')[i])==0 || routingEntry.Path.Split('/')[i].Contains('{'))
                            {
                                matchCounter++;
                            }
                        }
                    }
                    if (matchCounter == elements.Length)
                    {
                        return false;
                    }
                }
            }

            return true;

            /*

            
            IList<RoutingEntry> currentSelection = _routingTree.GetAllRoutingEntries();
            IList<RoutingEntry> nextSelection = new List<RoutingEntry>();

            string[] elements = destinationPath.Split('/');

            //iterate over each elements
            for(int i = 0; i < elements.Length; i++)
            {
                foreach(RoutingEntry routingEntry in currentSelection)
                {
                    if (elements[i].Contains("{"))
                    {
                        //since {...} is a wildcard, evey Routing Entry having a path of this length matches
                        if (routingEntry.Path.Split('/').Length > i)
                        {
                            nextSelection.Add(routingEntry);
                        }
                    }
                    //gnore (can be treaten like a normal symbol)
                    else if (elements[i].Contains("*"))
                    {
                       
                    }
                    //ingore END
                    else
                    {
                        string[] splitted = routingEntry.Path.Split('/');
                        if (splitted.Length > i && (splitted[i].CompareTo(elements[i])==0 || splitted[i].Contains("{")))
                        {
                            nextSelection.Add(routingEntry);
                        }

                    }
                    
                }
                currentSelection = nextSelection; //after all routing entries has been checked, copy next selection list into current selection list and go to next element
                nextSelection = new List<RoutingEntry>(); //clear list
            }
            //finally check, whether there are Routing Entries left matching the destination path
            if(currentSelection.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            */
            
        }

        /// <summary>
        /// Gets all routing entries as a dictionary
        /// </summary>
        public IReadOnlyDictionary<string, RoutingEntry> RoutingEntries
        {
            get
            {
                IList<RoutingEntry> routingEntries = _routingTree.GetAllRoutingEntries();
                IDictionary<string, RoutingEntry> dict = new Dictionary<string, RoutingEntry>();

                foreach(RoutingEntry routingEntry in routingEntries)
                {
                    dict.Add(routingEntry.Identifier, routingEntry);
                }

                return new ReadOnlyDictionary<string, RoutingEntry>(dict);
            }
        }


        public override string ToString()
        {
            return _routingTree.ToString();
        }

        public string TreeToXml()
        {
            return _routingTree.ToXmlRoot();
        }

    }
}
