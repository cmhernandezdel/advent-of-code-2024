namespace AdventOfCode24;

public static class Day4
{
    // Part one
    // Just find horizontal, vertical and diagonal matches while respecting boundaries
    private static int FindHorizontalMatches(string[] splitText)
    {
        var matches = 0;
        foreach (var t in splitText)
        {
            for (var i = 0; i < t.Length; i++)
            {
                if (t[i] != 'X') continue;
                if (i + 3 < t.Length && t[i+1] == 'M' && t[i+2] == 'A' && t[i+3] == 'S') matches++;
                if (i - 3 >= 0 && t[i-1] == 'M' && t[i-2] == 'A' && t[i-3] == 'S') matches++;
            }
        }
        
        Console.WriteLine($"Horizontal matches: {matches}");
        return matches;
    }

    private static int FindVerticalMatches(string text, int lineLength)
    {
        var matches = 0;
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] != 'X') continue;
            if ((i + lineLength * 3 < text.Length) && 
                (text[i+lineLength * 1] == 'M') && (text[i+lineLength * 2] == 'A') 
                && (text[i+lineLength*3] == 'S')) matches++;
            
            if ((i - lineLength * 3 >= 0) && 
                (text[i - lineLength * 1] == 'M') && (text[i - lineLength * 2] == 'A') 
                && (text[i - lineLength * 3] == 'S')) matches++;
        }
        
        Console.WriteLine($"Vertical matches: {matches}");
        return matches;
    }

    private static int FindDiagonalMatches(string text, int lineLength)
    {
        var matches = 0;
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] != 'X') continue;
            if ((i + lineLength * 3 + 3 < text.Length) && 
                (text[i + lineLength * 1 + 1] == 'M') && (text[i + lineLength * 2 + 2] == 'A') 
                && (text[i + lineLength * 3 + 3] == 'S') && (((i + lineLength * 3 + 3) / lineLength)) == (i / lineLength) + 3) matches++;
            if ((i - lineLength * 3 - 3 >= 0) && 
                (text[i - lineLength * 1 - 1] == 'M') && (text[i - lineLength * 2 - 2] == 'A') 
                && (text[i - lineLength * 3 - 3] == 'S') && (((i - lineLength * 3 - 3) / lineLength)) == (i / lineLength) - 3) matches++;
            if ((i + lineLength * 3 - 3 < text.Length) && 
                (text[i + lineLength * 1 - 1] == 'M') && (text[i + lineLength * 2 - 2] == 'A') 
                && (text[i + lineLength * 3 - 3] == 'S') && (((i + lineLength * 3 - 3) / lineLength)) == (i / lineLength) + 3) matches++;
            if ((i - lineLength * 3 + 3 >= 0) && 
                (text[i - lineLength * 1 + 1] == 'M') && (text[i - lineLength * 2 + 2] == 'A') 
                && (text[i - lineLength * 3 + 3] == 'S') && (((i - lineLength * 3 + 3) / lineLength)) == (i / lineLength) - 3) matches++;
        }
        
        Console.WriteLine($"Diagonal matches: {matches}");
        return matches;
    }

    // Part two
    // Find every 'A', then check if the opposite corners form 'MAS' or 'SAM'
    private static int FindCrossMatches(string text, int lineLength)
    {
        var matches = 0;
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] != 'A') continue;
            int a = i - lineLength - 1;
            int b = i - lineLength + 1;
            int c = i + lineLength - 1;
            int d = i + lineLength + 1;
            
            int aLine = a / lineLength;
            int bLine = b / lineLength;
            int cLine = c / lineLength;
            int dLine = d / lineLength;
            
            int centerLine = i / lineLength;
            
            if (a < 0 || b < 0 || c >= text.Length || d >= text.Length) continue;
            if ((aLine != bLine) || (cLine != dLine)) continue;
            if (aLine != centerLine - 1 || cLine != centerLine + 1) continue;

            if ((text[a] != 'M' && text[a] != 'S') || 
                (text[b] != 'M' && text[b] != 'S') || 
                (text[c] != 'M' && text[c] != 'S') ||
                (text[d] != 'M' && text[d] != 'S')) continue;

            if (text[a] == text[d] || text[b] == text[c]) continue;
            matches++;
        }

        return matches;
    }
    
    public static void Solve(string filename)
    {
        var text = File.ReadAllText(filename);
        var split = text.Split('\n');
        
        var lineLength = split[0].Length;
        text = text.Replace("\n", "");
        var h = FindHorizontalMatches(split);
        var v = FindVerticalMatches(text, lineLength);
        var z = FindDiagonalMatches(text, lineLength);
        Console.WriteLine($"Total matches: {h + v + z}");
        
        var cross = FindCrossMatches(text, lineLength);
        Console.WriteLine($"Cross matches: {cross}");
    }
    
}