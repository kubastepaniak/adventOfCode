using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day3 : DayTask
{
    public Day3(string path) : base(path) 
    {
        Initialize();
    }

    Dictionary<(int, int), Point> pointsMap = new Dictionary<(int, int), Point>();
    List<Number> numbersMap = new List<Number>();

    public class Point
    {
        public char Value { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        
        public Number? Number { get; set; }

        public Point? TL { get; set; }
        public Point? TT { get; set; }
        public Point? TR { get; set; }
        public Point? RR { get; set; }
        public Point? BR { get; set; }
        public Point? BB { get; set; }
        public Point? BL { get; set; }
        public Point? LL { get; set; }

        public bool HasAdjacentSymbol()
        {
            foreach(var point in GetAllAdjacentPoints())
            {
                if(IsSymbol(point.Value)) return true;
            }
            return false;
        }

        public List<Number> GetAdjacentNumbers()
        {
            var numbers = new List<Number>();
            foreach(var point in GetAllAdjacentPoints())
            {
                if (point.Number != null)
                {
                    if(!numbers.Any(n => n.Id == point.Number.Id))
                    {
                        numbers.Add(point.Number);
                    }
                }
            }
            return numbers;
        }

        private List<Point> GetAllAdjacentPoints() 
        {
            var all = new List<Point>();
            if (TL != null) all.Add(TL);
            if (TT != null) all.Add(TT);
            if (TR != null) all.Add(TR);
            if (RR != null) all.Add(RR);
            if (BR != null) all.Add(BR);
            if (BB != null) all.Add(BB);
            if (BL != null) all.Add(BL);
            if (LL != null) all.Add(LL);
            return all;
        }

        private bool IsSymbol(char c)
        {
            if (char.IsDigit(c)) return false;
            if (c ==  '.') return false;
            return true;
        }
    }

    public class Number
    {
        public int Value { get; set; }
        public List<Point> Points { get; set; }
        public int Id { get; set; }

        public Number()
        {
            Points = new List<Point>();
        }

        public bool HasAdjacentSymbol()
        {
            return Points.Any(p => p.HasAdjacentSymbol());
        }
    }

    protected override void Initialize()
    {
        // read for points map
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                int lineNumber = -1;
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    lineNumber++;
                    for (int i = 0; i < line.Length; i++)
                    {
                        pointsMap.Add((i, lineNumber), new Point() { Value = line[i], X = i, Y = lineNumber });
                    }
                }
            }
        }

        // points map
        foreach(var point in pointsMap.Values)
        {
            if (point.X - 1 >= 0 && point.Y - 1 >= 0) point.TL = pointsMap[(point.X - 1, point.Y - 1)];
            if (point.Y - 1 >= 0) point.TT = pointsMap[(point.X, point.Y - 1)];
            if (point.X + 1 < 140 && point.Y - 1 >= 0) point.TR = pointsMap[(point.X + 1, point.Y - 1)];

            if (point.X + 1 < 140) point.RR = pointsMap[(point.X + 1, point.Y)];
            
            if (point.X + 1 < 140 && point.Y + 1 < 140) point.BR = pointsMap[(point.X + 1, point.Y + 1)];
            if (point.Y + 1 < 140) point.BB = pointsMap[(point.X, point.Y + 1)];
            if (point.X - 1 >= 0 && point.Y + 1 < 140) point.BL = pointsMap[(point.X - 1, point.Y + 1)];
            
            if (point.X - 1 >= 0) point.LL = pointsMap[(point.X - 1, point.Y)];
        }

        // read for numbers map and map
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                int lineNumber = -1;
                string? line;
                string codeValue;
                int idCounter = 100;

                while ((line = stream.ReadLine()) != null)
                {
                    lineNumber++;
                    for (int i = 0; i < line.Length;)
                    {
                        codeValue = "";
                        if (line[i] != '.' && char.IsDigit(line[i]))
                        {
                            Number number = new();
                            number.Id = idCounter++;
                            do
                            {
                                codeValue += line[i];
                                pointsMap[(i, lineNumber)].Number = number;
                                number.Points.Add(pointsMap[(i, lineNumber)]);
                            } while ((++i < line.Length) && (char.IsDigit(line[i])));
                            number.Value = int.Parse(codeValue);
                            numbersMap.Add(number);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }
        }
    }

    public override void PartOne()
    {
        int sum = 0;
        foreach (var number in numbersMap)
        {
            if (number.HasAdjacentSymbol())
            {
                sum += number.Value;
            }
        }

        Console.WriteLine(sum);
    }

    public override void PartTwo()
    {
        int sum = 0;
        foreach(var point in pointsMap.Values)
        {
            if(point.Value == '*')
            {
                var numbers = point.GetAdjacentNumbers();
                if (numbers.Count == 2)
                {
                    sum += numbers[0].Value * numbers[1].Value;
                }
            }
        }

        Console.WriteLine(sum);
    }
}
