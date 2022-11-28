namespace Hw8.Common;

public struct ParseOutput
{
    public double Val1;
    public Operation Operation;
    public double Val2;

    public ParseOutput(double val1, Operation operation, double val2)
    {
        Val1 = val1;
        Operation = operation;
        Val2 = val2;
    }
}