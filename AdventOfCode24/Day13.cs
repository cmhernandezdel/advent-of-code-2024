using System.Text.RegularExpressions;

namespace AdventOfCode24;

public class Day13
{
    private const int MaxPushes = 100;
    
    private record Point2d(int X, int Y)
    {
        public override string ToString() => $"({X}, {Y})";
    }

    private record Machine(Point2d ButtonA, Point2d ButtonB, Point2d Prize)
    {
        public override string ToString() => $"ButtonA: {ButtonA}, ButtonB: {ButtonB}, Prize: {Prize})";
    }

    private class Node(Point2d p, int a, int b)
    {
        public Point2d Point { get; } = p;
        public int Distance { get; } = a * 3 + b;

        public int PressedA { get; } = a;
        
        public int PressedB { get; } = b;
        
        public override bool Equals(object? obj)
        {
            return obj is Node node && Point == node.Point;
        }
        
        public override int GetHashCode() => Point.GetHashCode();
    }
    
    private static List<Machine> ParseInput(string filename)
    {
        var input = File.ReadAllText(filename);
        var machines = new List<Machine>();
        
        const string pattern = @"Button A: X\+(\d+), Y\+(\d+)\n" +
                               @"Button B: X\+(\d+), Y\+(\d+)\n" +
                               @"Prize: X=(\d+), Y=(\d+)";
        
        var matches = Regex.Matches(input, pattern);

        foreach (Match match in matches)
        {
            Point2d a = new Point2d(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            Point2d b = new Point2d(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            Point2d p = new Point2d(int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value));
            
            var machine = new Machine(a, b, p);
            machines.Add(machine);
        }

        return machines;
    }

    // There should be room for improvement here because it takes a little while
    private static int Dijkstra(Point2d start, Point2d target, Point2d buttonA, Point2d buttonB)
    {
        int d = -1;
        HashSet<Node> unvisited = [];
        unvisited.Add(new Node(start, 0, 0));
        
        while (unvisited.Count != 0)
        {
            var current = unvisited.MinBy(n => n.Distance)!;
            unvisited.Remove(current);

            if (current is { PressedA: > MaxPushes, PressedB: > MaxPushes })
            {
                break;
            }

            if (current.Point == target)
            {
                d = current.Distance;
                break;
            }
            
            var neighborA = new Node(new Point2d(current.Point.X + buttonA.X, current.Point.Y + buttonA.Y), current.PressedA + 1, current.PressedB);
            var neighborB = new Node(new Point2d(current.Point.X + buttonB.X, current.Point.Y + buttonB.Y), current.PressedA, current.PressedB + 1);

            // If point is present but its calculated distance is greater than the distance along this path, re-add it.
            if (unvisited.Contains(neighborA) && unvisited.First(n => n.Point == neighborA.Point).Distance > neighborA.Distance)
            {
                unvisited.Remove(neighborA);
                unvisited.Add(neighborA);
            }
            
            // And if not present, add
            else if (!unvisited.Contains(neighborA))
            {
                unvisited.Add(neighborA);
            }
            
            // Do the same for the other button press
            if (unvisited.Contains(neighborB) && unvisited.First(n => n.Point == neighborB.Point).Distance > neighborB.Distance)
            {
                unvisited.Remove(neighborB);
                unvisited.Add(neighborB);
            }
            
            else if (!unvisited.Contains(neighborB))
            {
                unvisited.Add(neighborB);
            }
        }
        
        
        return d;
    }


    public static void Solve(string filename)
    {
        List<Machine> machines = ParseInput(filename);
        int totalDistance = 0;
        for (int i = 0; i < machines.Count; i++)
        {
            Console.WriteLine($"Calculating distance for machine {machines[i]}");
            int distance = Dijkstra(new Point2d(0,0), machines[i].Prize, machines[i].ButtonA, machines[i].ButtonB);
            if (distance != -1) totalDistance += distance;
        }
        
        Console.WriteLine($"Number of tokens: {totalDistance}");
    }
}