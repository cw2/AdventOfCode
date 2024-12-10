namespace AdventOfCode2024.Day04.Part1App;

// MMMSXXMASM | ....XXMAS.
// MSAMXMSMSA | .SAMXMS...
// AMXSXMAAMM | ...S..A...
// MSAMASMSMX | ..A.A.MS.X
// XMASAMXAMM | XMASAMX.MM
// XXAMMXXAMA | X.....XA.A
// SMSMSASXSS | S.S.S.S.SS
// SAXAMASAAA | .A.A.A.A.A
// MAMMMXMMMM | ..M.M.M.MM
// MXMXAXMASX | .X.X.XMASX

internal static class Program
{
    private const string Xmas = "XMAS";

    private record Direction(string Name, int MinX = 0, int MaxX = 0, int MinY = 0, int MaxY = 0, int DeltaX = 0, int DeltaY = 0);

    private static readonly Direction[] Directions =
    [
        new("⬆️", MinY: 3, DeltaY: -1),
        new("⬇️", MaxY: 3, DeltaY: 1),
        new("⬅️", MinX: 3, DeltaX: -1),
        new("➡️", MaxX: 3, DeltaX: 1),
        new("↗️", MaxX: 3, MinY: 3, DeltaX: 1, DeltaY: -1),
        new("↖️", MinX: 3, MinY: 3, DeltaX: -1, DeltaY: -1),
        new("↘️", MaxX: 3, MaxY: 3, DeltaX: 1, DeltaY: 1),
        new("↙️", MinX: 3, MaxY: 3, DeltaX: -1, DeltaY: 1),
    ];

    private static void Main()
    {
        // Do not care about the performance here
        var lines = File.ReadAllLines("input.txt");

        var lengthX = lines.First().Length;
        var lengthY = lines.Length;

        var xmasOccurrences = 0;

        // Totally naive approach - search in all directions at once...

        for (var y = 0; y < lengthY; y++)
        {
            // Exclude Y directions that are not possible (could have been precalculated...)
            var possibleDirections = Directions.Where(d => y >= d.MinY && y < lengthY - d.MaxY).ToList();

            for (var x = 0; x < lengthX; x++)
            {
                foreach (var dir in possibleDirections.Where(d => x >= d.MinX && x < lengthX - d.MaxX))
                {
                    var found = true;

                    for (int i = 0, x1 = x, y1 = y; i < Xmas.Length; i++, x1 += dir.DeltaX, y1 += dir.DeltaY)
                    {
                        if (lines[y1][x1] != Xmas[i])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        //System.Diagnostics.Debug.WriteLine($"[{x}, {y}] {dir.Name}");
                        xmasOccurrences++;
                    }
                }
            }
        }

        Console.WriteLine(xmasOccurrences); // 2534
    }
}
