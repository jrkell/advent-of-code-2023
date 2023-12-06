internal class AdventOfCode
{
    private static void Main()
    {
        const int day = 4;

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

        double total = 0;

        foreach (var line in input)
        {
            var split = line[9..].Split(" | ");
            var winningNumbers = ParseInts(split[0].Split(" ").ToList());
            var myNumbers = ParseInts(split[1].Split(" ").ToList());
            var wins = GetNumberOfWinningNumbers(winningNumbers, myNumbers);
            var cardScore = CalculateScore(wins);
            total += cardScore;
        }
        
        Console.WriteLine(total);
    }

    private static void SolvePart2(List<string> input)
    {
        Console.WriteLine("Part 2:");

        // add one turn for each card
        var cardTurns = new List<int>();
        for (var _ = 0; _ < input.Count; _++)
        {
            cardTurns.Add(1);
        }

        double totalTurns = 0;

        
        for (var gameNumber = 0;  gameNumber < input.Count; gameNumber++)
        {
            totalTurns += cardTurns[gameNumber];
            var split = input[gameNumber][9..].Split(" | ");
            var winningNumbers = ParseInts(split[0].Split(" ").ToList());
            var myNumbers = ParseInts(split[1].Split(" ").ToList());
            var wins = GetNumberOfWinningNumbers(winningNumbers, myNumbers);

            // add number of wins to the turns ahead
            for (var i = 1; i <= wins; i++)
            {
                cardTurns[gameNumber + i] += cardTurns[gameNumber];
            }
        }

        Console.WriteLine(totalTurns);
    }

    private static int GetNumberOfWinningNumbers(List<int> winningNumbers, List<int> myNumbers) =>
        myNumbers.Where(winningNumbers.Contains).ToList().Count;

    private static List<int> ParseInts(List<string> stringsList)
    {
        var result = new List<int>();

        foreach (var str in stringsList)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                result.Add(int.Parse(str.Trim()));
            }
        }

        return result;
    }

    private static double CalculateScore(int winningNumbers)
    {
        if (winningNumbers == 0)
        {
            return 0;
        }

        return Math.Pow(2, winningNumbers-1);
    }
}