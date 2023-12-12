namespace AdventOfCode;

public class Day1
{
    string filePath;

    public Day1(string path) 
    {
        filePath = path;
    }

    Dictionary<int, string> numbersDictionary = new Dictionary<int, string>()
    {
        { 1, "one" },
        { 2, "two" },
        { 3, "three" },
        { 4, "four" },
        { 5, "five" },
        { 6, "six" },
        { 7, "seven" },
        { 8, "eight" },
        { 9, "nine" }
    };

    public void PartOne()
    {
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                int sum = 0;
                string? line;

                while ((line = stream.ReadLine()) != null)
                {
                    var numbers = ExtractNumbersString(line);
                    sum += GetCodeFromNumbersString(numbers);
                }
                Console.WriteLine(sum);
            }
        }
    }

    public void PartTwo()
    {
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                string? line;
                var sum = 0;
                while ((line = stream.ReadLine()) != null)
                {
                    var numbers = ExtractNumbersStringWithLiterals(line);
                    sum += GetCodeFromNumbersString(numbers);
                }
                Console.WriteLine(sum);
            }
        }
    }

    string ExtractNumbersString(string line)
    {
        string numbers = "";
        foreach (char c in line)
        {
            if (char.IsDigit(c)) numbers += c;
        }
        return numbers;
    }

    string ExtractNumbersStringWithLiterals(string line)
    {
        List<(int, int)> numbersWithPositions = new List<(int, int)> ();
        for (int i = 1; i < 10; i++)
        {
            int position = 0;
            while(true)
            {
                position = line.IndexOf(i.ToString(), position);
                
                if (position == -1) break;

                numbersWithPositions.Add((i, position));
                position++;
            }

            position = 0;
            while (true)
            {
                position = line.IndexOf(numbersDictionary[i], position);

                if (position == -1) break;

                numbersWithPositions.Add((i, position));
                position++;
            }
        }
        numbersWithPositions.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        string numbers = "";
        foreach((int, int) number  in numbersWithPositions)
        {
            numbers += number.Item1.ToString();
        }
        return numbers;
    }

    int GetCodeFromNumbersString(string numbers) => int.Parse($"{numbers[0]}{numbers[numbers.Length - 1]}");
}