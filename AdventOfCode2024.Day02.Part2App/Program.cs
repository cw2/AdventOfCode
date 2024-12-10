using static System.StringSplitOptions;

namespace AdventOfCode2024.Day02.Part2App;

internal static class Program
{
    private static void Main()
    {
        var inputLines = File.ReadAllLines("input.txt");

        var safeCount = 0;

        foreach (var parts in inputLines.Select(line => line.Split(' ', TrimEntries | RemoveEmptyEntries)))
        {
            var report = parts.Select(int.Parse).ToArray();

            if (IsSafe(report))
            {
                safeCount++;
                continue;
            }

            // Now, the same rules apply as before, except if removing a single
            // level from an unsafe report would make it safe, the report instead counts as safe.

            for (var i = 0; i < report.Length; i++)
            {
                var reportWithoutOneLevel = report.Index().Where(x => x.Index != i).Select(x => x.Item).ToList();

                if (IsSafe(reportWithoutOneLevel))
                {
                    safeCount++;
                    break;
                }
            }
        }

        Console.WriteLine(safeCount);
    }

    // -- //

    private static bool IsSafe(IReadOnlyList<int> report)
    {
        var diffs = report.Index().Skip(1).Select(x =>
        {
            var diff = x.Item - report[x.Index - 1];
            return Math.Abs(diff) is >= 1 and <= 3 ? Math.Sign(diff) : 0;
        }).ToList();

        //var isSafe = diffs.All(x => x == 1) || diffs.All(x => x == -1);
        var isSafe = Math.Abs(diffs.Sum()) == diffs.Count;

        return isSafe;
    }
}