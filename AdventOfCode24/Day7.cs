using System.Diagnostics;

namespace AdventOfCode24;

public static class Day7
{
    private readonly record struct Operation(long Result, long[] Operands);

    private readonly record struct Node(long Value, int Depth, string StringRepresentation);

    private static Operation[] Parse(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        Operation[] operations = new Operation[lines.Length];
        int index = 0;
        foreach (string l in lines)
        {
            string[] s = l.Split(':');
            long result = long.Parse(s[0].Trim());
            string opsStr = s[1].Trim();
            string[] opsStrSplit = opsStr.Split(' ');
            long[] ops = opsStrSplit.Select(long.Parse).ToArray();
            Operation operation = new Operation(result, ops);
            operations[index++] = operation;
        }

        return operations;
    }

    // Part one
    // We implement a breadth-first search pruning all the branches that give a result
    // higher than our target. If we wanted a depth-first search we would use a stack instead of a queue.
    private static bool Test(Operation op)
    {
        Stack<Node> nextToVisit = [];
        nextToVisit.Push(new Node(op.Operands[0], 0, $"{op.Operands[0]}"));

        while (nextToVisit.Count != 0)
        {
            Node current = nextToVisit.Pop();

            if (current.Value == op.Result && current.Depth + 1 >= op.Operands.Length) 
            {
                return true; // if we reached our target the equation is valid, but only exit if we have used EVERY value!
            }
            if (current.Value > op.Result) continue; // ignore further search of this branch because we surpassed target
            
            if (current.Depth + 1 >= op.Operands.Length)
            {
                continue; // we reached the end of the search tree
            }
            
            nextToVisit.Push(new Node(current.Value + op.Operands[current.Depth + 1], current.Depth + 1, $"{current.StringRepresentation} + {op.Operands[current.Depth + 1]}"));
            nextToVisit.Push(new Node(current.Value * op.Operands[current.Depth + 1], current.Depth + 1, $"{current.StringRepresentation} * {op.Operands[current.Depth + 1]}"));
            
            // Part two: add "concat" operator
            nextToVisit.Push(new Node(Convert.ToInt64($"{current.Value}{op.Operands[current.Depth + 1]}"), current.Depth + 1, $"{current.StringRepresentation} || {op.Operands[current.Depth + 1]}"));
        }

        return false;
    }

    public static void Solve(string filename)
    {
        Operation[] operations = Parse(filename);
        Stopwatch stopwatch = Stopwatch.StartNew();
        long result = operations
            .Where(Test)
            .Sum(op => op.Result);
        stopwatch.Stop();
        Console.WriteLine($"Sum of test values: {result}, time: {stopwatch.ElapsedMilliseconds} ms");
    }
}