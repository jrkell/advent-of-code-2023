internal class AdventOfCode
{
    private static void Main()
    {
        // Update day
        const int day = 9;

        Console.WriteLine($"Advent of Code 2023 - Day {day}");
        Console.WriteLine();

        var inputLines = File.ReadAllLines($"input.txt").ToList();

        var expandedPatterns = ExpandPatterns(inputLines);

        SolvePart1(expandedPatterns);
        SolvePart2(expandedPatterns);

        Console.WriteLine("Press enter to exit...");
        Console.ReadLine();
    }

    private static void SolvePart1(List<List<List<int>>> expandedPatterns)
    {
        Console.WriteLine("Part 1:");

        var answers = new List<int>();
        foreach (var expandedPattern in expandedPatterns)
        {
            var addNum = 0;
            for (var i = expandedPattern.Count - 2; i >= 0; i--)
            {
                var current = expandedPattern[i];
                addNum += current[current.Count-1];
            }
            answers.Add(addNum);
        }

        Console.WriteLine(answers.Sum());
    }

    private static void SolvePart2(List<List<List<int>>> expandedPatterns)
    {
        Console.WriteLine("Part 2:");

        // solve it backwards
        var answers = new List<int>();
        foreach (var expandedPattern in expandedPatterns)
        {
            var addNum = 0;
            for (var i = expandedPattern.Count - 2; i >= 0; i--)
            {
                var current = expandedPattern[i];
                addNum = current[0] - addNum;
            }
            answers.Add(addNum);
        }

        Console.WriteLine(answers.Sum());
    }

    private static List<List<List<int>>> ExpandPatterns(List<string> inputLines)
    {
        var patterns = inputLines.Select(line => line.Split(' ').Select(int.Parse).ToList()).ToList();
        var expandedPatterns = new List<List<List<int>>>();

        // expand each pattern to get the full sequence
        foreach (var pattern in patterns)
        {
            var currentSequence = pattern;
            var expandedPattern = new List<List<int>>
            {
                pattern
            };
            var finished = false;

            // keep getting the differences until they are all 0s
            while (!finished)
            {
                var differences = new List<int>();

                for (var i = 0; i < currentSequence.Count - 1; i++)
                {
                    differences.Add(currentSequence[i + 1] - currentSequence[i]);
                }

                expandedPattern.Add(differences);

                if (differences.All(x => x == 0))
                {
                    expandedPatterns.Add(expandedPattern);
                    finished = true;
                }
                else
                {
                    currentSequence = differences;
                }
            }
        }

        return expandedPatterns;
    }
}