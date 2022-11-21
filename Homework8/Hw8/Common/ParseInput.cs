namespace Hw8.Common;

public struct ParseInput
{
    public string Val1;
    public string Operation;
    public string Val2;

    public ParseInput(string val1, string operation, string val2)
    {
        Val1 = val1;
        Operation = operation;
        Val2 = val2;
    }
}