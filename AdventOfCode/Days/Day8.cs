using AdventOfCode.Common;

namespace AdventOfCode.Days;

#pragma warning disable CS8604

public class Day8 : DayTask
{
    public Day8(string path) : base(path) 
    {
        Initialize();
    }

    Dictionary<string, (string, string)> NodesStr = new();
    List<char> Moves = new();
    protected override void Initialize()
    {
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                Moves.AddRange(stream.ReadLine());
                
                stream.ReadLine(); // blank line
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    var lineNodes = line.Split(' ', '(', ')', ',', '=').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    NodesStr.Add(lineNodes[0], (lineNodes[1], lineNodes[2]));
                }
            }
        }
    }

    public override void PartOne()
    {
        Console.WriteLine(FindLength("AAA", (step) => { return step == "ZZZ"; }));
    }

    public override void PartTwo()
    {
        var currentSteps = NodesStr.Keys.Where(x => x[2] == 'A').ToList();
        var mins = currentSteps.Select(step =>
        {
            return FindLength(step, (condition) =>
            {
                return condition[2] == 'Z';
            });
        }).ToList();

        if(mins.Count > 1)
        {
            var result = LCM(mins[0], mins[1]);
            for (int i = 2; i < mins.Count; i++)
            {
                result = LCM(result, mins[i]);
            }
            Console.WriteLine(result);
        }
        else
        {
            Console.WriteLine(mins[0]);
        }
    }

    int FindLength(string start, Func<string, bool> predicate)
    {
        var currentStep = start;
        var count = 0;
        while (true)
        {
            foreach (var move in Moves)
            {
                count++;

                currentStep = move == 'L'
                    ? NodesStr[currentStep].Item1
                    : NodesStr[currentStep].Item2;

                if (predicate(currentStep))
                {
                    return count;
                }
            }
        }
    }

    int LCM(int a, int b)
    {
        int num1, num2;
        if (a > b)
        {
            num1 = b; 
            num2 = a;
        }
        else
        {
            num1 = a;
            num2 = b;
        }

        for (int i = 1; i < num1; i++)
        {
            var mult = num1 * i;
            if (mult % num2 == 0) return mult;
        }
        return num1 * num2;
    }
}
