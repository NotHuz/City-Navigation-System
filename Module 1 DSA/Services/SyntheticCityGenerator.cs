//using Module_1_DSA.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Module_1_DSA.Services
//{
//    public class SyntheticCityGenerator
//    {
//        public Graph GenerateTestCity()
//        {
//            var graph = new Graph();

//            // Add nodes (locations) with coordinates and names
//            graph.AddNode("HOME", 40.7589, -73.9851, "My Home");
//            graph.AddNode("UNI", 40.7505, -73.9934, "University Campus");
//            graph.AddNode("MALL", 40.7614, -73.9776, "Shopping Mall");
//            graph.AddNode("HOSPITAL", 40.7661, -73.9665, "City Hospital");
//            graph.AddNode("PARK", 40.7829, -73.9654, "Central Park");
//            graph.AddNode("STADIUM", 40.7505, -73.9934, "Sports Stadium");
//            graph.AddNode("CAFE", 40.7527, -74.0050, "Downtown Cafe");

//            // Add edges (roads) with distances and types
//            graph.AddEdge("HOME", "UNI", 3.2, "MAIN_ROAD");
//            graph.AddEdge("UNI", "MALL", 2.1, "LOCAL_ROAD");
//            graph.AddEdge("MALL", "HOSPITAL", 1.5, "ARTERIAL_ROAD");
//            graph.AddEdge("HOSPITAL", "PARK", 4.3, "HIGHWAY");
//            graph.AddEdge("PARK", "STADIUM", 2.8, "MAIN_ROAD");
//            graph.AddEdge("STADIUM", "CAFE", 1.2, "LOCAL_ROAD");
//            graph.AddEdge("CAFE", "HOME", 2.7, "ARTERIAL_ROAD");
//            graph.AddEdge("UNI", "CAFE", 1.8, "LOCAL_ROAD");
//            graph.AddEdge("MALL", "STADIUM", 3.1, "MAIN_ROAD");

//            // Add realistic traffic patterns
//            graph.AddTrafficPattern("HOME", "UNI", "morning", 1.8); // Heavy traffic to university
//            graph.AddTrafficPattern("UNI", "HOME", "evening", 1.8); // Heavy traffic from university
//            graph.AddTrafficPattern("MALL", "HOSPITAL", "afternoon", 1.4); // Moderate traffic
//            graph.AddTrafficPattern("HOSPITAL", "PARK", "evening", 2.0); // Very heavy traffic

//            return graph;
//        }
//    }
//}
