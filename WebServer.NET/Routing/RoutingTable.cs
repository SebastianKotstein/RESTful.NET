using SKotstein.Net.Http.Context;
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
    /// <summary>
    /// The routing table consists of a routing tree mapping an HTTP Method + URL to an internal identifier and a dictionary mapping this identifiers to <see cref="RoutingEntry"/>'s containing details of the target method to be invoked.
    /// </summary>
    public class RoutingTable
    {
        private RoutingTree _routingTree = new RoutingTree();

        private IDictionary<string, RoutingEntry> _routingEntries = new Dictionary<string, RoutingEntry>();

        private int _counter = 0;

        /// <summary>
        /// Adds a new entry to the routing table.
        /// A routing entry consists of an HTTP Method, an URL, a processing group with its associated HttpProcessor, an HttpController hosting the method to be invoked
        /// and the method info containing details about the method.
        /// This method may throw an exception if a HTTP Method + URL is already mapped to a method (only distinct mappings are allowed)
        /// </summary>
        /// <param name="destinationPath">HTTP Method + URL </param>
        /// <param name="processingGroup">name of the processing group</param>
        /// <param name="httpController">HttpController hosting the method</param>
        /// <param name="methodInfo">details about the method</param>
        public void AddEntry(string destinationPath, string processingGroup, HttpController httpController, MethodInfo methodInfo)
        {
           

            //2. generate ID:
            string id = "Rx" + _counter++;

            //3. check whether path is already tagged with an identifier:
            if(_routingTree.FindTargetIDs(destinationPath).Count > 0)
            {
                throw new Exception("Path is already in use! Check whether your HTTP paths are distinct");
            }

            //4. Add routing entry:
            _routingEntries.Add(id, new RoutingEntry(processingGroup, httpController, methodInfo, id, destinationPath));

            //5. Add path to tree:
            _routingTree.AddPath(destinationPath, id);
        }

        /// <summary>
        /// Determines a routing target and returns the result as a routing entry. If null is returned, the target cannot be resolved.
        /// </summary>
        /// <param name="destionationPath">HTTP Method + URL</param>
        /// <returns>Routing entry or null</returns>
        public RoutingEntry GetEntry(string destionationPath)
        {
            //1. find target identifier
            IList<string> targetIds = _routingTree.FindTargetIDs(destionationPath);

            if(targetIds.Count > 1)
            {
                throw new Exception("Failure: More than one matching routing entries found.");
            }
            else if(targetIds.Count == 0)
            {
                //no entries found
                return null;
            }
            else
            {
                return _routingEntries[targetIds[0]];
            }

        }

        /// <summary>
        /// Gets all routing entries as a dictionary
        /// </summary>
        public IReadOnlyDictionary<string, RoutingEntry> RoutingEntries
        {
            get
            {
                return new ReadOnlyDictionary<string, RoutingEntry>(_routingEntries);
            }
        }

        public override string ToString()
        {
            string entries = "";
            foreach(string id in _routingEntries.Keys)
            {
                entries += "[ID: " + id + "]\t[" + _routingEntries[id].Path + "]\n";
            }
            return entries;
        }


    }
}
