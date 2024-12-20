namespace AdventOfCode24;

public class Day11
{
    public const string ExampleInput = "125 17";
    public const string Input = "8793800 1629 65 5 960 0 138983 85629";

    // Here I thought I was super smart because I actually solved this on the 19th
    // and of course I had read the Reddit, but seems like caching the results this way
    // turns into memory issues really fast.
    
    // Then I tried to cache only the multiplications, but I got into memory issues as well, only a little bit later.
    
    // Reading a little bit more, turns out that I don't really need the list because the order is irrelevant,
    // so I just need a dictionary and the number of times that each stone appears.

    private readonly Dictionary<string, long> _originalStones = [];
    
    public void Solve(string input, int times)
    {
        string[] s = input.Split(" ");
        foreach (string stone in s)
        {
            AddOrUpdate(_originalStones, stone, 1);
        }

        for (int i = 0; i < times; i++)
        {
            var cp = _originalStones.ToDictionary();
            foreach (var x in cp)
            {
                var stone = x.Key;
                var appearances = x.Value;
                if (stone == "0")
                {
                    AddOrUpdate(_originalStones, "1", appearances);
                }
                else if (stone.Length % 2 == 0)
                {
                    string[] values = new string[2];
                    values[0] = Convert.ToInt64(stone[..(stone.Length / 2)]).ToString();
                    values[1] = Convert.ToInt64(stone[(stone.Length / 2)..]).ToString();
                    AddOrUpdate(_originalStones, values[0], appearances);
                    AddOrUpdate(_originalStones, values[1], appearances);
                }
                else
                {
                    long lValue = Convert.ToInt64(stone);
                    lValue *= 2024;
                    string newValue = lValue.ToString();
                    AddOrUpdate(_originalStones, newValue, appearances);
                }

                Remove(_originalStones, stone, appearances);
            }
        }

        long count = 0;
        foreach (var x in _originalStones)
        {
            count += x.Value;
        }
        
        Console.WriteLine($"Number of stones: {count}");
    }


    private static void AddOrUpdate(Dictionary<string, long> d, string stone, long times)
    {
        if (d.TryGetValue(stone, out long value))
        {
            d[stone] = value + times;
        }
        else
        {
            d.Add(stone, times);
        }
    }

    private static void Remove(Dictionary<string, long> d, string stone, long times)
    {
        if (!d.TryGetValue(stone, out long value)) return;
        d[stone] = value - times;
        if (d[stone] <= 0) d.Remove(stone);
    }
}