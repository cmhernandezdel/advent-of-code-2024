namespace AdventOfCode24.Shared;

public class Vector2d
{
    public int I { get; }
    public int J { get; }

    public Vector2d(int i, int j)
    {
        I = i;
        J = j;
    }

    public override string ToString()
    {
        return $"({I}, {J})";
    }

    public override bool Equals(object? other)
    {
        return other is Vector2d vector && I == vector.I && J == vector.J;
    }

    public override int GetHashCode()
    {
        return I * 100000 + J;
    }

    public static Vector2d operator +(Vector2d a, Vector2d b) => new Vector2d(a.I + b.I, a.J + b.J);
    public static Vector2d operator -(Vector2d a, Vector2d b) => new Vector2d(a.I - b.I, a.J - b.J);
    public static bool operator ==(Vector2d a, Vector2d b) => a.Equals(b);
    public static bool operator !=(Vector2d a, Vector2d b) => !a.Equals(b);
    
}