namespace AdventOfCode24.Shared;

public class Point2d(int i, int j)
{
    public int I { get; } = i;
    public int J { get; } = j;

    public override string ToString()
    {
        return $"({I}, {J})";
    }

    public override bool Equals(object? other)
    {
        return other is Point2d vector && I == vector.I && J == vector.J;
    }

    public override int GetHashCode()
    {
        return I * 100000 + J;
    }

    public static Point2d operator +(Point2d a, Point2d b) => new Point2d(a.I + b.I, a.J + b.J);
    public static Point2d operator -(Point2d a, Point2d b) => new Point2d(a.I - b.I, a.J - b.J);
    public static bool operator ==(Point2d a, Point2d b) => a.Equals(b);
    public static bool operator !=(Point2d a, Point2d b) => !a.Equals(b);
    
    public static Point2d operator +(Point2d p, Vector2d v) => new Point2d(p.I + v.I, p.J + v.J);
    public static Point2d operator -(Point2d p, Vector2d v) => new Point2d(p.I - v.I, p.J - v.J);
}