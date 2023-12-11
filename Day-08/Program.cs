using System;
using System.Runtime.InteropServices;

internal class AdventOfCode
{
    private static void Main()
    {
        // Update day
        const int day = 1;

        Console.WriteLine($"Advent of Code 2023 - Day {day}");
        Console.WriteLine();

        var input = File.ReadAllLines($"input.txt").ToList();

        // get instructions and mappings
        var instructions = input[0];
        var mappings = new Dictionary<string, (string, string)>();
        for (var i = 2; i < input.Count; i++)
        {
            var mapping = input[i][..3];
            var left = input[i][7..10];
            var right = input[i][12..15];
            mappings.Add(mapping, (left, right));
        }

        SolvePart1(instructions, mappings);
        SolvePart2(instructions, mappings);

        Console.WriteLine("Press enter to exit...");
        Console.ReadLine();
    }

    private static void SolvePart1(string instructions, Dictionary<string, (string, string)>  mappings)
    {
        Console.WriteLine("Part 1:");

        var numSteps = 0;
        var instructionNum = -1;
        var currentNode = "AAA";
        while (currentNode != "ZZZ")
        {
            numSteps++;

            if (instructionNum == instructions.Length - 1)
            {
                instructionNum = 0;
            }
            else
            {
                instructionNum++;
            }

            var instruction = instructions[instructionNum];

            if (instruction == 'R')
            {
                currentNode = mappings[currentNode].Item2;
            }
            else
            {
                currentNode = mappings[currentNode].Item1;
            }
        }

        Console.WriteLine(numSteps);
    }

    private static void SolvePart2(string instructions, Dictionary<string, (string, string)> mappings)
    {
        Console.WriteLine("Part 2:");

        var startingNodes = mappings
            .Where(x => x.Key[2] == 'A')
            .Select(x => x.Key)
            .ToList();

        var shortestPaths = new List<int>();

        foreach (var startingNode in startingNodes)
        {
            var numSteps = 0;
            var instructionNum = -1;
            var currentNode = startingNode;

            while (currentNode[2] != 'Z')
            {
                numSteps++;

                if (instructionNum == instructions.Length - 1)
                {
                    instructionNum = 0;
                }
                else
                {
                    instructionNum++;
                }

                var instruction = instructions[instructionNum];

                if (instruction == 'R')
                {
                    currentNode = mappings[currentNode].Item2;
                }
                else
                {
                    currentNode = mappings[currentNode].Item1;
                }
            }

            shortestPaths.Add(numSteps);
        }


        Console.WriteLine(CalculateLCM(shortestPaths.Select(x => (long) x).ToArray()));
    }

    private static long CalculateGCD(long n1, long n2)
    {
        return n2 == 0 ? n1 : CalculateGCD(n2, n1 % n2);
    }

    private static long CalculateLCM(long[] numbers)
    {
        return numbers.Aggregate((S, val) => S * val / CalculateGCD(S, val));
    }
}