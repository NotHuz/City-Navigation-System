namespace Module_1_DSA.Models
{
    // Represents both road-nodes and place-nodes
    public class Node
    {
        public string Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Human readable name (only for places)
        public string Name { get; set; }

        // TRUE = real place (hospital, area, shop, etc)
        // FALSE = road/intersection node (hidden from user)
        public bool IsPlace { get; set; }

        public Node(string id, double lat, double lon, string name = null, bool isPlace = false)
        {
            Id = id;
            Latitude = lat;
            Longitude = lon;
            Name = name;
            IsPlace = isPlace;
        }
    }
}
