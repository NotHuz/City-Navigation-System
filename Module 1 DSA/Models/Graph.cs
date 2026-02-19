using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_1_DSA.Models
{
    public class Graph
    {
        public Dictionary<string, Node> Nodes { get; set; } = new();
        public Dictionary<string, List<Edge>> AdjacencyList { get; set; } = new();

        public void AddNode(string id, double lat, double lon, string name = null, bool isPlace = false)
        {
            if (!Nodes.ContainsKey(id))
                Nodes[id] = new Node(id, lat, lon, name, isPlace);

            if (!AdjacencyList.ContainsKey(id))
                AdjacencyList[id] = new List<Edge>();
        }

        public void AddEdge(string from, string to, double distance, string roadType)
        {
            var edge = new Edge(from, to, distance, roadType);
            var reverse = new Edge(to, from, distance, roadType);

            if (!AdjacencyList.ContainsKey(from))
                AdjacencyList[from] = new List<Edge>();

            if (!AdjacencyList.ContainsKey(to))
                AdjacencyList[to] = new List<Edge>();

            AdjacencyList[from].Add(edge);
            AdjacencyList[to].Add(reverse);
        }
   



        private Dictionary<string, double> CreateDefaultTrafficPattern()
        {
            return new Dictionary<string, double>
            {
                ["morning"] = 1.5,  // 50% slower in morning
                ["afternoon"] = 1.2, // 20% slower in afternoon  
                ["evening"] = 1.8,   // 80% slower in evening
                ["night"] = 1.0      // Normal speed at night
            };
        }

        public void DisplayGraph()
        {
            Console.WriteLine("\n🗺️  City Map:");
            foreach (var node in Nodes.Values)
            {
                Console.WriteLine($"📍 {node.Name} ({node.Id})");
                foreach (var edge in AdjacencyList[node.Id])
                {
                    var targetNode = Nodes[edge.ToNodeId];
                    Console.WriteLine($"   → {targetNode.Name} ({edge.Distance}km, {edge.RoadType})");
                }
            }
        }
    }
}
