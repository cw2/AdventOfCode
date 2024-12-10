namespace AdventOfCode2024.Day05.Part2App;

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
            var numbers = update.Split(',').ToArray();

            // For each of the incorrectly-ordered updates, use the page ordering rules
            // to put the page numbers in the right order
            if (IsWrongOrder_Swap(numbers, rules: rules))
            {
                while (IsWrongOrder_Swap(numbers, rules: rules))
                {
                    // Do nothing
                }

                System.Diagnostics.Debug.Assert(int.IsOddInteger(numbers.Length));
                sumOfMiddlePageNumbers += int.Parse(numbers[numbers.Length / 2]);
            }
        }

        Console.WriteLine(sumOfMiddlePageNumbers); // 6897
    }

    // Whilst it is not really ideal to have the swapping included here,
    // it is much simpler than fiddling with return tuple or out parameters
    private static bool IsWrongOrder_Swap(string[] numbers, HashSet<string> rules)
    {
        for (var i = 0; i < numbers.Length - 1; i++)
        {
            for (var j = i + 1; j < numbers.Length; j++)
            {
                if (!rules.Contains(numbers[i] + "|" + numbers[j]))
                {
                    (numbers[i], numbers[j]) = (numbers[j], numbers[i]); // Swap

                    return true;
                }
            }
        }

        return false;
    }
}
