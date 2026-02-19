using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_1_DSA.Models
{
    // Models/Edge.cs
    public class Edge
    {
        public string FromNodeId { get; set; }
        public string ToNodeId { get; set; }
        public double Distance { get; set; } // in kilometers
        public string RoadType { get; set; }
        public double BaseSpeed { get; set; } // km/h

        public Edge(string from, string to, double distance, string roadType)
        {
            FromNodeId = from;
            ToNodeId = to;
            Distance = distance;
            RoadType = roadType;
            BaseSpeed = GetSpeedLimit(roadType);
        }

        private double GetSpeedLimit(string roadType)
        {
            return roadType switch
            {
                "HIGHWAY" => 100.0,
                "MAIN_ROAD" => 60.0,
                "ARTERIAL_ROAD" => 50.0,
                "LOCAL_ROAD" => 30.0,
                _ => 40.0
            };
        }
    }
}
