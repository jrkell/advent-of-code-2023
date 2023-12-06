using System.Collections;

internal class AdventOfCode
{
    private static void Main()
    {
        // Update day
        const int day = 3;

        Console.WriteLine($"Advent of Code 2023 - Day {day}");
        Console.WriteLine();

        var inputLines = File.ReadAllLines($"input.txt").ToList();

        var grid = new Grid(inputLines.Count, inputLines[0].Length);
        PopulateGrid(grid, inputLines);
        PopulateNeighbours(grid);

        SolvePart1(grid);
        SolvePart2(grid);

        Console.WriteLine("Press enter to exit...");
        Console.ReadLine();
    }

    private static void SolvePart1(Grid grid)
    {
        Console.WriteLine("Part 1:");

        var total = 0;
        foreach (var cell in grid)
        {
            if (cell.Type.Equals(CellType.Number) && cell.Neighbours.Any(n => n.Type.Equals(CellType.Symbol)))
            {
                total += int.Parse(cell.Value);
            }
        }
        
        Console.WriteLine(total);
    }

    private static void SolvePart2(Grid grid)
    {
        Console.WriteLine("Part 2:");

        var total = 0;

        foreach (var cell in grid)
        {
            if (IsAsterisk(cell.Value))
            {
                var numberNeighbours = cell.Neighbours.Where(n => n.Type == CellType.Number).ToList();
                var uniqueNumberNeighbours = numberNeighbours.DistinctBy(x => x.Value).ToList();

                if (uniqueNumberNeighbours.Count == 2) 
                {
                    total += (int.Parse(uniqueNumberNeighbours[0].Value) * int.Parse(uniqueNumberNeighbours[1].Value));
                }
            }
        }

        Console.WriteLine(total);
    }

    private static void PopulateGrid(Grid grid, List<string> input)
    {
        for (var y = 0; y < input.Count; y++)
        {
            var currentRow = input[y];
            for (var x = 0; x < currentRow.Length; x++)
            {
                var currentChar = currentRow[x];
                if (IsSymbol(currentChar))
                {
                    grid.Map[y][x] = new Cell { Type = CellType.Symbol, Length = 1, Value = currentChar.ToString() };
                    continue;
                }

                if (IsNumber(currentChar))
                {
                    var number = GetFullNumber(currentRow[x..]);
                    var numberLength = number.Length;
                    var numberCell = new Cell { Type = CellType.Number, Length = numberLength, Value = number.ToString() };
                    
                    grid.Map[y][x] = numberCell;
                    for (var i = x+1; i < x + numberLength; i++)
                    {
                        grid.Map[y][i] = new Cell { Type = CellType.Number, Length = numberLength, Value = number.ToString(), Duplicate = true };
                    }

                    // skip other cells in number
                    x += numberLength - 1;
                    continue;
                }

                // else is dot
                grid.Map[y][x] = new Cell { Type = CellType.Dot, Length = 1, Value = currentChar.ToString() };
            }
        }
    }

    private static string GetFullNumber(string input)
    {
        var numberLength = 0;
        while (true)
        {
            // found a dot or symbol
            if (!IsNumber(input[numberLength]))
            {
                break;
            }

            // reached the end of the row
            if (numberLength == input.Length - 1)
            {
                numberLength++;
                break;
            }

            numberLength++;
        }

        return input[..numberLength];
    }

    private static bool IsSymbol(char character) =>
        character != '.' && !IsNumber(character);

    private static bool IsNumber(char character) =>
        int.TryParse(character.ToString(), out _);

    private static bool IsAsterisk(string str) =>
        str == "*";

    private static List<Cell?> GetNeighbouringCells(int x, int y, Grid grid)
    {
        var cell = grid.Map[y][x];
        var startX = x;
        var endX = x + cell.Length - 1;

        // calculate coords in all directions
        var coordsList = new List<Cell?>
        {
            grid.SafeGet(x-1, y), // left
            grid.SafeGet(endX+1, y), // right
            grid.SafeGet(x-1, y-1), // up-left
            grid.SafeGet(endX+1, y-1), // up-right
            grid.SafeGet(x-1, y+1), // down-left
            grid.SafeGet(endX+1, y+1), // down-right
        };

        for (var i = startX; i <= endX; i++)
        {
            coordsList.Add(grid.SafeGet(i, y - 1)); // up
            coordsList.Add(grid.SafeGet(i, y + 1)); // down
        }

        return coordsList.Where(x => x != null && x != cell).ToList();
    }

    private static void PopulateNeighbours(Grid grid)
    {
        for (var y=0; y<grid.Map.Count; y++)
        {
            for (var x=0; x<grid.Map[y].Count; x++)
            {
                var cell = grid.SafeGet(x, y);
                if (cell != null && cell.Neighbours.Count == 0)
                {
                    cell.Neighbours = GetNeighbouringCells(x, y, grid);
                }
            }
        }
    }
}

public class Grid : IEnumerable<Cell>
{
    public List<List<Cell?>> Map { get; set; }

    public Grid(int width, int height)
    {
        Map = new List<List<Cell?>>(height);

        // initialise with all null cells
        for (var y = 0; y < height; y++)
        {
            Map.Add(new List<Cell?>(width));
            for (var x = 0; x < width; x++)
            {
                Map[y].Add(null);
            }
        }
    }

    public IEnumerator<Cell> GetEnumerator()
    {
        foreach (var row in Map)
        {
            foreach (var cell in row.Where(cell => cell != null && cell.Duplicate != true))
            {
                yield return cell;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public Cell? SafeGet(int x, int y)
    {
        try
        {
            return Map[y][x];
        }
        catch (ArgumentOutOfRangeException)
        {
            return null;
        }
    }
}

public class Cell
{
    public string Value;
    public CellType Type;
    public int Length;
    public List<Cell?> Neighbours = [];
    public bool Duplicate;
}

public enum CellType
{
    Dot,
    Symbol,
    Number
}