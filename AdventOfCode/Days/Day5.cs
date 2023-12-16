using AdventOfCode.Common;

namespace AdventOfCode.Days;

#pragma warning disable CS8602

public class Day5 : DayTask
{
    public Day5(string path) : base(path) 
    {
        Initialize();
    }

    List<double> Seeds = new();
    ValuesMap SeedToSoil = new();
    ValuesMap SoilToFertilizer = new();
    ValuesMap FertilizerToWater = new();
    ValuesMap WaterToLight = new();
    ValuesMap LightToTemperature = new();
    ValuesMap TemperatureToHumidity = new();
    ValuesMap HumidityToLocation = new();
    static Comparer<ValuesMapRange> rangeComparer = Comparer<ValuesMapRange>.Create((range1, range2) => range1.CompareTo(range2));

    public class ValuesMapRange : IComparable<ValuesMapRange>
    {
        public double SourceStart;
        public double SourceEnd;
        public double DestinationStart;
        public double DestinationEnd;

        public int CompareTo(ValuesMapRange? other)
        {
            var result = Math.Sign(SourceStart - other.SourceStart);
            if(result == 0)
            {
                return Math.Sign(DestinationStart - other.DestinationStart);
            }
            return result;
        }
    }

    public class ValuesMap
    {
        public SortedSet<ValuesMapRange> MapRanges = new(rangeComparer);

        public void AddValuesRange(double destinationStart, double sourceStart, double range)
        {
            MapRanges.Add(new ValuesMapRange() { SourceStart = sourceStart, SourceEnd = sourceStart + range, DestinationStart = destinationStart, DestinationEnd = destinationStart + range });
        }


        public double GetValue(double source)
        {
            foreach (var range in MapRanges)
            {
                if(source >= range.SourceStart && source <= range.SourceEnd)
                {
                    return range.DestinationStart + (source - range.SourceStart);
                }
            }
            return source;
        }

        public SortedSet<ValuesMapRange> GetValuesRanges(double start, double end)
        {
            SortedSet<ValuesMapRange> values = new(rangeComparer);

            foreach (var range in MapRanges)
            {
                if(start < range.SourceStart && range.CompareTo(MapRanges.ToList()[0]) == 0)
                {
                    values.Add(new ValuesMapRange()
                    {
                        SourceStart = start,
                        SourceEnd = range.SourceStart,
                        DestinationStart = start,
                        DestinationEnd = range.SourceStart
                    });
                    start = range.SourceStart;
                }
                if(start < range.SourceEnd && end > range.SourceStart)
                {
                    var newRangeOffset = start - range.SourceStart;
                    var newRangeLength = range.SourceEnd < end ? range.SourceEnd - start : end - start;
                    values.Add(new ValuesMapRange()
                    {
                        SourceStart = start,
                        SourceEnd = start + newRangeLength,
                        DestinationStart = range.DestinationStart + newRangeOffset,
                        DestinationEnd = range.DestinationStart + newRangeOffset + newRangeLength
                    });

                    if(end > range.SourceEnd)
                    {
                        start = range.SourceEnd + 1;
                    }
                }
                if(start > range.SourceEnd && range.CompareTo(MapRanges.ToList()[MapRanges.Count -1]) == 0)
                {
                    values.Add(new ValuesMapRange()
                    {
                        SourceStart = start,
                        SourceEnd = end,
                        DestinationStart = start,
                        DestinationEnd = end
                    });
                }
            }

            return values;
        }
    }

    protected override void Initialize()
    {
        using (var input = File.OpenRead(filePath))
        {
            using (var stream = new StreamReader(input))
            {
                var seedsLine = stream.ReadLine();
                var seeds = seedsLine?.Split(' ').Where(x => x != "seeds:").ToList();
                foreach (var seed in seeds) 
                { 
                    Seeds.Add(double.Parse(seed)); 
                }

                stream.ReadLine();
                while (!stream.EndOfStream)
                {
                    var mapTitle = stream.ReadLine();
                    string? line;
                    while ((line = stream.ReadLine()) != "" && line != null)
                    {
                        switch (mapTitle)
                        {
                            case "seed-to-soil map:":
                                {
                                    var row = line.Split(' ');
                                    SeedToSoil.AddValuesRange(double.Parse(row[0]), double.Parse(row[1]), double.Parse(row[2]));
                                    break;
                                }
                            case "soil-to-fertilizer map:":
                                {
                                    var row = line.Split(' ');
                                    SoilToFertilizer.AddValuesRange(double.Parse(row[0]), double.Parse(row[1]), double.Parse(row[2]));
                                    break;
                                }
                            case "fertilizer-to-water map:":
                                {
                                    var row = line.Split(' ');
                                    FertilizerToWater.AddValuesRange(double.Parse(row[0]), double.Parse(row[1]), double.Parse(row[2]));
                                    break;
                                }
                            case "water-to-light map:":
                                {
                                    var row = line.Split(' ');
                                    WaterToLight.AddValuesRange(double.Parse(row[0]), double.Parse(row[1]), double.Parse(row[2]));
                                    break;
                                }
                            case "light-to-temperature map:":
                                {
                                    var row = line.Split(' ');
                                    LightToTemperature.AddValuesRange(double.Parse(row[0]), double.Parse(row[1]), double.Parse(row[2]));
                                    break;
                                }
                            case "temperature-to-humidity map:":
                                {
                                    var row = line.Split(' ');
                                    TemperatureToHumidity.AddValuesRange(double.Parse(row[0]), double.Parse(row[1]), double.Parse(row[2]));
                                    break;
                                }
                            case "humidity-to-location map:":
                                {
                                    var row = line.Split(' ');
                                    HumidityToLocation.AddValuesRange(double.Parse(row[0]), double.Parse(row[1]), double.Parse(row[2]));
                                    break;
                                }
                        }
                    }
                }
            }
        }
    }

