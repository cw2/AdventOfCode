using static System.StringSplitOptions;

namespace AdventOfCode2024.Day01.Part1App;

internal static class Program
{
    private static void Main()
    {
        var inputLines = File.ReadAllLines("input.txt");

        var numbers1 = new List<int>();
        var numbers2 = new List<int>();

        foreach(var parts in inputLines.Select(line => line.Split(' ', TrimEntries | RemoveEmptyEntries)))
        {
            numbers1.Add(int.Parse(parts[0]));
            numbers2.Add(int.Parse(parts[1]));
        }

        var sumOfDistances = numbers1.Order().Zip(numbers2.Order(), (n1, n2) => Math.Abs(n1 - n2)).Sum();

        Console.WriteLine(sumOfDistances);
    }
}
