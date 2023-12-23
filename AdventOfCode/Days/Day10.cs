using AdventOfCode.Common;
using System.Linq;

namespace AdventOfCode.Days;

public class Day10 : DayTask
{
    public Day10(string path) : base(path)
    {
        Initialize();
    }

    List<List<Node>> PipesMap = new List<List<Node>>();
    Node StartNode = new Node('S');

    public class Node
    {
        public char Type {  get; set; }

        public (int, int) Coords { get; set; } // for debug only

        public int Distance { get; set; }

        public List<Node> AdjacentPipes { get; set; }

        public Node(char c)
        {
            Type = c;
            AdjacentPipes = new List<Node>();
            Distance = -1; // not yet traversed
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
                    PipesMap.Add(line.Select(x => { return new Node(x); }).ToList());
                }
            }
        }

        for (var i = 0; i < PipesMap.Count; i++)
        {
            for (var j = 0; j < PipesMap[i].Count; j++)
            {
                PipesMap[i][j].Coords = (i, j);
                switch (PipesMap[i][j].Type)
                {
                    case '|': // from up to down
                        {
                            if (i > 0 && "|F7".Contains(PipesMap[i - 1][j].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i - 1][j]);
                            if (i < 139 && "|LJ".Contains(PipesMap[i + 1][j].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i + 1][j]);
                            break;
                        }
                    case '-': // from left to right
                        {
                            if (j > 0 && "-FL".Contains(PipesMap[i][j - 1].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i][j - 1]);
                            if (j < 139 && "-J7".Contains(PipesMap[i][j + 1].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i][j + 1]);
                            break;
                        }
                    case 'L': // from up to right
                        {
                            if (i > 0 && "|F7".Contains(PipesMap[i - 1][j].Type)) 
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i - 1][j]);
                            if (j < 139 && "-J7".Contains(PipesMap[i][j + 1].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i][j + 1]);
                            break;
                        }
                    case 'J': // from up to left
                        {
                            if (i > 0 && "|F7".Contains(PipesMap[i - 1][j].Type)) 
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i - 1][j]);
                            if (j > 0 && "-FL".Contains(PipesMap[i][j - 1].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i][j - 1]);
                            break;
                        }
                    case '7': // from down to left
                        {
                            if (i < 139 && "|LJ".Contains(PipesMap[i + 1][j].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i + 1][j]);
                            if (j > 0 && "-FL".Contains(PipesMap[i][j - 1].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i][j - 1]);
                            break;
                        }
                    case 'F': // from down to right
                        {
                            if (i < 139 && "|LJ".Contains(PipesMap[i + 1][j].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i + 1][j]);
                            if (j < 139 && "-J7".Contains(PipesMap[i][j + 1].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i][j + 1]);
                            break;
                        }
                    case 'S': // start, but all 4 directions
                        {
                            if (i > 0 && "|F7".Contains(PipesMap[i - 1][j].Type)) 
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i - 1][j]);
                            if (i < 139 && "|LJ".Contains(PipesMap[i + 1][j].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i + 1][j]);
                            if (j > 0 && "-FL".Contains(PipesMap[i][j - 1].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i][j - 1]);
                            if (j < 139 && "-J7".Contains(PipesMap[i][j + 1].Type))
                                PipesMap[i][j].AdjacentPipes.Add(PipesMap[i][j + 1]);
                            StartNode = PipesMap[i][j];
                            break;
                        }
                    default: break;
                }
            }
        }
    }

    /*
     * This function gives the correct result however it is not universal.
     * In case the fake pipe branch would be longer than the longest loop,
     * the result would be the one of the fake branch, and not the loop.
     */
    public override void PartOne()
    {
        var currentStepNodes = new List<Node>(StartNode.AdjacentPipes);
        var tmp = new List<Node>();
        var distance = 0;
        StartNode.Distance = distance;

        while(true)
        {
            distance++;
            currentStepNodes.ForEach(x => x.Distance = distance);

            foreach(var node in currentStepNodes)
            {
                tmp.AddRange(node.AdjacentPipes.Where(x => x.Distance == -1));
            }

            if(tmp.Count == 0) break;

            currentStepNodes = new List<Node>(tmp);
            tmp = new List<Node>();
        }
        Console.WriteLine(distance);

        /*
        foreach(var row in PipesMap)
        {
            Console.Write("\n");
            row.ForEach(x =>
            {
                if (x.Distance == 0) 
                {
                    Console.Write("X");
                }
                else if (x.Distance > 0) 
                {
                    Console.Write((char)(x.Distance % 26 + 96));
                }
                else
                {
                    Console.Write('.');
                }
            });
        }
        */
    }

    public override void PartTwo()
    {
        //prune irrelevant 
    }
}
