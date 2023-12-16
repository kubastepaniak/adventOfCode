using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day4 : DayTask
{
    public Day4(string path) : base(path) 
    {
        Initialize();
    }
    
    List<Scratchcard> scratchcards = new();

    public class Scratchcard
    {
        public int Index { get; set; }
        public double Weight { get; set; }
        public string[] WinningNumbers { get; set; }
        public string[] CurrentNumbers { get; set; }

        public Scratchcard(int index, double weight, string[] winningNumbers, string[] currentNumbers)
        {
            Index = index;
            Weight = weight;
            WinningNumbers = winningNumbers;
            CurrentNumbers = currentNumbers;
        }
    }

    protected override void Initialize()
    {
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    var gameDataSplit = line.Split(':');
                    var indexToken = gameDataSplit[0];
                    var gameDataToken = gameDataSplit[1];

                    var indexSplit = indexToken.Split(' ');
                    var index = int.Parse(indexSplit[indexSplit.Length - 1]);

                    var numbersSplit = gameDataToken.Split('|');
                    var winningNumbersToken = numbersSplit[0];
                    var currentNumbersToken = numbersSplit[1];

                    var winningNumbers = winningNumbersToken.Split(' ').Where(x => x != "").ToArray();
                    var currentNumbers = currentNumbersToken.Split(' ').Where(x => x != "").ToArray();

                    scratchcards.Add(new Scratchcard(index, 1, winningNumbers, currentNumbers));
                }
            }
        }
    }

    public override void PartOne()
    {
        int sum = 0;
        foreach(var scratchcard in scratchcards)
        {
            sum += (int)Math.Pow(2, CountMatches(scratchcard.WinningNumbers, scratchcard.CurrentNumbers) - 1);
        }
        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        double sum = 0;
        for(int i = 0; i < scratchcards.Count; i++)
        {
            var scratchcard = scratchcards[i];
            var matches = CountMatches(scratchcard.WinningNumbers, scratchcard.CurrentNumbers);
            
            for(int j = i + 1; j <= Math.Min(i + matches, scratchcards.Count -1); j++)
            {
                scratchcards[j].Weight += scratchcard.Weight;
            }
        }

        foreach (var scratchcard in scratchcards)
        {
            sum += scratchcard.Weight;
        }
        Console.WriteLine(sum);
    }

    int CountMatches(string[] winningNumbers, string[] currentNumbers)
    {
        int count = 0;
        foreach (string winningNumber in winningNumbers)
        {
            if (currentNumbers.Contains(winningNumber)) count++;
        }
        return count;
    }
}
