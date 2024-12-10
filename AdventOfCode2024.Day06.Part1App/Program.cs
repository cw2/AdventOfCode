namespace AdventOfCode2024.Day06.Part1App;

internal static class Program
{
    private const char Obstacle = '#';

    private record Direction(char Guard, int DeltaX = 0, int DeltaY = 0);

    private static readonly List<Direction> Directions =
    [
        // Note: Order by 90 degrees clockwise
        new('^', DeltaY: -1),
        new('>', DeltaX: 1),
        new('v', DeltaY: 1),
        new('<', DeltaX: -1),
    ];

    private static void Main()
    {
        // Do not care about the performance here
        var lines = File.ReadAllLines("input.txt");

        var lengthX = lines.First().Length;
        var lengthY = lines.Length;

        var guardChars = Directions.Select(x => x.Guard).ToArray();

        // Find the guard (resp. facing direction)
        int directionIndex = -1;
        int guardX = 0;
        int guardY;
        for (guardY = 0; guardY < lengthY; guardY++)
        {
            if ((guardX = lines[guardY].IndexOfAny(guardChars)) != -1)
            {
                directionIndex = Directions.FindIndex(x => x.Guard == lines[guardY][guardX]);
                break;
            }
        }

        System.Diagnostics.Debug.Assert(directionIndex != -1, "Guard not found");

        // Save visited positions, so we don't need mutable 2D char array
        var visitedPositions = new Dictionary<string, int>(); // x,y -> count

        while (true)
        {
            var position = guardX + "," + guardY;
            visitedPositions[position] = visitedPositions.GetValueOrDefault(position) + 1;

            var direction = Directions[directionIndex];

            var x = guardX + direction.DeltaX;
            var y = guardY + direction.DeltaY;

            if (x < 0 || x >= lengthX || y < 0 || y >= lengthY)
            {
                System.Diagnostics.Debug.WriteLine($"Guard leaving area at ({guardX}, {guardY})");
                break;
            }

            // If there is something directly in front of the guard, turn right 90 degrees.
            if (lines[y][x] is Obstacle)
            {
                directionIndex = (directionIndex + 1) % Directions.Count;
                continue;
            }

            // Otherwise take a step forward (in the selected direction)
            guardX = x;
            guardY = y;
        }

        var numberOfUniqueVisitedPositions = visitedPositions.Count; // Keys are unique

        Console.WriteLine(numberOfUniqueVisitedPositions); // 5551
    }
}
