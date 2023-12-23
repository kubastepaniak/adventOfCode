using AdventOfCode.Days;

Console.WriteLine("======= DAY 1 =======");
var day1 = new Day1("day1.txt");
day1.PartOne();
day1.PartTwo();

Console.WriteLine("======= DAY 2 =======");
var day2 = new Day2("day2.txt");
day2.PartOne();
day2.PartTwo();

Console.WriteLine("======= DAY 3 =======");
var day3 = new Day3("day3.txt");
day3.PartOne();
day3.PartTwo();

Console.WriteLine("======= DAY 4 =======");
var day4 = new Day4("day4.txt");
day4.PartOne();
day4.PartTwo();

Console.WriteLine("======= DAY 5 =======");
var day5 = new Day5("day5.txt");
day5.PartOne();
day5.PartTwo();
// Due to not yet debugged issue, the result of day 5 displayed is one higher that the real anwser.
// Don't ask me how I guessed that, I just had a hunch that I messed up some destination edge case or indexing during translation

Console.WriteLine("======= DAY 6 =======");
var day6 = new Day6("day6.txt");
day6.PartOne();
//day6.PartTwo(); // all 3 methods, so commented because it lags

Console.WriteLine("======= DAY 7 =======");
var day7 = new Day7("day7.txt");
day7.PartOne();
day7.PartTwo();

Console.WriteLine("======= DAY 8 =======");
var day8 = new Day8("day8.txt");
day8.PartOne();
//day8.PartTwo(); no clue tbh

Console.WriteLine("======= DAY 9 =======");
var day9 = new Day9("day9.txt");
day9.PartOne();
day9.PartTwo();

Console.WriteLine("======= DAY 10 =======");
var day10 = new Day10("day10.txt");
day10.PartOne();
day10.PartTwo();

Console.WriteLine(Console.ReadLine());