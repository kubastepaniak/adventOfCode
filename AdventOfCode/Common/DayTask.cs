namespace AdventOfCode.Common;

public class DayTask
{
    protected string filePath;

    public DayTask(string path)
    {
        filePath = "..\\..\\..\\Inputs\\" + path;
    }

    protected virtual void Initialize() { }
}
