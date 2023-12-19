internal class AdventOfCode
{
    private static readonly Dictionary<char, List<Direction>> Mappings = new()
    {
        { '|', new List<Direction> { Direction.North, Direction.South } },
        { '-', new List<Direction> { Direction.East, Direction.West } },
        { 'L', new List<Direction> { Direction.North, Direction.East } },
        { 'J', new List<Direction> { Direction.North, Direction.West } },
        { '7', new List<Direction> { Direction.South, Direction.West } },
        { 'F', new List<Direction> { Direction.South, Direction.East } },
        { '.', new List<Direction>() },
    };
        
    private static void Main()
    {
        // Update day
        const int day = 10;

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

        var startingPoint = GetStartingPoint(input);
        var nodesInPath = GetLoop(startingPoint, input);
        
        Console.WriteLine(nodesInPath.Count / 2);
    }

    private static void SolvePart2(List<string> input)
    {
        Console.WriteLine("Part 2:");

        var startingPoint = GetStartingPoint(input);
        var nodesInPath = GetLoop(startingPoint, input);
        var corners = new List<char> { 'S', '7', 'L', 'F' };
        var enclosedByLoopCount = 0;

        for (var y = 0; y < input.Count; y++)
        {
            char previousCorner = default;
            var insideLoop = false;
            for (var x = 0; x < input[y].Length; x++)
            {
                var currentNode = input[y][x];
                var partOfLoop = nodesInPath.Contains((x, y));

                // redo ifs
                if (insideLoop && !partOfLoop)
                {
                    currentNode = 'O';
                    enclosedByLoopCount++;
                }

                Console.Write(currentNode);

                if (!partOfLoop)
                {
                    continue;
                }

                // vertical bar of loop
                if (currentNode == '|')
                {
                    insideLoop = !insideLoop;
                }

                // NW loop
                else if (currentNode == 'J')
                {
                    if (previousCorner == 'F')
                    {
                        insideLoop = !insideLoop;
                    }
                }

                // SW loop
                else if (currentNode == '7')
                {
                    if (previousCorner == 'L')
                    {
                        insideLoop = !insideLoop;
                    }
                }

                // Starting node
                else if (currentNode == 'S')
                {
                    insideLoop = !insideLoop;
                }

                if (corners.Contains(currentNode))
                {
                    previousCorner = currentNode;
                }

            }

            Console.WriteLine();
        }

        Console.WriteLine(enclosedByLoopCount);
    }

    private static (int x, int y) GetNextCoord(int x, int y, Direction direction)
    {
        return direction switch
        {
            Direction.North => (x, y - 1),
            Direction.East => (x + 1, y),
            Direction.South => (x, y + 1),
            Direction.West => (x - 1, y),
            _ => default,
        };
    }

    private static (int x, int y) GetStartingPoint(List<string> input)
    {
        (int x, int y) startingPoint = default;
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == 'S')
                {
                    startingPoint = (x, y);
                    break;
                }
            }

            if (startingPoint != default)
            {
                break;
            }
        }
        return startingPoint;
    }

    private static List<(int x, int y)> GetLoop((int x, int y) startingPoint, List<string> input)
    {
        // follow all the way round back to S and count steps
        var currentNode = startingPoint;
        var isStarting = true;
        var nodes = new List<(int, int)>();
        (int x, int y) previousNode = default;

        while (true)
        {
            Direction moveDirection = default;
            var currentNodeValue = input[currentNode.y][currentNode.x];
            nodes.Add(currentNode);

            // check if you're back at the start
            if (!isStarting && currentNodeValue == 'S')
            {
                break;
            }

            // else figure out which way to go next
            if (isStarting)
            {
                moveDirection = FindStartingDirection(input, currentNode);
                isStarting = false;
            }
            else
            {
                foreach (var direction in Mappings[currentNodeValue])
                {
                    var coords = GetNextCoord(currentNode.x, currentNode.y, direction);
                    if (coords != previousNode)
                    {
                        moveDirection = direction;
                    }
                }
            }

            // move along
            previousNode = currentNode;
            currentNode = GetNextCoord(currentNode.x, currentNode.y, moveDirection);
        }

        return nodes;

    }

    private static Direction FindStartingDirection(List<string> input, (int x, int y) startingPoint)
    {
        var northNeighbour = GetNextCoord(startingPoint.x, startingPoint.y, Direction.North);
        var eastNeighbour = GetNextCoord(startingPoint.x, startingPoint.y, Direction.East);
        var westNeighbour = GetNextCoord(startingPoint.x, startingPoint.y, Direction.South);
        var southNeighbour = GetNextCoord(startingPoint.x, startingPoint.y, Direction.West);

        if (Mappings[input[northNeighbour.y][northNeighbour.x]].Contains(Direction.South))
        {
            return Direction.North;
        }

        if (Mappings[input[eastNeighbour.y][eastNeighbour.x]].Contains(Direction.West))
        {
            return Direction.East;
        }

        if (Mappings[input[westNeighbour.y][westNeighbour.x]].Contains(Direction.East))
        {
            return Direction.West;
        }

        if (Mappings[input[southNeighbour.y][southNeighbour.x]].Contains(Direction.North))
        {
            return Direction.South;
        }

        return default;
    }

    private enum Direction
    {
        Unknown,
        North,
        East,
        South,
        West
    }
}