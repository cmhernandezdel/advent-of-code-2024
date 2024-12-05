namespace AdventOfCode24;

public sealed class Day5
{
    private readonly record struct OrderInfo(List<int> before, List<int> after);
    private Dictionary<int, OrderInfo> orders = new();
    private List<List<int>> pages = [];

    private void ParseOrders(string filename)
    {
        var lines = File.ReadAllLines(filename);
        foreach (var l in lines)
        {
            var split = l.Split('|');
            var b = int.Parse(split[0]);
            var a = int.Parse(split[1]);
            
            if (!orders.ContainsKey(a)) orders[a] = new OrderInfo([], [b]);
            else orders[a].after.Add(b);
            if (!orders.ContainsKey(b)) orders[b] = new OrderInfo([a], []);
            else orders[b].before.Add(a);
        }
    }

    private void ParsePages(string filename)
    {
        var lines = File.ReadAllLines(filename);
        foreach (var l in lines)
        {
            var n = l.Split(',');
            var ints = n.Select(x => int.Parse(x.Trim())).ToList();
            pages.Add(ints);
        }
    }

    // Part one
    // We really only have to start from the end and look if any number should come after the current one
    // If any of the numbers that are BEFORE our current one should be AFTER it, then the list is not ordered
    private List<List<int>> GetOrderedPages()
    {
        return pages.Where(IsOrdered).ToList();
    }

    private int GetMiddleValue(List<int> p)
    {
        return p[p.Count / 2];
    }

    private bool IsOrdered(List<int> l)
    {
        for (int i = l.Count - 1; i >= 0; i--)
        {
            var currentPage = l[i];
            var beforeCurrent = l.Slice(0, i);
            foreach (var b in beforeCurrent)
            {
                if (orders[currentPage].before.Contains(b)) return false;
            }
        }

        return true;
    }

    // Part two
    // Applying the same logic, implement a custom sorting function based on the rules
    // Then apply part 1 once again to the sorted arrays
    private List<List<int>> GetUnorderedPages()
    {
        return pages.Where(p => !IsOrdered(p)).ToList();
    }
    
    private List<int> Sort(List<int> input)
    {
        var cp = input.ToList();
        cp.Sort(Compare);
        return cp;
    }

    private int Compare(int a, int b)
    {
        if (orders[a].before.Contains(b)) return -1;
        if (orders[a].after.Contains(b)) return 1;
        else return 0;
    }

    public void Solve(string fnOrders, string fnPages)
    {
        ParseOrders(fnOrders);
        ParsePages(fnPages);
        var ordered = GetOrderedPages();
        var sum = ordered.Sum(GetMiddleValue);
        Console.WriteLine($"Sum of middle values: {sum}");
        
        var unordered = GetUnorderedPages();
        var sumAfterOrder = 0;
        foreach (var u in unordered)
        {
            var sorted = Sort(u);
            var middleValue = GetMiddleValue(sorted);
            sumAfterOrder += middleValue;
        }
        Console.WriteLine($"Sum of middle values of unordered lists after order: {sumAfterOrder}");
        
    }
}