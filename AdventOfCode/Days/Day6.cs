using AdventOfCode.Common;
using System.Diagnostics;

namespace AdventOfCode.Days;

#pragma warning disable CS8604

public class Day6 : DayTask
{
    public Day6(string path) : base(path)
    {
        Initialize();
    }

    List<(int, int)> Races = new List<(int, int)>();

    protected override void Initialize()
    {
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                var times = stream.ReadLine()?.Split(' ').Where(x => x != "" && int.TryParse(x, out _)).ToList().Select(int.Parse).ToList();
                var distances = stream.ReadLine()?.Split(' ').Where(x => x != "" && int.TryParse(x, out _)).ToList().Select(int.Parse).ToList();
                Races.AddRange(times.Zip(distances));
            }
        }
    }

    public override void PartOne()
    {
        var winningCases = new List<int>();
        for (int i = 0; i < Races.Count; i++)
        {
            var cases = GenerateCases(Races[i].Item1);
            winningCases.Add(cases.Where((x) => x.Value > Races[i].Item2).Count());
        }

        var result = 1;
        foreach(var count in winningCases) result *= count;
        Console.WriteLine(result);
    }

    public override void PartTwo()
    {
        string timeString = "";
        string distanceString = "";

        foreach(var race in Races)
        {
            timeString += race.Item1;
            distanceString += race.Item2;
        }

        long bigTime = long.Parse(timeString);
        long bigDistance = long.Parse(distanceString);

        // brute force Part One solution ~ 2.2s
        Stopwatch sw1 = Stopwatch.StartNew();

        var cases = GenerateCases(bigTime);
        var winningCases = cases.Where(x => x.Value > bigDistance).Count();
        
        sw1.Stop();
        Console.WriteLine(winningCases + " \t Time: " + sw1.ElapsedMilliseconds);

        // brute force no dictionary solution ~ 90ms
        winningCases = 0;
        Stopwatch sw2 = Stopwatch.StartNew();

        for (long windUpTime = 0; windUpTime <= bigTime; windUpTime++)
        {
            if((bigTime - windUpTime) * windUpTime > bigDistance) winningCases++;
        }

        sw2.Stop();
        Console.WriteLine(winningCases + " \t Time: " + sw2.ElapsedMilliseconds);

        // calculating from deviation ~ 15ms
        winningCases = 0;
        long firstCase = 0;
        Stopwatch sw3 = Stopwatch.StartNew();

        for (long windUpTime = 0; windUpTime <= bigTime; windUpTime++)
        {
            if ((bigTime - windUpTime) * windUpTime > bigDistance)
            {
                firstCase = windUpTime;
                break;
            }
        }
        // bigTime is even so it's easier
        winningCases = (int)(bigTime - (firstCase * 2));

        sw3.Stop();
        Console.WriteLine(winningCases + " \t Time: " + sw3.ElapsedMilliseconds);
    }

    private Dictionary<long, long> GenerateCases(long time)
    {
        var cases = new Dictionary<long, long>();
        for (long windUpTime = 0; windUpTime <= time; windUpTime++)
        {   
            var swimDistance = (time - windUpTime) * (windUpTime);
            cases.Add(windUpTime, swimDistance);
        }
        return cases;
    }
}
