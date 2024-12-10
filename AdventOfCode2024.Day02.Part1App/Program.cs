using static System.StringSplitOptions;

namespace AdventOfCode2024.Day02.Part1App;

internal static class Program
{
    private static void Main()
    {
        var inputLines = File.ReadAllLines("input.txt");

        var safeCount = 0;

        foreach (var parts in inputLines.Select(line => line.Split(' ', TrimEntries | RemoveEmptyEntries)))
        {
            var numbers = parts.Select(int.Parse).ToArray();

            var diffs = numbers.Index().Skip(1).Select(x =>
            {
                var diff = x.Item - numbers[x.Index - 1];
                return Math.Abs(diff) is >= 1 and <= 3 ? Math.Sign(diff) : 0;
            }).ToList();

            //var isSafe = diffs.All(x => x == 1) || diffs.All(x => x == -1);
            var isSafe = Math.Abs(diffs.Sum()) == diffs.Count;

            if (isSafe)
            {
                safeCount++;
            }
        }

        Console.WriteLine(safeCount); // 314
    }
}
