using static System.StringSplitOptions;

namespace AdventOfCode2024.Day01.Part2App;

internal static class Program
{
    private static void Main()
    {
        var inputLines = File.ReadAllLines("input.txt");

        var left = new List<int>();
        var right = new Dictionary<int, int>();

        foreach (var parts in inputLines.Select(line => line.Split(' ', TrimEntries | RemoveEmptyEntries)))
        {
            left.Add(int.Parse(parts[0]));

            var r = int.Parse(parts[1]);
            right[r] = right.GetValueOrDefault(r, 0) + 1;
        }

        var totalSimilarityScore = left.Sum(x => x * right.GetValueOrDefault(x, 0));

        Console.WriteLine(totalSimilarityScore);
    }
}
