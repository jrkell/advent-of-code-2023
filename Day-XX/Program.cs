internal class AdventOfCode
{
    private static void Main()
    {
        // Update day
        const int day = 1;

        Console.WriteLine($"Advent of Code 2023 - Day {day}");
        Console.WriteLine();

        var inputLines = File.ReadAllLines($"input.txt").ToList();

        SolvePart1(inputLines);
        SolvePart2(inputLines);

        Console.WriteLine("Press enter to exit...");
        Console.ReadLine();
    }

    private static void SolvePart1(List<string> input)
    {
        Console.WriteLine("Part 1:");

        // ...
    }

    private static void SolvePart2(List<string> input)
    {
        Console.WriteLine("Part 2:");

        // ...
    }
}