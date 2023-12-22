using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day9 : DayTask
{
    public Day9(string path) : base(path)
    {
        Initialize();
    }

    List<List<int>> data = new();

    protected override void Initialize()
    {
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
               string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    data.Add(Array.ConvertAll(line.Split(' '), int.Parse).ToList());
                }
            }
        }
    }

    public override void PartOne()
    {
        double sum = 0;
        foreach (var row in data)
        {
            sum += row[^1] + Extrapolate(row);
        }
        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        double sum = 0;
        foreach (var row in data)
        {
            sum += row[0] - ExtrapolateBackwards(row);
        }
        Console.WriteLine(sum);
    }

    int Extrapolate(List<int> data)
    {
        if (data.All(x => x == 0))
        {
            return 0;
        }
        else
        {
            List<int> lower = new();
            for (int i = 0; i < data.Count - 1; i++)
            {
                lower.Add(data[i + 1] - data[i]);
            }
            return lower[^1] + Extrapolate(lower);
        }
    }

    int ExtrapolateBackwards(List<int> data)
    {
        if (data.All(x => x == 0))
        {
            return 0;
        }
        else
        {
            List<int> lower = new();
            for (int i = 0; i < data.Count - 1; i++)
            {
                lower.Add(data[i + 1] - data[i]);
            }
            return lower[0] - ExtrapolateBackwards(lower);
        }
    }
}
