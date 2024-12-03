namespace AdventOfCode24;

public static class Day2
{
    // parse input into a list of lists of integers
    private static List<List<int>> Parse(string filename)
    {
        var retVal = new List<List<int>>();
        var lines = File.ReadAllLines(filename);
        foreach (var line in lines)
        {
            var pStr = line.Split(' ');
            var v = pStr.Select(s => Convert.ToInt32(s.Trim())).ToArray();
            retVal.Add(v.ToList());
        }

        return retVal;
    }

    // Part one
    private static int CalculateNumberOfSafeReports(List<List<int>> reports)
    {
        var retVal = 0;
        foreach (var r in reports)
        {
            if (IsValid(r)) retVal++;
        }

        return retVal;
    }

    private static bool IsValid(List<int> r)
    {
        // potentially speed up things if it doesn't meet basic criteria
        // given the size of the input this isn't really needed, but it was a cool thing to do
        if (!CanBeValid(r)) return false;
        var isIncreasing = r[1] > r[0];
        var isValid = true;
        var last = r[0];
        for (var i = 1; i < r.Count; i++)
        {
            // if this is an increasing list, the moment we encounter a decreasing trend we break
            // we also break if the difference between consecutive numbers is greater than 3
            if (isIncreasing)
            {
                if (r[i] - last > 3 || r[i] - last <= 0)
                {
                    isValid = false;
                    break;
                }
            }
            
            // same thing but in reverse
            else
            {
                if (last - r[i] > 3 || last - r[i] <= 0)
                {
                    isValid = false;
                    break;
                }
            }
            last = r[i];
        }

        return isValid;
    }
    
    // A report can only be valid if it's decreasing or increasing constantly
    // and always by a number greater than 0 and lower than 3
    private static bool CanBeValid(List<int> r)
    {
        int minIncreasingLastValue = r.First() + r.Count - 1;
        int maxIncreasingLastValue = r.First() + (r.Count - 1) * 3;
        int minDecreasingLastValue = r.First() - ((r.Count - 1) * 3);
        int maxDecreasingLastValue = r.First() - (r.Count - 1);
        bool isIncreasing = r[0] < r[1];
        
        return (isIncreasing && r.Last() >= minIncreasingLastValue && r.Last() <= maxIncreasingLastValue) ||
               (!isIncreasing && r.Last() >= minDecreasingLastValue && r.Last() <= maxDecreasingLastValue);
    }
    
    // Part two
    // Not going to lie, I don't like the brute force approach of taking an element out of every list
    // and then applying the part 1 solution to the sublist, because if the input was larger
    // this would've taken a while. However, I couldn't find a better solution in the given time.
    // I took a look at the input and, since it was small, I went with this.
    private static int CalculateNumberOfSafeReportsWithDampener(List<List<int>> reports)
    {
        var retVal = 0;
        foreach (var r in reports)
        {
            bool isAnyIterationValid = false;
            for (int i = 0; i < r.Count; ++i)
            {
                var cp = r.ToList();
                cp.RemoveAt(i);
                bool isValid = IsValid(cp);
                if (isValid)
                {
                    isAnyIterationValid = true;
                    break;
                }
            }

            if (isAnyIterationValid) retVal++;
        }

        return retVal;
    }
    
    public static void Solve(string filename)
    {
        var parsedInput = Parse(filename);
        var safeReports = CalculateNumberOfSafeReports(parsedInput);
        Console.WriteLine($"Safe reports: {safeReports}");
        var safeReportsWithDampener = CalculateNumberOfSafeReportsWithDampener(parsedInput);
        Console.WriteLine($"Safe reports with dampener: {safeReportsWithDampener}");
    }
    
}