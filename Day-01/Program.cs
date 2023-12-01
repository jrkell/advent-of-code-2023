internal class AdventOfCode
{
    private static readonly Dictionary<string, int> NumberWords = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
    };

    private static void Main()
    {
        const int day = 1;

        Console.WriteLine($"Advent of Code 2023 - Day {day}");
        Console.WriteLine();

        var inputLines = File.ReadAllLines($"input.txt");

        SolvePart1(inputLines);
        SolvePart2(inputLines);

    }

    private static void SolvePart1(IEnumerable<string> input)
    {
        Console.WriteLine("Part 1:");

        var total = 0;

        foreach (var line in input)
        {
            var allNumbers = line.Where(c => GetInt(c) != -1).ToList();
            total += CombineFirstAndLastNumber(allNumbers);
        }

        Console.WriteLine(total);
    }

    private static void SolvePart2(IEnumerable<string> input)
    {
        Console.WriteLine("Part 2:");

        var total = 0;

        foreach (var line in input)
        {
            var allNumbers = GetAllNumbers(line);
            total += CombineFirstAndLastNumber(allNumbers);
        }

        Console.WriteLine(total);
    }

    private static List<int> GetAllNumbers(string line)
    {
        var list = new List<int>();
        for (var i = 0; i < line.Length; i++)
        {
            // try find an actual number
            var number = GetInt(line[i]);
            if (number != -1)
            {
                list.Add(number);
                continue;
            }

            // try find a number word
            number = GetInt(line[i..]);
            if (number != -1)
            {
                list.Add(number);
            }
        }

        return list;
    }

    private static int GetInt(char character)
    {
        if (int.TryParse(character.ToString(), out var number))
        {
            return number;
        }

        return -1;
    }

    private static int GetInt(string line)
    {
        foreach (var (word, number) in NumberWords)
        {
            if (line.StartsWith(word))
            {
                return number;
            }
        }

        return -1;
    }

    private static int CombineFirstAndLastNumber(List<int> list) =>
        int.Parse($"{list.First()}{list.Last()}");

    private static int CombineFirstAndLastNumber(List<char> list) =>
        int.Parse($"{list.First()}{list.Last()}");
}