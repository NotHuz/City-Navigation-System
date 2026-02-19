using Module_1_DSA.Models;
using OsmSharp;
using OsmSharp.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Module_1_DSA.Services
{
    public class AutomatedKarachiGenerator
    {
        public List<Place> Places { get; private set; } = new();

        private const double MIN_LAT = 24.85;
        private const double MAX_LAT = 24.88;
        private const double MIN_LON = 66.99;
        private const double MAX_LON = 67.03;

        private bool Inside(double lat, double lon)
        {
            return lat >= MIN_LAT && lat <= MAX_LAT &&
                   lon >= MIN_LON && lon <= MAX_LON;
        }

        public Graph GenerateKarachi()
        {
            var graph = new Graph();
            var osmPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot", "data", "export.osm");

            using var fs = File.OpenRead(osmPath);
            var source = new XmlOsmStreamSource(fs);

            var osmNodes = new Dictionary<long, OsmSharp.Node>();
            var ways = new List<Way>();

            foreach (var geo in source)
            {
                if (geo is OsmSharp.Node n &&
                    n.Latitude.HasValue && n.Longitude.HasValue &&
                    Inside(n.Latitude.Value, n.Longitude.Value))
                {
                    osmNodes[n.Id.Value] = n;

                    bool isPlace = n.Tags?.ContainsKey("name") == true;
                    string name = isPlace ? n.Tags["name"] : null;

                    graph.AddNode(
                        n.Id.Value.ToString(),
                        n.Latitude.Value,
                        n.Longitude.Value,
                        name,
                        isPlace
                    );
                }
                else if (geo is Way w)
                {
                    ways.Add(w);
                }
            }

            foreach (var w in ways)
            {
                if (w.Tags == null || !w.Tags.ContainsKey("highway")) continue;
                if (w.Nodes == null || w.Nodes.Length < 2) continue;

                for (int i = 0; i < w.Nodes.Length - 1; i++)
                {
                    if (!osmNodes.ContainsKey(w.Nodes[i]) ||
                        !osmNodes.ContainsKey(w.Nodes[i + 1]))
                        continue;

                    var a = osmNodes[w.Nodes[i]];
                    var b = osmNodes[w.Nodes[i + 1]];

                    string highway = w.Tags["highway"];

                    string roadType = highway switch
                    {
                        "motorway" or "trunk" => "HIGHWAY",
                        "primary" => "MAIN_ROAD",
                        "secondary" => "ARTERIAL_ROAD",
                        "tertiary" => "ARTERIAL_ROAD",
                        _ => "LOCAL_ROAD"
                    };

                    graph.AddEdge(
                        w.Nodes[i].ToString(),
                        w.Nodes[i + 1].ToString(),
                        Haversine(a.Latitude.Value, a.Longitude.Value,
                                  b.Latitude.Value, b.Longitude.Value),
                        roadType
                    );

                }
            }
            foreach (var node in graph.Nodes.Values.Where(n => n.IsPlace))
            {
                var nearestRoadId = FindNearestRoadNode(
                    graph,
                    node.Latitude,
                    node.Longitude
                );

                if (nearestRoadId != null)
                {
                    double dist = Haversine(
                        node.Latitude, node.Longitude,
                        graph.Nodes[nearestRoadId].Latitude,
                        graph.Nodes[nearestRoadId].Longitude
                    );

                    // Bidirectional connection
                    graph.AddEdge(node.Id, nearestRoadId, dist, "LOCAL_ROAD");
                }
            }

            return graph;
        }
        private string FindNearestRoadNode(Graph graph, double lat, double lon)
        {
            string nearestId = null;
            double minDist = double.MaxValue;

            foreach (var node in graph.Nodes.Values)
            {
                // Look for nodes that have edges (connected to network)
                if (!graph.AdjacencyList.ContainsKey(node.Id) ||
                    !graph.AdjacencyList[node.Id].Any())
                    continue;

                double d = Haversine(lat, lon, node.Latitude, node.Longitude);
                if (d < minDist && d < 0.5) // Within 500 meters
                {
                    minDist = d;
                    nearestId = node.Id;
                }
            }
            return nearestId;
        }


        private double Haversine(double lat1, double lon1,
                                 double lat2, double lon2)
        {
            double R = 6371;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;

            lat1 *= Math.PI / 180;
            lat2 *= Math.PI / 180;

            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            return 2 * R * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }
    }
}
