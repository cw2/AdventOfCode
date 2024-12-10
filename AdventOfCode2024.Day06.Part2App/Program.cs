namespace AdventOfCode2024.Day06.Part2App;

internal static class Program
{
    private const char Obstacle = '#';

    private record Direction(char Name, int DeltaX = 0, int DeltaY = 0);

    private static readonly List<Direction> Directions =
    [
        // Order by 90 degrees clockwise
        new('^', DeltaY: -1),
        new('>', DeltaX: 1),
        new('v', DeltaY: 1),
        new('<', DeltaX: -1),
    ];

    // Guard chars - must be in the same order as Directions
    private static readonly char[] GuardChars = Directions.Select(x => x.Name).ToArray();

    // Delegate over Func<> to have named parameters
    private delegate bool WalkPredicate(int x, int y, int directionIndex);

    // IDEA: readonly record struct (or even ref struct) Position(int X, int Y)

    private static void Main()
    {
        // Do not care about the performance here
        var lines = File.ReadAllLines("input.txt");

        var lengthX = lines.First().Length;
        var lengthY = lines.Length;

        // Find the guard
        int guardX = 0;
        int guardY;
        for (guardY = 0; guardY < lengthY; guardY++)
        {
            if ((guardX = lines[guardY].IndexOfAny(GuardChars)) != -1)
            {
                break;
            }
        }

        // Get the guard direction
        var guardDirectionIndex = Directions.FindIndex(x => x.Name == lines[guardY][guardX]);
        System.Diagnostics.Debug.Assert(guardDirectionIndex != -1);

        var numberOfObstaclesCausingLoops = 0;

        Walk(guardX, guardY, guardDirectionIndex, (currX, currY, dirIndex) =>
        {
            // Loop detection: if the guard can turn (right) and step...
            var turnDirectionIndex = (dirIndex + 1) % Directions.Count;

            if (Step(x: currX, y: currY, Directions[turnDirectionIndex]) is var (turnX, turnY))
            {
                // ... and walk to the current position, then there is a loop...
                if (Walk(x: turnX, y: turnY, turnDirectionIndex, (x, y, _) => x == currX && y == currY))
                {
                    // ... and the obstacle can be placed in front of the guard.
                    if (Step(x: currX, y: currY, Directions[dirIndex]) is var (obstacleX, obstacleY))
                    {
                        //System.Diagnostics.Debug.WriteLine($"Placing obstacle at: [{obstacleX}, {obstacleY}]");
                        numberOfObstaclesCausingLoops++;
                    }
                }
            }

            return false; // Continue walking (until the guard leaves the area)
        });

        Console.WriteLine(numberOfObstaclesCausingLoops);

        return;

        (int x, int y)? Step(int x, int y, Direction dir) // IDEA: Position.Step(Direction dir)
        {
            int newX = x + dir.DeltaX;
            int newY = y + dir.DeltaY;

            return (newX >= 0 && newX < lengthX && newY >= 0 && newY < lengthY) ? (newX, newY) : null;
        }

        bool Walk(int x, int y, int dirIndex, WalkPredicate predicate)
        {
            while (true)
            {
                var direction = Directions[dirIndex];

                if (Step(x: x, y: y, direction) is not var (newX, newY))
                {
                    //System.Diagnostics.Debug.WriteLine($"Guard leaving area at ({x}, {y})");
                    return false;
                }

                if (predicate(x: x, y: y, dirIndex))
                {
                    return true;
                }

                if (lines[newY][newX] is Obstacle)
                {
                    dirIndex = (dirIndex + 1) % Directions.Count;
                    continue;
                }

                // Otherwise take a step forward (in the selected direction)
                x = newX;
                y = newY;
            }
        }
    }
}
