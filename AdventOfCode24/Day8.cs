using System.Text.RegularExpressions;
using AdventOfCode24.Shared;

namespace AdventOfCode24;

public class Day8
{
    private readonly Dictionary<char, List<Vector2d>> _antennas = [];
    private readonly HashSet<Vector2d> _antinodes = [];
    private int _gridHeight, _gridWidth;
    
    private void Parse(string filename)
    {
        Regex regex = new Regex("^[a-zA-Z0-9_]*$");
        string[] lines = File.ReadAllLines(filename);
        _gridHeight = lines.Length;
        _gridWidth = lines[0].Length;
        
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                char c = line[j];
                if (regex.IsMatch(c.ToString()))
                {
                    //Console.WriteLine($"Match! ({i}, {j})");
                    if (_antennas.ContainsKey(c)) _antennas[c].Add(new Vector2d(i,j));
                    else _antennas.Add(c, [new Vector2d(i, j)]);
                }
            }
        }
    }

    // Part one
    // Calculate distance between the antennas and apply that distance (within the same direction, outwards)
    // to the antennas involved, checking that the points are within the limits of the grid.
    private void GenerateAntinodes()
    {
        foreach (var antenna in _antennas)
        {
            for (int a = 0; a < antenna.Value.Count; a++)
            {
                for (int b = 0; b < antenna.Value.Count; b++)
                {
                    var antenna1 = antenna.Value[a];
                    var antenna2 = antenna.Value[b];
                    if (antenna1 == antenna2) continue;
                    var distance = antenna1 - antenna2;
                    var point = antenna1 + distance;
                    
                    if(point.I < 0 || point.I >= _gridHeight || point.J < 0 || point.J >= _gridWidth) continue;
                    
                    _antinodes.Add(point);
                }
            }
        }
    }
    
    // Part two
    // Do the same but keep going in the straight line until grid bounds, applying the same distance in each step.
    // Also every antenna counts as an antinode now.
    private void GenerateAntinodesWithResonance()
    {
        foreach (var antenna in _antennas)
        {
            for (int a = 0; a < antenna.Value.Count; a++)
            {
                for (int b = 0; b < antenna.Value.Count; b++)
                {
                    var antenna1 = antenna.Value[a];
                    var antenna2 = antenna.Value[b];
                    if (antenna1 == antenna2) continue;
                    
                    _antinodes.Add(antenna1); // every antenna counts
                    var distance = antenna1 - antenna2;
                    var point = antenna1 + distance;
                    while (point.I >= 0 && point.I < _gridHeight && point.J >= 0 && point.J < _gridWidth)
                    {
                        _antinodes.Add(point);
                        point += distance;
                    }
                }
            }
        }
    }
    
    public void Solve(string filename)
    {
        Parse(filename);
        GenerateAntinodes();
        Console.WriteLine($"Antinodes: {_antinodes.Count}");
        _antinodes.Clear();
        GenerateAntinodesWithResonance();
        Console.WriteLine($"Antinodes with resonance: {_antinodes.Count}");
    }
}