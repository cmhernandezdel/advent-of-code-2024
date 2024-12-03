using System.Text.RegularExpressions;

namespace AdventOfCode24;

public static class Day1
{
    // parse input into 2 lists
    private static Tuple<ICollection<int>, ICollection<int>> Parse(string filename)
    {
        var spaceRegex = new Regex(@"\s+");
        var l1 = new List<int>();
        var l2 = new List<int>();
        var lines = File.ReadAllLines(filename);
        foreach (var line in lines)
        {
            // replace multiple spaces for just one
            var trimmed = spaceRegex.Replace(line, " ");
            // split by spaces
            var pStr = trimmed.Split(" ");
            // convert to int
            var v = pStr.Select(s => Convert.ToInt32(s.Trim())).ToArray();
            
            // add to return lists
            l1.Add(v[0]);
            l2.Add(v[1]);
        }

        return new Tuple<ICollection<int>, ICollection<int>>(l1, l2);
    }

    // Part one
    // This one is easy although maybe this could be achieved at a lower time complexity.
    // We order the lists and calculate the distance between them.
    private static int CalculateDistance(Tuple<ICollection<int>, ICollection<int>> input)
    {
        var l1 = input.Item1.Order().ToList();
        var l2 = input.Item2.Order().ToList();
        var distance = 0;

        for (var i = 0; i < l1.Count; ++i)
        {
            distance += Math.Abs(l1[i] - l2[i]);
        }

        return distance;
    }

    // Part two
    // We group one of the lists and multiply its key by the number of appearances in that list
    // given that the key appears too in the first list.
    private static int CalculateSimilarity(Tuple<ICollection<int>, ICollection<int>> input)
    {
        var similarity = 0;
        var grouped = input.Item2.GroupBy(i => i).ToList();
        foreach (var l in input.Item1)
        {
            var g = grouped.FirstOrDefault(g => g.Key == l);
            if (g is null) continue;
            similarity += l * g.Count();
        }

        return similarity;
    }

    public static void Solve(string filename)
    {
        var parsedInput = Parse(filename);
        var distance = CalculateDistance(parsedInput);
        var similarity = CalculateSimilarity(parsedInput);
        Console.WriteLine($"Distance: {distance}");
        Console.WriteLine($"Similarity: {similarity}");
    }
}