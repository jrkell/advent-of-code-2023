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

    }

    private static void SolvePart1(List<string> input)
    {
        Console.WriteLine("Part 1:");

        var rules = new ColourCount(12, 14, 13);

        var total = 0;

        foreach (var line in input)
        {
            var game = new Game(line);
            if (game.IsPossible(rules))
            {
                total += game.Number;
            }
        }

        Console.WriteLine(total);
    }

    private static void SolvePart2(List<string> input)
    {
        Console.WriteLine("Part 2:");

        var total = 0;
        foreach (var line in input)
        {
            var game = new Game(line);
            var minRequired = game.MinimumRequired();
            var power = minRequired.RedCount * minRequired.GreenCount * minRequired.BlueCount;
            total += power;
        }

        Console.WriteLine(total);
    }
}

public class Game
{
    public int Number { get; set; }
    private List<Set> SetList { get; set; } = new ();

    public Game(string gameString)
    {
        var split = gameString.Split(": ");
        var setStringList = split[1].Split("; ");
        foreach (var setString in setStringList)
        {
            SetList.Add(new Set(setString));
        }
        Number = int.Parse(split[0][5..]);
    }

    public bool IsPossible(ColourCount rules)
    {
        var tooManyRed = SetList.Any(x => x.RedCount > rules.RedCount);
        var tooManyGreen = SetList.Any(x => x.GreenCount > rules.GreenCount);
        var tooManyBlue = SetList.Any(x => x.BlueCount > rules.BlueCount);

        return !tooManyRed && !tooManyGreen && !tooManyBlue;
    }

    public ColourCount MinimumRequired()
    {
        var maximumRedsInSets = SetList.Max(x => x.RedCount);
        var maximumGreensInSets = SetList.Max(x => x.GreenCount);
        var maximumBluesInSets = SetList.Max(x => x.BlueCount);

        return new ColourCount(maximumRedsInSets, maximumBluesInSets, maximumGreensInSets);
    }
}

public class Set
{
    public int RedCount { get; set; }
    public int BlueCount { get; set; }
    public int GreenCount { get; set; }
    
    public Set(string setString)
    {
        RedCount = 0;
        BlueCount = 0;
        GreenCount = 0;

        foreach (var colourAndCount in setString.Split(", "))
        {
            var splitSet = colourAndCount.Split(" ");
            var colour = splitSet[1];
            var count = int.Parse(splitSet[0]);

            switch (colour)
            {
                case "red":
                    RedCount += count;
                    break;
                case "blue":
                    BlueCount += count;
                    break;
                case "green":
                    GreenCount += count;
                    break;
            }
        }
    }
}

public class ColourCount
{
    public int RedCount;
    public int BlueCount;
    public int GreenCount;

    public ColourCount(int redCount, int blueCount, int greenCount)
    {
        RedCount = redCount;
        BlueCount = blueCount;
        GreenCount = greenCount;
    }
}