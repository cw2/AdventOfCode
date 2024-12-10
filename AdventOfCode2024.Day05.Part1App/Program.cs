namespace AdventOfCode2024.Day05.Part1App;

internal static class Program
{
    private static void Main()
    {
        using var reader = File.OpenText("input.txt");

        var rules = new HashSet<string>();

        while (reader.ReadLine() is { Length: > 0 } rule)
        {
            rules.Add(rule); // x|y where x is preceding and y is succeeding page number
        }
        // Empty line between rules and update page

        var sumOfMiddlePageNumbers = 0;

        while (reader.ReadLine() is { } update)
        {
            // Update is a comma-separated list of page numbers
            var numbers = update.Split(',').Select(int.Parse).ToArray();

            var rightOrder = true;

            for (var i = 0; i < numbers.Length - 1; i++)
            {
                for (var j = i + 1; j < numbers.Length; j++)
                {
                    if (!rules.Contains(numbers[i] + "|" + numbers[j]))
                    {
                        rightOrder = false;
                        break;
                    }
                }
            }

            if (rightOrder)
            {
                System.Diagnostics.Debug.Assert(int.IsOddInteger(numbers.Length));
                sumOfMiddlePageNumbers += numbers[numbers.Length / 2];
            }
        }

        Console.WriteLine(sumOfMiddlePageNumbers); // 6505
    }
}
