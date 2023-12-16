namespace AdventOfCode.Common;

public abstract class DayTask
{
    protected string filePath;

    public DayTask(string path)
    {
        filePath = "..\\..\\..\\Inputs\\" + path;
    }

    protected virtual void Initialize() { }

    public abstract void PartOne();
    public abstract void PartTwo();
}
