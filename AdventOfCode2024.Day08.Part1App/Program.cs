namespace AdventOfCode2024.Day08.Part1App;

internal static class Program
{
    private const char Empty = '.';

    private readonly record struct Position(int X, int Y);

    private static readonly Dictionary<char, List<Position>> Antennas = new();

    private static readonly HashSet<Position> Antinodes = new();

    private static void Main()
    {
        // Do not care about the performance here
        var lines = File.ReadAllLines("input.txt");

        var lengthX = lines.First().Length;
        var lengthY = lines.Length;

        for (var y = 0; y < lengthY; y++)
        {
            ReadOnlySpan<char> line = lines[y];

            for (int x = 0, indexFromX; x < lengthX; x += indexFromX + 1)
            {
                if ((indexFromX = line[x..].IndexOfAnyExcept(Empty)) == -1) // IDEA: SearchValues and/or char.IsAsciiLetterOrDigit()
                {
                    break;
                }

                // Current antenna position (adjusted index)
                var pos = new Position(x + indexFromX, y);

                var frequency = line[pos.X];

                if (!Antennas.TryGetValue(frequency, out var antennas))
                {
                    antennas = [];
                    Antennas[frequency] = antennas;
                }

                // Get the antennas preceding the current o
                foreach (var antenna in antennas.Where(kvp => (kvp.Y < pos.Y) || (kvp.Y == pos.Y && kvp.X < pos.X)))
                {
                    var deltaX = pos.X - antenna.X;
                    var deltaY = pos.Y - antenna.Y;

                    AddAntinode(antenna.X - deltaX, antenna.Y - deltaY);
                    AddAntinode(pos.X + deltaX, pos.Y + deltaY);

                    void AddAntinode(int antinodeX, int antinodeY)
                    {
                        if (antinodeX >= 0 && antinodeX < lengthX && antinodeY >= 0 && antinodeY < lengthY)
                        {
                            Antinodes.Add(new Position(antinodeX, antinodeY));
                        }
                    }
                }

                antennas.Add(pos);
            }
        }

        Console.WriteLine(Antinodes.Count); // 348
    }
}
