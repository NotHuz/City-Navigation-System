using Microsoft.AspNetCore.Mvc;
using Module_1_DSA.Models;
using Module_1_DSA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Route = Module_1_DSA.Models.Route;

namespace CityNavigation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NavigationController : ControllerBase
    {
        private readonly Graph _cityGraph;
        private readonly PathFinder _pathFinder;

        public NavigationController(CityGraphService graphService)
        {
            _cityGraph = graphService.Graph;
            _pathFinder = graphService.PathFinder;
        }

        // ----------------------------
        // HEALTH CHECK FOR ROUTING SERVICES
        // ----------------------------
        [HttpGet("routing-status")]
        public IActionResult GetRoutingStatus()
        {
            return Ok(new
            {
                CustomGraphRouting = true,
                Message = "Custom graph routing is available",
                GraphNodesCount = _cityGraph.Nodes.Count,
                GraphEdgesCount = _cityGraph.AdjacencyList.Sum(kv => kv.Value.Count)
            });
        }

        // ----------------------------
        // ORIGINAL CUSTOM GRAPH ROUTING
        // ----------------------------
        [HttpGet("route")]
        public IActionResult GetRoute(string from, string to, string timeOfDay = "normal")
        {
            try
            {
                var (start, end) = ResolveRouteEndpoints(from, to);
                var route = _pathFinder.FindFastestPath(start.Id, end.Id, timeOfDay);

                // Return both path and coordinates
                return Ok(new
                {
                    Path = route.Path,
                    PathNames = route.Path.Select(id => _cityGraph.Nodes[id].Name).ToList(),
                    Coordinates = route.Path.Select(id => new
                    {
                        lat = _cityGraph.Nodes[id].Latitude,
                        lon = _cityGraph.Nodes[id].Longitude
                    }),
                    route.TotalDistance,
                    route.EstimatedTime,
                    route.TrafficCondition
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Debug
        [HttpGet("debug-graph")]
        public IActionResult DebugGraph()
        {
            var placesWithEdges = _cityGraph.Nodes.Values
                .Where(n => n.IsPlace)
                .Select(n => new
                {
                    id = n.Id,
                    name = n.Name,
                    isPlace = n.IsPlace,
                    edgeCount = _cityGraph.AdjacencyList.ContainsKey(n.Id)
                        ? _cityGraph.AdjacencyList[n.Id].Count
                        : 0,
                    hasEdges = _cityGraph.AdjacencyList.ContainsKey(n.Id)
                        && _cityGraph.AdjacencyList[n.Id].Any()
                })
                .ToList();

            return Ok(new
            {
                totalPlaces = placesWithEdges.Count,
                connectedPlaces = placesWithEdges.Count(p => p.hasEdges),
                disconnectedPlaces = placesWithEdges.Count(p => !p.hasEdges),
                places = placesWithEdges
            });
        }

        [HttpGet("test-route")]
        public IActionResult TestRoute()
        {
            // Find two places that have edges
            var connectedPlaces = _cityGraph.Nodes.Values
                .Where(n => n.IsPlace &&
                       _cityGraph.AdjacencyList.ContainsKey(n.Id) &&
                       _cityGraph.AdjacencyList[n.Id].Any())
                .Take(2)
                .ToList();

            if (connectedPlaces.Count < 2)
                return BadRequest(new { error = "Need at least 2 connected places" });

            var fromId = connectedPlaces[0].Id;
            var toId = connectedPlaces[1].Id;

            try
            {
                var route = _pathFinder.FindFastestPath(fromId, toId);

                return Ok(new
                {
                    message = "Route found successfully",
                    from = connectedPlaces[0].Name,
                    to = connectedPlaces[1].Name,
                    path = route.Path,
                    distance = route.TotalDistance,
                    time = route.EstimatedTime,
                    coordinates = route.Path.Select(id => new
                    {
                        lat = _cityGraph.Nodes[id].Latitude,
                        lon = _cityGraph.Nodes[id].Longitude
                    })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("debug-nodes")]
        public IActionResult GetAllNodes()
        {
            try
            {
                var nodes = _cityGraph.Nodes.Values.Select(n => new {
                    id = n.Id,
                    lat = n.Latitude,
                    lon = n.Longitude,
                    name = n.Name,
                    isPlace = n.IsPlace
                }).ToDictionary(n => n.id);

                return Ok(nodes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ----------------------------
        // OTHER ENDPOINTS
        // ----------------------------
        [HttpGet("search")]
        public IActionResult SearchNodes(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(new List<object>());

            q = q.ToLower();

            var results = _cityGraph.Nodes.Values
                .Where(n => n.IsPlace && n.Name != null && n.Name.ToLower().Contains(q))
                .Take(50)
                .Select(n => new
                {
                    id = n.Id,
                    name = n.Name,
                    lat = n.Latitude,
                    lon = n.Longitude
                });

            return Ok(results);
        }

        [HttpGet("nodes")]
        public IActionResult GetNodes(string ids)
        {
            var split = ids.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var result = split
                .Where(id => _cityGraph.Nodes.ContainsKey(id))
                .Select(id => {
                    var n = _cityGraph.Nodes[id];
                    return new { id = n.Id, lat = n.Latitude, lon = n.Longitude };
                });

            return Ok(result);
        }

        [HttpGet("route-options")]
        public IActionResult GetRouteOptions(string from, string to, string timeOfDay = "normal")
        {
            try
            {
                var (start, end) = ResolveRouteEndpoints(from, to);

                var fastest = _pathFinder.FindFastestPath(start.Id, end.Id, timeOfDay);
                var shortest = _pathFinder.FindShortestPath(start.Id, end.Id, timeOfDay);
                var balanced = FindBalancedRoute(start.Id, end.Id, timeOfDay);

                return Ok(new
                {
                    Fastest = ConvertToRouteResponse(fastest),
                    Shortest = ConvertToRouteResponse(shortest),
                    Balanced = ConvertToRouteResponse(balanced)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("node")]
        public IActionResult GetNode(string id)
        {
            if (string.IsNullOrEmpty(id) || !_cityGraph.Nodes.ContainsKey(id))
                return NotFound(new { error = "Node not found" });

            var n = _cityGraph.Nodes[id];
            return Ok(new
            {
                id = n.Id,
                name = n.Name,
                lat = n.Latitude,
                lon = n.Longitude
            });
        }

        [HttpGet("locations")]
        public IActionResult GetLocations()
        {
            return Ok(_cityGraph.Nodes.Values
                .Where(n => n.IsPlace)
                .Select(n => new {
                    id = n.Id,
                    name = n.Name,
                    lat = n.Latitude,
                    lon = n.Longitude
                }));
        }

        // ----------------------------
        // PRIVATE HELPER METHODS
        // ----------------------------
        private Node FindNearestNode(double lat, double lon)
        {
            Node nearest = null;
            double minDistance = double.MaxValue;

            foreach (var node in _cityGraph.Nodes.Values)
            {
                if (!_cityGraph.AdjacencyList[node.Id].Any())
                    continue;

                double distance = Haversine(lat, lon, node.Latitude, node.Longitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = node;
                }
            }
            return nearest;
        }

        private double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        private Route FindBalancedRoute(string from, string to, string timeOfDay)
        {
            var fastest = _pathFinder.FindFastestPath(from, to, timeOfDay);
            var shortest = _pathFinder.FindShortestPath(from, to, timeOfDay);

            double maxDist = Math.Max(fastest.TotalDistance, shortest.TotalDistance);
            double maxTime = Math.Max(fastest.EstimatedTime, shortest.EstimatedTime);

            double fastestScore =
                (fastest.TotalDistance / maxDist) * 0.4 +
                (fastest.EstimatedTime / maxTime) * 0.6;

            double shortestScore =
                (shortest.TotalDistance / maxDist) * 0.6 +
                (shortest.EstimatedTime / maxTime) * 0.4;

            return fastestScore <= shortestScore ? fastest : shortest;
        }

        private object ConvertToRouteResponse(Route route)
        {
            return new
            {
                Path = route.Path,
                PathNames = route.Path.Select(id => _cityGraph.Nodes[id].Name).ToList(),
                Coordinates = route.Path.Select(id => new
                {
                    lat = _cityGraph.Nodes[id].Latitude,
                    lon = _cityGraph.Nodes[id].Longitude
                }),
                TotalDistance = route.TotalDistance,
                EstimatedTime = route.EstimatedTime,
                TrafficCondition = route.TrafficCondition
            };
        }

        private (Node start, Node end) ResolveRouteEndpoints(string fromId, string toId)
        {
            if (!_cityGraph.Nodes.ContainsKey(fromId) || !_cityGraph.Nodes.ContainsKey(toId))
                throw new Exception("Invalid node IDs");

            var fromNode = _cityGraph.Nodes[fromId];
            var toNode = _cityGraph.Nodes[toId];

            if (!_cityGraph.AdjacencyList[fromNode.Id].Any())
                throw new Exception("Start node is not connected to road network");

            if (!_cityGraph.AdjacencyList[toNode.Id].Any())
                throw new Exception("End node is not connected to road network");

            return (fromNode, toNode);
        }
    }
}