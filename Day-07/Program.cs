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

        var hands = new List<(string hand, int score, int bid)>();

        foreach (var line in input)
        {
            var tokens = line.Split(" ").ToList();
            var hand = tokens[0];
            var bid = int.Parse(tokens[1]);
            hands.Add((hand, GetScore(hand), bid));
        }

        hands.Sort(new HandComparer());

        var total = 0;
        for (var i = 0; i < hands.Count; i++)
        {
            total += ((i+1) * hands[i].bid);
        }

        Console.WriteLine(total);
    }

    private static void SolvePart2(List<string> input)
    {
        Console.WriteLine("Part 2:");

        var hands = new List<(string hand, int score, int bid)>();

        foreach (var line in input)
        {
            var tokens = line.Split(" ").ToList();
            var hand = tokens[0];
            var bid = int.Parse(tokens[1]);
            //Console.WriteLine($"{hand} - {GetScore(hand, true)}");
            hands.Add((hand, GetScore(hand, true), bid));
        }

        hands.Sort(new HandComparer2());

        var total = 0;
        for (var i = 0; i < hands.Count; i++)
        {
            total += ((i + 1) * hands[i].bid);
        }

        Console.WriteLine(total);

        // ...
    }

    // convert type to score
    private static int GetScore(string hand, bool useJokerRule = false)
    {
        // get a list of all unique chars and sort by their counts
        List<(char key, int count)> charCounts = hand
            .GroupBy(x => x)
            .Select(group => (group.Key, Count: group.Count()))
            .OrderByDescending(tuple => tuple.Count)
            .ToList();

        if (useJokerRule)
        {
            var joker = charCounts.FirstOrDefault(x => x.key == 'J');
            // remove jokers and add count to top card
            if (joker != default)
            {
                var numOfJokers = joker.count;

                if (numOfJokers < 5)
                {

                    charCounts.RemoveAt(charCounts.IndexOf(joker));

                    var firstCharCopy = charCounts[0];
                    firstCharCopy.count += numOfJokers;
                    charCounts[0] = firstCharCopy;
                }
            }
        }

        // five of a kind
        if (charCounts.Count == 1)
        {
            return 6;
        }

        // four of a kind
        if (charCounts[0].count == 4)
        {
            return 5;
        }

        if (charCounts[0].count == 3)
        {
            // full house
            if (charCounts[1].count == 2)
            {
                return 4;
            }

            // three of a kind
            return 3;
        }

        if (charCounts[0].count == 2)
        {
            // two pair
            if (charCounts[1].count == 2)
            {
                return 2;
            }

            // one pair
            return 1;
        }
        
        // high card
        return 0;
    }

    private class HandComparer : IComparer<(string hand, int score, int bid)>
    {
        private readonly List<char> _cardRankings = new() { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };

        public int Compare((string hand, int score, int bid) a, (string hand, int score, int bid) b)
        {
            if (a.score > b.score)
            {
                return 1;
            }

            if (b.score > a.score)
            {
                return -1;
            }

            for (var i = 0; i < 5; i++)
            {
                var aRank = _cardRankings.IndexOf(a.hand[i]);
                var bRank = _cardRankings.IndexOf(b.hand[i]);

                if (aRank > bRank)
                {
                    return -1;
                }

                if (bRank > aRank)
                {
                    return 1;
                }

                // else next letter
            }

            return 0;
        }
    }

    private class HandComparer2 : IComparer<(string hand, int score, int bid)>
    {
        private readonly List<char> _cardRankings = new() { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };

        public int Compare((string hand, int score, int bid) a, (string hand, int score, int bid) b)
        {
            if (a.score > b.score)
            {
                return 1;
            }

            if (b.score > a.score)
            {
                return -1;
            }

            for (var i = 0; i < 5; i++)
            {
                var aRank = _cardRankings.IndexOf(a.hand[i]);
                var bRank = _cardRankings.IndexOf(b.hand[i]);

                if (aRank > bRank)
                {
                    return -1;
                }

                if (bRank > aRank)
                {
                    return 1;
                }

                // else next letter
            }

            return 0;
        }
    }
}