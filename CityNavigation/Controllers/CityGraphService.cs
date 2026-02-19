using Module_1_DSA.Services;
using Module_1_DSA.Models;

public class CityGraphService
{
    public Graph Graph { get; }
    public PathFinder PathFinder { get; }

    public CityGraphService()
    {
        var generator = new AutomatedKarachiGenerator();
        Graph = generator.GenerateKarachi();
        PathFinder = new PathFinder(Graph);

        Console.WriteLine(
            $"[Graph Loaded] Places with edges: {Graph.Nodes.Values.Count(n => n.IsPlace && Graph.AdjacencyList[n.Id].Any())}"
        );
    }
}
