using System.IO.MemoryMappedFiles;
using System.Text;

namespace AdventOfCode2024.Day03.Part2App;

internal static class Program
{
    private static void Main()
    {
        using var mmf = MemoryMappedFile.CreateFromFile("input.txt");

        using var stream = mmf.CreateViewStream();

        // Note: BinaryReader used mainly for convenience (do not really care about raw performance here)
        using var reader = new BinaryReader(stream, Encoding.ASCII);

        // "mul(X,Y)" where X and Y are each 1-3 digit
        const int BufferLength = (3 - 1) + 1 + 3 + 1 + 3 + 1; // Note: -1 for 'm' being read separately
        const int MinMulLength = (3 - 1) + 1 + 1 + 1 + 1 + 1;

        Span<char> buffer = stackalloc char[BufferLength];

        long sumOfProducts = 0;

        // There are two new instructions you'll need to handle:
        // o) The do() instruction enables future mul instructions.
        // o) The don't() instruction disables future mul instructions.
        //
        // Only the most recent do() or don't() instruction applies.
        // At the beginning of the program, mul instructions are enabled.

        var mulEnabled = true;

        for (int ch; (ch = reader.Read()) != -1;)
        {
            var chIsD = ch is 'd';

            // There are many potential optimizations here, but it would require
            // proper benchmarking to see if they are worth it (e.g. use of big
            // reading buffers, better search etc.). For now, we keep it simple
            // and assume the memory-mapped file + stream is efficient enough.
            if (ch is not 'm' && !chIsD)
            {
                continue;
            }

            // Save the current position in case we need to backtrack to the next character
            var position = reader.BaseStream.Position;

            var readLength = reader.Read(buffer);

            if (chIsD)
            {
                if (ParseDoOrDont(readLength, buffer) is var (enabled, offset))
                {
                    mulEnabled = enabled;
                    reader.BaseStream.Position = position + offset;
                }

                continue;
            }

            if (readLength >= MinMulLength && buffer.StartsWith("ul(")) // 'm' already read
            {
                if (ParseThreeDigitNumberAndPostfix(buffer[3..], ',') is var (number1, length1) &&
                    ParseThreeDigitNumberAndPostfix(buffer[(4 + length1)..], ')') is var (number2, length2))
                {
                    if (mulEnabled)
                    {
                        sumOfProducts += number1 * number2;
                    }

                    // Advance to the next character after the closing parenthesis
                    position += 3 + length1 + 1 + length2 + 1;
                }
            }

            reader.BaseStream.Position = position;
        }

        Console.WriteLine(sumOfProducts); // 161289189
    }

    // -- //

    private static (int number, int length)? ParseThreeDigitNumberAndPostfix(ReadOnlySpan<char> input, char postfix)
    {
        var number = 0;
        var length = 0;

        while (char.IsAsciiDigit(input[length]) && length < 3)
        {
            number = number * 10 + input[length++] - '0';
        }

        return input[length] == postfix ? (number, length) : null;
    }

    private static (bool enabled, int offset)? ParseDoOrDont(int readLength, ReadOnlySpan<char> input) => readLength switch
    {
        >= 6 when input.StartsWith("on't()") => (false, 6),
        >= 3 when input.StartsWith("o()") => (true, 3),
        _ => null,
    };
}
