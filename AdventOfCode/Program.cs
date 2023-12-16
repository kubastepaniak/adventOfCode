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

Console.WriteLine(Console.ReadLine());