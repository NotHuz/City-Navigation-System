using Module_1_DSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_1_DSA.Services
{
    public class PathFinder
    {
        private Graph _graph;

        public PathFinder(Graph graph)
        {
            _graph = graph;
        }

        public Route FindShortestPath(string startId, string endId, string timeOfDay = "normal")
        {
            // Dijkstra's algorithm for shortest distance
            return FindOptimalPath(startId, endId, timeOfDay, "distance");
        }

        public Route FindFastestPath(string startId, string endId, string timeOfDay = "normal")
        {
            // Dijkstra's algorithm for fastest time (considering traffic)
            return FindOptimalPath(startId, endId, timeOfDay, "time");
        }

        private Route FindOptimalPath(string startId, string endId, string timeOfDay, string criteria)
        {
            var distances = new Dictionary<string, double>();
            var previous = new Dictionary<string, string>();
            var visited = new HashSet<string>();

            // Simple priority queue using sorted list
            var priorityQueue = new List<(string nodeId, double priority)>();

            // Initialize
            foreach (var nodeId in _graph.Nodes.Keys)
            {
                distances[nodeId] = double.MaxValue;
            }
            distances[startId] = 0;
            priorityQueue.Add((startId, 0));

            while (priorityQueue.Count > 0)
            {
                // Get node with smallest distance
                priorityQueue.Sort((a, b) => a.priority.CompareTo(b.priority));
                var (currentNodeId, currentDistance) = priorityQueue[0];
                priorityQueue.RemoveAt(0);

                if (visited.Contains(currentNodeId))
                    continue;

                visited.Add(currentNodeId);

                if (currentNodeId == endId)
                    break;

                foreach (var edge in _graph.AdjacencyList[currentNodeId])
                {
                    if (visited.Contains(edge.ToNodeId))
                        continue;

                    double edgeWeight = criteria == "time"
                        ? CalculateTravelTime(edge, timeOfDay)
                        : edge.Distance;

                    double newDistance = distances[currentNodeId] + edgeWeight;

                    if (newDistance < distances[edge.ToNodeId])
                    {
                        distances[edge.ToNodeId] = newDistance;
                        previous[edge.ToNodeId] = currentNodeId;
                        priorityQueue.Add((edge.ToNodeId, newDistance));
                    }
                }
            }

            if (!previous.ContainsKey(endId) && startId != endId)
                throw new Exception("No route found between selected locations");

            return ReconstructRoute(previous, endId, distances[endId], timeOfDay, criteria);

        }

        private double CalculateTravelTime(Edge edge, string timeOfDay)
        {
            //double trafficFactor = _graph.GetTrafficFactor(edge.FromNodeId, edge.ToNodeId, timeOfDay);
            double trafficMultiplier = timeOfDay switch
            {
                "morning" => edge.RoadType == "HIGHWAY" ? 1.4 : 1.7,
                "afternoon" => edge.RoadType == "HIGHWAY" ? 1.2 : 1.4,
                "evening" => edge.RoadType == "HIGHWAY" ? 1.6 : 2.0,
                "night" => 0.9,
                _ => 1.0
            };

            double baseTime = (edge.Distance / edge.BaseSpeed) * 60;
            return baseTime * trafficMultiplier;

        }

        private Route ReconstructRoute(Dictionary<string, string> previous, string endId,
                                     double totalCost, string timeOfDay, string criteria)
        {
            var path = new List<string>();
            string current = endId;

            while (previous.ContainsKey(current))
            {
                path.Insert(0, current);
                current = previous[current];
            }
            path.Insert(0, current);

            // Calculate actual distance and time for the route
            double totalDistance = 0;
            double totalTime = 0;

            for (int i = 0; i < path.Count - 1; i++)
            {
                var edge = _graph.AdjacencyList[path[i]]
                .FirstOrDefault(e => e.ToNodeId == path[i + 1]);

                if (edge == null)
                    throw new Exception($"Missing edge between {path[i]} and {path[i + 1]}");

                totalDistance += edge.Distance;
                totalTime += CalculateTravelTime(edge, timeOfDay);
            }

            string trafficCondition = timeOfDay switch
            {
                "morning" => "Heavy ",
                "evening" => "Very Heavy ",
                "afternoon" => "Moderate ",
                _ => "Light "
            };

            return new Route
            {
                Path = path,
                TotalDistance = totalDistance,
                EstimatedTime = totalTime,
                TrafficCondition = trafficCondition
            };
        }


    }
}
