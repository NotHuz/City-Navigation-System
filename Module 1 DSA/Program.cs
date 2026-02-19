
// Program.cs
using Module_1_DSA.Models;
using Module_1_DSA.Services;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        //        Console.WriteLine("City Navigation System");
        //        Console.WriteLine("==========================\n");

        //        // Step 1: Generate synthetic city
        //        //var cityGenerator = new SyntheticCityGenerator();
        //        var city = cityGenerator.GenerateTestCity();

        //        // Display the city map
        //        city.DisplayGraph();

        //        // Step 2: Initialize path finder
        //        var pathFinder = new PathFinder(city);

        //        // Step 3: Test different routes
        //        TestRoute(pathFinder, city, "HOME", "PARK", "morning");
        //        TestRoute(pathFinder, city, "UNI", "CAFE", "afternoon");
        //        TestRoute(pathFinder, city, "MALL", "STADIUM", "evening");

        //        // Step 4: Interactive menu
        //        RunInteractiveMenu(pathFinder, city);
        //    }

        //    static void TestRoute(PathFinder finder, Graph city, string from, string to, string timeOfDay)
        //    {
        //        Console.WriteLine($"\n Testing {timeOfDay} route from {city.Nodes[from].Name} to {city.Nodes[to].Name}:");

        //        var route = finder.FindFastestPath(from, to, timeOfDay);
        //        route.DisplayRoute(city);
        //    }

        //    static void RunInteractiveMenu(PathFinder finder, Graph city)
        //    {
        //        while (true)
        //        {
        //            Console.WriteLine("\n Interactive Navigation");
        //            Console.WriteLine("1. Find fastest route");
        //            Console.WriteLine("2. Find shortest route");
        //            Console.WriteLine("3. Show city map");
        //            Console.WriteLine("4. Exit");
        //            Console.Write("Choose an option: ");

        //            string choice = Console.ReadLine();

        //            switch (choice)
        //            {
        //                case "1":
        //                    FindRoute(finder, city, "fastest");
        //                    break;
        //                case "2":
        //                    FindRoute(finder, city, "shortest");
        //                    break;
        //                case "3":
        //                    city.DisplayGraph();
        //                    break;
        //                case "4":
        //                    Console.WriteLine("Goodbye! 👋");
        //                    return;
        //                default:
        //                    Console.WriteLine("Invalid option. Please try again.");
        //                    break;
        //            }
        //        }
        //    }

        //    static void FindRoute(PathFinder finder, Graph city, string routeType)
        //    {
        //        Console.WriteLine("\nAvailable locations:");
        //        foreach (var node in city.Nodes.Values)
        //        {
        //            Console.WriteLine($"  {node.Id}: {node.Name}");
        //        }

        //        Console.Write("Enter start location ID: ");
        //        string from = Console.ReadLine().ToUpper();

        //        Console.Write("Enter destination ID: ");
        //        string to = Console.ReadLine().ToUpper();

        //        Console.Write("Enter time of day (morning/afternoon/evening/night): ");
        //        string timeOfDay = Console.ReadLine().ToLower();

        //        if (!city.Nodes.ContainsKey(from) || !city.Nodes.ContainsKey(to))
        //        {
        //            Console.WriteLine("❌ Invalid location IDs. Please try again.");
        //            return;
        //        }

        //        try
        //        {
        //            Route route = routeType == "fastest"
        //                ? finder.FindFastestPath(from, to, timeOfDay)
        //                : finder.FindShortestPath(from, to, timeOfDay);

        //            route.DisplayRoute(city);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"❌ Error finding route: {ex.Message}");
        //        }
    }
}