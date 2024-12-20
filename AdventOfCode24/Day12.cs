namespace AdventOfCode24;

public class Day12
{
    private string[][] Parse(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        string[][] r = new string[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            r[i] = new string[lines[i].Length];
            for (int j = 0; j < lines[i].Length; j++)
            {
                r[i][j] = lines[i][j].ToString();
            }
        }

        return r;
    }
    
    public void Expand(string c, int startI, int startJ)
}