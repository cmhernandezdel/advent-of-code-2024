using AdventOfCode24.Shared;

namespace AdventOfCode24;

public class Day10
{
    private readonly Dictionary<Point2d, int> grid = [];
    private int gridWidth;
    private int gridHeight;

    private const int MaxValue = 9;
    
    private void Parse(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        gridHeight = lines.Length;
        gridWidth = lines[0].Length;
        
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                char c = line[j];
                int n = int.Parse(c.ToString());
                grid.Add(new Point2d(i, j), n);
            }
        }
    }
    
    // Part one
    // Find the trailhead starting points and do a breadth-first search until we reach a summit
    // Don't count repeats
    // Part two
    // Do the same but now count repeats
    private List<Point2d> FindTrailheadStartingPoints(int start)
    {
        return grid.Where(x => x.Value == start)
            .Select(x => x.Key)
            .ToList();
    }

    private int FindTrailheadScore(List<Point2d> startingPoints, bool countRepeated)
    {
        int trailheadScore = 0;
        foreach (Point2d startingPoint in startingPoints)
        {
            trailheadScore += FindTrailheadScore(startingPoint, countRepeated);
        }

        return trailheadScore;
    }

    private int FindTrailheadScore(Point2d startingPoint, bool countRepeated)
    {
        Queue<Tuple<Point2d, int>> toVisit = [];
        HashSet<Point2d> summits = [];
        toVisit.Enqueue(new Tuple<Point2d, int>(startingPoint, 0));

        while (toVisit.Count > 0)
        {
            Tuple<Point2d, int> currentPointInfo = toVisit.Peek();
            Point2d currentPoint = currentPointInfo.Item1;
            int currentValue = currentPointInfo.Item2;

            // success exit condition
            if (currentValue == MaxValue)
            {
                break;
            }

            toVisit.Dequeue();
            List<Point2d> points = GeneratePoints(currentPoint, currentValue + 1);
            points.ForEach(p => toVisit.Enqueue(new Tuple<Point2d, int>(p, currentValue + 1)));
        }

        foreach (var x in toVisit)
        {
            summits.Add(x.Item1);
        }

        return countRepeated ? toVisit.Count : summits.Count;
    }

    private List<Point2d> GeneratePoints(Point2d startingPoint, int nextValue)
    {
        List<Point2d> l = [
            startingPoint + new Point2d(-1, 0), 
            startingPoint + new Point2d(1, 0),
            startingPoint + new Point2d(0, 1),
            startingPoint + new Point2d(0, -1)
        ];

        foreach (Point2d p in l.ToList())
        {
            if (IsOutOfBounds(p) || grid[p] != nextValue) l.Remove(p);
        }

        return l;
    }

    private bool IsOutOfBounds(Point2d p)
    {
        return p.I < 0 || p.I >= gridHeight || p.J < 0 || p.J >= gridWidth;
    }

    public void Solve(string filename)
    {
        Parse(filename);
        List<Point2d> startingPoints = FindTrailheadStartingPoints(0);
        int score1 = FindTrailheadScore(startingPoints, countRepeated: false);
        Console.WriteLine($"Trailhead score (part 1): {score1}");
        int score2 = FindTrailheadScore(startingPoints, countRepeated: true);
        Console.WriteLine($"Trailhead score (part 2): {score2}");
    }
}