using static System.StringSplitOptions;

namespace AdventOfCode2024.Day07.Part2App;

internal static class Program
{
    private readonly record struct Node(long Value, int Index = 0);

    private static void Main()
    {
        // Do not care about the performance here
        using var reader = File.OpenText("input.txt");

        long sumOfTestValues = 0;

        while (reader.ReadLine() is { Length: > 0 } line)
        {
            ReadOnlySpan<long> numbers = line.Split([':', ' '], TrimEntries | RemoveEmptyEntries).Select(long.Parse).ToArray();

            var testValue = numbers[0];
            var operands = numbers[1..];

            // Breadth-first search

            var queue = new Queue<Node>();

            queue.Enqueue(new Node(operands[0]));

            while (queue.TryDequeue(out var node))
            {
                if (node.Value == testValue)
                {
                    if (node.Index == operands.Length - 1) // Ok, you've got me (test value produced by not all operands)
                    {
                        sumOfTestValues += testValue;
                        break;
                    }
                }

                if (node.Index < operands.Length - 1)
                {
                    var index = node.Index + 1;
                    queue.Enqueue(new Node(node.Value + operands[index], Index: index));
                    queue.Enqueue(new Node(node.Value * operands[index], Index: index));

                    // Operator ||
                    queue.Enqueue(new Node(long.Parse(node.Value.ToString() + operands[index].ToString()), Index: index));
                    // TODO: node.Value.TryFormat(span)
                }
            }
        }

        Console.WriteLine(sumOfTestValues);  // 20928985450275
    }
}
