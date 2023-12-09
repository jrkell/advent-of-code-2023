using static AdventOfCode;

internal class AdventOfCode
{
    private static void Main()
    {
        // Update day
        const int day = 5;

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

        var seeds = GetSeeds(input[0]);
        var mappings = GetMappings(input);

        var results = new List<long>();

        foreach (var seed in seeds)
        {
            var currentSeedNumber = seed;
            foreach (var mapping in mappings)
            {
                foreach (var range in mapping.Ranges)
                {
                    var match = range.GetCorrespondingNumber(currentSeedNumber);
                    if (match != -1)
                    {
                        currentSeedNumber = match;
                        break;
                    }
                }
            }

            results.Add(currentSeedNumber);
        }

        Console.WriteLine(results.Min());
    }

    private static void SolvePart2(List<string> input)
    {
        Console.WriteLine("Part 2:");

        var seeds = GetSeeds2(input[0]);
        var mappings = GetMappings(input);

        var results = new List<long>();


        var lowest = 99999999999999999;
        foreach (var seed in seeds)
        {
            var destinations = new List<(long, long)>();
            destinations.Add(seed);

            foreach (var mapping in mappings)
            {
                var sources = destinations;
                destinations = new List<(long, long)>();
                while (sources.Count > 0)
                {
                    var source = Pop(sources);
                    var (sourceStart, sourceEnd) = source;
                    foreach (var range in mapping.Ranges)
                    {
                        var adjustment = range.DestinationRangeStart - range.SourceRangeStart;
                        var rangeStart = range.SourceRangeStart;
                        var rangeEnd = range.SourceRangeStart + range.RangeLength;

                        // completely covered by mapping, move to next mapping
                        if (sourceStart >= rangeStart && sourceEnd <= rangeEnd)
                        {
                            destinations.Add((sourceStart + adjustment, sourceEnd + adjustment));
                            break;
                        }

                        // completely outside range, try next one
                        if (sourceEnd < rangeStart || sourceStart > rangeEnd)
                        {
                            continue;
                        }

                        // last bit is in range, break in two and add pieces back to queue
                        if (sourceStart < rangeStart)
                        {
                            sources.Add((sourceStart, rangeStart + 1));
                            sources.Add((rangeStart, sourceEnd));
                            break;
                        }

                        // first bit is in range, break in two and add pieces back to queue
                        if (sourceEnd > rangeEnd)
                        {
                            sources.Add((sourceStart, rangeEnd));
                            sources.Add((rangeEnd + 1, sourceEnd));
                            break;
                        }
                    }
                    destinations.Add(source);
                }

                var seedLowest = 0;

            }
        }
          


        //foreach (var seed in seeds)
        //{
        //    var currentSeedNumber = seed;
        //    foreach (var mapping in mappings)
        //    {
        //        foreach (var range in mapping.Ranges)
        //        {
        //            var match = range.GetCorrespondingNumber(currentSeedNumber);
        //            if (match != -1)
        //            {
        //                currentSeedNumber = match;
        //                break;
        //            }
        //        }
        //    }

        //    results.Add(currentSeedNumber);
        //}

        //Console.WriteLine(results.Min());
    }

    private static List<Mapping> GetMappings(List<string> input)
    {
        var currentMapping = new Mapping("","");
        var mappings = new List<Mapping>();

        foreach (var line in input[2..])
        {
            // skip blank lines
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            // start new mapping
            if (line.Contains("map:"))
            {
                var (sourceName, destinationName) = GetSourceAndDestination(line);
                currentMapping = new Mapping(sourceName, destinationName);
                mappings.Add(currentMapping);
                continue;
            }

            // add ranges to mapping
            var (destRangeStart, sourceRangeStart, rangeLength) = GetNumbers(line);
            var range = new Range
            {
                RangeLength = rangeLength,
                SourceRangeStart = sourceRangeStart,
                DestinationRangeStart = destRangeStart,
            };

            currentMapping.Ranges.Add(range);
        }

        return mappings;
    }

    private static (string source, string destination) GetSourceAndDestination(string input)
    {
        var split = input.Split(' ')[0].Split("-to-");
        return (split[0], split[1]);
    }

    private static (long destRangeStart, long sourceRangeStart, long rangeLength) GetNumbers(string input)
    {
        var split = input.Split().Select(long.Parse).ToList();
        return (split[0], split[1], split[2]);
    }

    private static List<long> GetSeeds(string input) =>
        input[7..].Split(' ').Select(long.Parse).ToList();

    private static List<(long, long)> GetSeeds2(string input)
    {
        var seeds = new List<(long, long)>();
        var ints = GetSeeds(input);
        for (var i = 0; i < ints.Count; i += 2)
        {
            seeds.Add((ints[i], ints[i] + ints[i+1]));
        }
        return seeds;
    }

    private static T Pop<T>(List<T> list)
    {
        var popped = list[0];
        list.RemoveAt(0);
        return popped;
    }

    public class Mapping
    {
        public string SourceName;
        public string DestinationName;
        public List<Range> Ranges = new ();

        public Mapping(string sourceName, string destinationName)
        {
            SourceName = sourceName;
            DestinationName = destinationName;
        }
    }

    public class Range
    {
        public long RangeLength { get; set; }
        public long SourceRangeStart { get; set; }
        public long DestinationRangeStart { get; set; }

        public long GetCorrespondingNumber(long sourceNumber)
        {
            // don't care about numbers outside range
            if (!(sourceNumber >= SourceRangeStart
                  && sourceNumber <= SourceRangeStart + RangeLength))
            {
                return -1;
            }

            // return the same difference from the destination
            var difference = sourceNumber - SourceRangeStart;
            return DestinationRangeStart + difference;
        }
    }
}