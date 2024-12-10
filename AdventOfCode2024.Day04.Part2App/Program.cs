namespace AdventOfCode2024.Day04.Part2App;

internal static class Program
{
    private static void Main()
    {
        // Do not care about the performance here
        var lines = File.ReadAllLines("input.txt");

        var lengthX = lines.First().Length;
        var lengthY = lines.Length;

        var xmasOccurrences = 0;

        Span<char> buffer = stackalloc char[4];

        for (var y = 1; y < lengthY - 1; y++)
        {
            for (var x = 1; x < lengthX - 1; x++)
            {
                if (lines[y][x] != 'A') // Center
                {
                    continue;
                }
                // Do not care about checking the other (corner) characters

                buffer[0] = lines[y - 1][x - 1]; // Top left
                buffer[1] = lines[y - 1][x + 1]; // Top right
                buffer[2] = lines[y + 1][x - 1]; // Bottom left
                buffer[3] = lines[y + 1][x + 1]; // Bottom right

                // M.S | M.M | S.M | S.S
                // .A. | .A. | .A. | .A.  - ignored, already checked above
                // M.S | S.S | S.M | M.M
                var found = buffer is "MSMS" or "MMSS" or "SMSM" or "SSMM";
                // IDEA: Compare as int32 via BinaryPrimitives.ReadInt32Xxx() (?)

                if (found)
                {
                    xmasOccurrences++;
                }
            }
        }

        Console.WriteLine(xmasOccurrences); // 1866
    }
}
