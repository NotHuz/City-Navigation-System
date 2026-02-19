using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_1_DSA.Models
{
    // Models/Route.cs
    public class Route
    {
        public List<string> Path { get; set; } = new List<string>();
        public double TotalDistance { get; set; }
        public double EstimatedTime { get; set; } // in minutes
        public string TrafficCondition { get; set; } = "Normal";
        public string RoadName { get; set; }


        public void DisplayRoute(Graph graph)
        {
            Console.WriteLine($"\nRoute Found!");
            Console.WriteLine($"Path: {string.Join(" → ", Path.Select(id => graph.Nodes[id].Name))}");
            Console.WriteLine($"Total Distance: {TotalDistance:F2} km");
            Console.WriteLine($"Estimated Time: {EstimatedTime:F1} minutes");
            Console.WriteLine($"Traffic: {TrafficCondition}");
        }
    }
}
