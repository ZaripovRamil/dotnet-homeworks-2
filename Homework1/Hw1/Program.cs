namespace Hw1;

public class Program
{
    public static int Main(string[] args)
    {
        try
        {
            Parser.ParseCalcArguments(args, out var arg1, out var operation, out var arg2);
            var result = Calculator.Calculate(arg1, operation, arg2);
            Console.WriteLine(result);
            return 0;
        }
        catch (ArgumentOutOfRangeException)
        {
            return -1;
        }
        catch (ArgumentException)
        {
            return -2;
        }
        catch (InvalidOperationException)
        {
            return -3;
        }
    }
}