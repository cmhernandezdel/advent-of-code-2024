using System.Text.RegularExpressions;

namespace AdventOfCode24;

public static class Day3
{
    // Part one
    // Apply the regex, remove the mul() thing, convert the numbers to integers and multiply them.
    private static int GetMultiplicationsValue(string input)
    {
        var regex = new Regex(@"mul\([0-9]+,[0-9]+\)");
        var matches = regex.Matches(input);
        return matches.Sum(x =>
            x.Value
                .Replace("mul(", "")
                .Replace(")", "")
                .Split(',')
                .Select(int.Parse)
                .Aggregate(1, (y, acc) => acc *= y)
        );
    }

    // Part two
    // Another regex, we split by do() and don't(), but this time we keep them.
    // Our initial position is a "do", we apply part 1 until we find a don't, then we skip
    // until we find a do again.
    private static int GetMultiplicationsValueWithConditionals(string input)
    {
        var res = 0;
        var substrings = Regex.Split(input, @"(do\(\))|(don't\(\))");
        var skip = false;
        foreach (var st in substrings)
        {
            if (st == "do()") skip = false;
            else if (st == "don't()") skip = true;
            else
            {
                if (skip) continue;
                res += GetMultiplicationsValue(st);
            }
        }

        return res;
    }
    
    public static void Solve(string filename)
    {
        var input = File.ReadAllText(filename);
        var mul = GetMultiplicationsValue(input);
        Console.WriteLine($"Multiplications result: {mul}");
        var mulWithCond = GetMultiplicationsValueWithConditionals(input);
        Console.WriteLine($"Multiplications with conditions result: {mulWithCond}");
    }
}