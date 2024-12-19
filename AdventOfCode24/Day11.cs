namespace AdventOfCode24;

public class Day11
{
    public const string ExampleInput = "125 17";
    public const string Input = "8793800 1629 65 5 960 0 138983 85629";

    // Here I thought I was super smart because I actually solved this on the 19th
    // and of course I had read the Reddit, but seems like caching the results this way
    // turns into memory issues really fast.
    
    // Then I tried to cache only the multiplications, but I got into memory issues as well.
    // Ideas: manually managed memory, parallelization
    private readonly Dictionary<string, string> _mapping = new Dictionary<string, string>
    {
        { "0", "1" },
        { "1", "2024" }
    };

    private string Blink(string input)
    {
        List<string> newInput = [];
        string[] s = input.Split(" ");
        foreach (var x in s)
        {
            bool success = _mapping.TryGetValue(x, out string? v);
            if (success)
            {
                newInput.Add(v!);
            }
            else
            {
                if (x.Length % 2 == 0)
                {
                    string[] values = new string[2];
                    values[0] = Convert.ToInt64(x.Substring(0, x.Length / 2)).ToString();
                    values[1] = Convert.ToInt64(x.Substring(x.Length / 2)).ToString();
                    string newValue = string.Join(' ', values);
                    newInput.Add(newValue);
                }
                else
                {
                    long lValue = Convert.ToInt64(x);
                    lValue *= 2024;
                    string newValue = lValue.ToString();
                    _mapping.Add(x, newValue);
                    newInput.Add(newValue);
                }
            }
        }
        return string.Join(' ', newInput);
    }
    
    public void Solve(string input, int times)
    {
        string newInput = input;
        for (int i = 0; i < times; i++)
        {
            newInput = Blink(newInput);
        }
        
        Console.WriteLine($"Number of stones: {newInput.Split(' ').Length}");
    }


}