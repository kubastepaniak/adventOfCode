using AdventOfCode.Common;

namespace AdventOfCode.Days;

public class Day2 : DayTask
{
    public Day2(string path) : base(path) { }

    public class RGB
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public RGB(string data)
        {
            Red = 0;
            Green = 0;
            Blue = 0;

            var tokens = data.Split(',');
            foreach (var token in tokens)
            {
                var values = token.Split(' ');
                switch (values[2])
                {
                    case "red":
                        Red = int.Parse(values[1]);
                        break;
                    case "green":
                        Green = int.Parse(values[1]);
                        break;
                    case "blue":
                        Blue = int.Parse(values[1]);
                        break;
                }
            }
        }
    }

    public class GameData
    {
        public int GameId { get; set; }
        public List<RGB> GamesData { get; set; }

        public GameData(string line) 
        {
            var split = line.Split(':');
            GameId = int.Parse(split[0].Split(' ')[1]);

            GamesData = new List<RGB>();
            var dataSplit = split[1].Split(';');
            foreach (var data in dataSplit)
            {
                GamesData.Add(new RGB(data));
            }
        }

        public (int, int, int) FindLeastAmountOfCubes()
        {
            int leastRed = 0;
            int leastGreen = 0;
            int leastBlue = 0;
            foreach (var data in GamesData)
            {
                if (data.Red > leastRed) 
                {
                    leastRed = data.Red; 
                }
                if (data.Green > leastGreen)
                {
                    leastGreen = data.Green;
                }
                if (data.Blue > leastBlue)
                {
                    leastBlue = data.Blue;
                }
            }
            return (leastRed, leastGreen, leastBlue);
        }

    }

    public override void PartOne()
    {
        List<GameData> data = new List<GameData>();

        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    data.Add(new GameData(line));
                }
            }
        }

        int sumOfPossibleGames = 0;
        foreach(GameData gameData in data)
        {
            if(IsGamePossible(gameData))
            {
                sumOfPossibleGames += gameData.GameId;
            }
        }
        
        Console.WriteLine(sumOfPossibleGames.ToString());
    }

    bool IsGamePossible(GameData gameData)
    {
        int targetRed = 12;
        int targetGreen = 13;
        int targetBlue = 14;

        foreach(RGB data in gameData.GamesData)
        {
            if (data.Red > targetRed) return false;
            if (data.Green > targetGreen) return false;
            if (data.Blue > targetBlue) return false;
        }

        return true;
    }

    public override void PartTwo()
    {
        List<GameData> data = new List<GameData>();

        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    data.Add(new GameData(line));
                }
            }
        }

        var sumOfPowers = 0;
        foreach(GameData gameData in data)
        {
            var leastCubes = gameData.FindLeastAmountOfCubes();
            var power = leastCubes.Item1 * leastCubes.Item2 * leastCubes.Item3;
            sumOfPowers += power;
        }
        Console.WriteLine(sumOfPowers.ToString());
    }
}