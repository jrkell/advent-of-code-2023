internal class AdventOfCode
{
    private static void Main()
    {
        // Update day
        const int day = 6;

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

        // extract tuple of each race
        var times = input[0][11..]
            .Split(' ')
            .Where(x => x != "")
            .Select(x => int.Parse(x.Trim()))
            .ToList();
        var records = input[1][11..]
            .Split(' ')
            .Where(x => x != "")
            .Select(x => int.Parse(x.Trim()))
            .ToList();
        var races = times.Select((t, i) => (t, records[i])).ToList();

        // run each race and get number of winning button presses
        var waysToBeatRecords = new List<int>();
        foreach (var (time, record) in races)
        {
            var distances = CalculateDistances(time);
            var winningDistancesCount = distances.Count(x => x.distance > record);
            waysToBeatRecords.Add(winningDistancesCount);
        }

        // get the product
        var total = waysToBeatRecords.Aggregate(1, (current, way) => current * way);

        Console.WriteLine(total);
    }

    private static void SolvePart2(List<string> input)
    {
        Console.WriteLine("Part 2:");

        // extract the time and distance for the one race
        var timeString = new string(input[0][11..]
            .Where(x => !char.IsWhiteSpace(x)).ToArray());
        var recordString = new string(input[1][11..]
            .Where(x => !char.IsWhiteSpace(x)).ToArray());

        // calculate all the button lengths and get winners
        var distances = CalculateDistances(long.Parse(timeString));
        var total = distances.Count(x => x.distance > long.Parse(recordString));

        Console.WriteLine(total);


    }

    private static List<(long button, long distance)> CalculateDistances(long time)
    {
        // distance = (time - button) * button
        var distances = new List<(long, long)>();
        for (long button = 1; button < time; button++)
        {
            var distance = (time - button) * button;
            distances.Add((button, distance));
        }

        return distances;
    }
}