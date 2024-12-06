using System.Text;

namespace AdventOfCode24;

public class Day6
{
    private class Vector(int i, int j)
    {
        public readonly int I = i;
        public readonly int J = j;

        private static Vector UnitVector(int direction)
        {
            return direction switch
            {
                DIRECTION_UP => new Vector(-1, 0),
                DIRECTION_DOWN => new Vector(1, 0),
                DIRECTION_LEFT => new Vector(0, -1),
                DIRECTION_RIGHT => new Vector(0, 1),
                _ => throw new ArgumentException("Invalid direction")
            };
        }

        public Vector ApplyDirection(int direction)
        {
            Vector dirV = Vector.UnitVector(direction);
            return dirV + this;
        }
        
        public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.I + v2.I, v1.J + v2.J);
        public static bool operator ==(Vector v1, Vector v2) => v1.I == v2.I && v1.J == v2.J;
        public static bool operator !=(Vector v1, Vector v2) => !(v1 == v2);
        
        public override bool Equals(object? obj)
        {
            return obj is Vector other && this.I == other.I && this.J == other.J;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(I, J);
        }

        public override string ToString()
        {
            return $"({I}, {J})";
        }
    }

    private class Square(int i, int j, int value)
    {
        public readonly int I = i;
        public readonly int J = j;
        public int VisitedLastDirection = DIRECTION_NONE;
        public readonly int Value = value;
    }

    private class Map
    {
        private readonly Square[] _squares;
        private readonly int _initialPositionIndex;
        private readonly int _lineLength;

        public Square this[int i]
        {
            get => _squares[i];
            private set => _squares[i] = value;
        }

        public Square this[int i, int j] => _squares[i * _lineLength + j];
        public int Length => _squares.Length;
        
        public Map(Square[] squares, int lineLength, int initialPositionIndex)
        {
            _squares = squares;
            _initialPositionIndex = initialPositionIndex;
            _lineLength = lineLength;

            _squares[_initialPositionIndex].VisitedLastDirection = DIRECTION_UP;
        }

        public Vector GetInitialPosition() => new Vector(_initialPositionIndex / _lineLength, _initialPositionIndex % _lineLength);
        public int GetInitialPositionIndex() => _initialPositionIndex;
        public bool IsVectorOutOfBounds(Vector v) => v.I >= _squares.Length / _lineLength || v.I < 0 || v.J >= _lineLength || v.J < 0;
        public int GetNumberOfVisitedSquares() => _squares.Count(s => s.VisitedLastDirection != DIRECTION_NONE);
        public void Visit(Vector coords, int direction) => this[coords.I, coords.J].VisitedLastDirection = direction;
        public void BlockNode(int index) => this[index] = new Square(this[index].I, this[index].J, BLOCKED_SQUARE);

        public Map Copy()
        {
            Square[] squares = new Square[_squares.Length];
            for (int i = 0; i < _squares.Length; i++) squares[i] = new Square(_squares[i].I, _squares[i].J, _squares[i].Value);
            return new Map(squares, _lineLength, _initialPositionIndex);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _squares.Length / _lineLength; i++)
            {
                for (int j = 0; j < _lineLength; j++)
                {
                    char c = '.';
                    switch (this[i, j].Value)
                    {
                        case EMPTY_SQUARE:
                            if (this[i, j].VisitedLastDirection != DIRECTION_NONE) c = 'X';
                            break;
                        case BLOCKED_SQUARE:
                            c = '#';
                            break;
                    }

                    sb.Append($"{c} ");
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    private const int DIRECTION_NONE = -1;
    private const int DIRECTION_UP = 0;
    private const int DIRECTION_RIGHT = 1;
    private const int DIRECTION_DOWN = 2;
    private const int DIRECTION_LEFT = 3;
    private const int NUMBER_OF_DIRECTIONS = 4;

    private const int EMPTY_SQUARE = 9;
    private const int BLOCKED_SQUARE = 8;
    
    private static Map Parse(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        int initialPositionIndex = 0;
        int lineLength = lines[0].Length;
        Square[] map = new Square[lines.Length * lineLength];
        
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                switch (lines[i][j])
                {
                    case '.':
                        map[i * lineLength + j] = new Square(i, j, EMPTY_SQUARE);
                        break;
                    case '#':
                        map[i * lineLength+ j] = new Square(i, j, BLOCKED_SQUARE);
                        break;
                    case '^':
                        map[i * lineLength + j] = new Square(i, j, EMPTY_SQUARE);
                        initialPositionIndex = i * lineLength + j;
                        break;
                    default:
                        throw new ArgumentException("Invalid input");
                }
            }
        }
        
        return new Map(map, lineLength, initialPositionIndex);
    }
    
    // Part one
    // Starting at an initial position and an initial direction, compute the next step, if there's an obstacle
    // turn 90 degrees right, otherwise go there, then repeat this until you're out of bounds.
    // We need to assume that there won't be a cycle for this to work.
    private static int CountNumberOfSteps(Map map)
    {
        Vector initialPosition = map.GetInitialPosition();
        int currentDirection = DIRECTION_UP; // it starts facing up
        Vector currentPosition = new Vector(initialPosition.I, initialPosition.J);
        Vector nextPosition = currentPosition.ApplyDirection(currentDirection);
        do
        {
            if (map[nextPosition.I, nextPosition.J].Value == BLOCKED_SQUARE)
            {
                currentDirection = (currentDirection + 1) % NUMBER_OF_DIRECTIONS;
            }
            else if (map[nextPosition.I, nextPosition.J].Value == EMPTY_SQUARE)
            {
                currentPosition = nextPosition;
                map.Visit(currentPosition, currentDirection);
            }
            
            nextPosition = currentPosition.ApplyDirection(currentDirection);
        } while (!map.IsVectorOutOfBounds(nextPosition));
        
        return map.GetNumberOfVisitedSquares();
    }

    // Part two
    // There is a cycle if we get to the same position and direction as the initial ones
    // There is not a cycle if we get out of bounds with our algorithm.
    // For each square, check if putting down an obstacle there makes a loop, then count the number of positions
    // that end up in a loop
    private static bool Loops(Map map)
    {
        Vector initialPosition = map.GetInitialPosition();
        int currentDirection = DIRECTION_UP; // it starts facing up
        Vector currentPosition = new Vector(initialPosition.I, initialPosition.J);
        Vector nextPosition = currentPosition.ApplyDirection(currentDirection);
        
        do
        {
            if (map[nextPosition.I, nextPosition.J].Value == BLOCKED_SQUARE)
            {
                currentDirection = (currentDirection + 1) % NUMBER_OF_DIRECTIONS;
            }
            else if (map[nextPosition.I, nextPosition.J].Value == EMPTY_SQUARE)
            {
                // break condition
                if (map[nextPosition.I, nextPosition.J].VisitedLastDirection == currentDirection)
                {
                    return true; 
                }
                currentPosition = nextPosition;
                map.Visit(currentPosition, currentDirection);
            }
            
            nextPosition = currentPosition.ApplyDirection(currentDirection);
        } while (!map.IsVectorOutOfBounds(nextPosition));

        // if we got here, then there is not a cycle
        return false;
    }
    
    private static int FindNumberOfPossibleLoops(Map initialMap)
    {
        int numberOfLoops = 0;
        int initialPositionIndex = initialMap.GetInitialPositionIndex();
        
        for (int i = 0; i < initialMap.Length; i++)
        {
            if (initialMap[i].Value == BLOCKED_SQUARE) continue; // skip this because it wouldn't be a new obstacle
            if (i == initialPositionIndex) continue; // skip this because we cannot place the object where the guard is
            
            Map map = initialMap.Copy();
            map.BlockNode(i);
            if (Loops(map))
            {
                numberOfLoops++;
            }
        }

        return numberOfLoops;
    }
    
    
    public static void Solve(string filename)
    {
        Map map = Parse(filename);
        int numberOfSteps = CountNumberOfSteps(map);
        Console.WriteLine($"Number of steps: {numberOfSteps}");
        int numberOfLoops = FindNumberOfPossibleLoops(map);
        Console.WriteLine($"Number of loops: {numberOfLoops}");
    }
}