    public override void PartOne()
    {
        double lowestLocation = double.MaxValue;
        foreach(var seed in Seeds)
        {
            double locationForSeed = HumidityToLocation.GetValue(
                TemperatureToHumidity.GetValue(
                    LightToTemperature.GetValue(
                        WaterToLight.GetValue(
                            FertilizerToWater.GetValue(
                                SoilToFertilizer.GetValue(
                                    SeedToSoil.GetValue(seed)))))));
            if(lowestLocation > locationForSeed)
            {
                lowestLocation = locationForSeed;
            }
        }

        Console.WriteLine(lowestLocation);
    }

    public override void PartTwo()
    {
        List<(double, double)> SeedsRanges = new();
        for (int i = 0; i <  Seeds.Count; i += 2)
        {
            SeedsRanges.Add((Seeds[i], Seeds[i] + Seeds[i + 1]));
        }

        SortedSet<ValuesMapRange> soilsRanges = new(rangeComparer);
        foreach (var seedRange in SeedsRanges)
        {
            var tmpRange = SeedToSoil.GetValuesRanges(seedRange.Item1, seedRange.Item2);
            foreach (var range in tmpRange)
            {
                soilsRanges.Add(range);
            }
        }

        SortedSet<ValuesMapRange> fertilizersRanges = new(rangeComparer);
        foreach (var soilRange in soilsRanges)
        {
            var tmpRange = SoilToFertilizer.GetValuesRanges(soilRange.DestinationStart, soilRange.DestinationEnd);
            foreach (var range in tmpRange)
            {
                fertilizersRanges.Add(range);
            }
        }

        SortedSet<ValuesMapRange> waterRanges = new(rangeComparer); ;
        foreach (var fertilizerRange in fertilizersRanges)
        {
            var tmpRange = FertilizerToWater.GetValuesRanges(fertilizerRange.DestinationStart, fertilizerRange.DestinationEnd);
            foreach (var range in tmpRange)
            {
                waterRanges.Add(range);
            }
        }

        SortedSet<ValuesMapRange> lightRanges = new(rangeComparer); ;
        foreach (var waterRange in waterRanges)
        {
            var tmpRange = WaterToLight.GetValuesRanges(waterRange.DestinationStart, waterRange.DestinationEnd);
            foreach (var range in tmpRange)
            {
                lightRanges.Add(range);
            }
        }

        SortedSet<ValuesMapRange> temperatureRanges = new(rangeComparer); ;
        foreach (var lightRange in lightRanges)
        {
            var tmpRange = LightToTemperature.GetValuesRanges(lightRange.DestinationStart, lightRange.DestinationEnd);
            foreach (var range in tmpRange)
            {
                temperatureRanges.Add(range);
            }
        }

        SortedSet<ValuesMapRange> humidityRanges = new(rangeComparer); ;
        foreach (var temperatureRange in temperatureRanges)
        {
            var tmpRange = TemperatureToHumidity.GetValuesRanges(temperatureRange.DestinationStart, temperatureRange.DestinationEnd);
            foreach (var range in tmpRange)
            {
                humidityRanges.Add(range);
            }
        }

        SortedSet<ValuesMapRange> locationRanges = new(rangeComparer); ;
        foreach (var humidityRange in humidityRanges)
        {
            var tmpRange = HumidityToLocation.GetValuesRanges(humidityRange.DestinationStart, humidityRange.DestinationEnd);
            foreach (var range in tmpRange)
            {
                locationRanges.Add(range);
            }
        }

        double lowest = double.MaxValue;
        foreach (var locationRange in locationRanges)
        {
            if(locationRange.DestinationStart < lowest) lowest = locationRange.DestinationStart;
        }

        Console.WriteLine(lowest);
    }
}
