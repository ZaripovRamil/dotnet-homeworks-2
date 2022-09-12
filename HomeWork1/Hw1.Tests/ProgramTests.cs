using Xunit;

namespace Hw1.Tests;

public class ProgramTests
{
    [Theory]
    [InlineData("1 + 2")]
    [InlineData("1 / 2")]
    [InlineData("1 - 2")]
    [InlineData("0 / 0")]
    [InlineData("1 / 0")]
    public void ProgramDoesNotFallOnMathOperations(string input)
    {
        RunTest(input, 0);
    }

    [Theory]
    [InlineData("1 2")]
    [InlineData("1")]
    [InlineData("1 . 3 5")]
    [InlineData("1 +")]
    public void ProgramCatchesWrongNumberOfArguments(string input)
    {
        RunTest(input, -2);
    }

    [Theory]
    [InlineData(". + 2")]
    [InlineData("a - f")]
    public void ProgramCatchesNonDoubleInput(string input)
    {
        RunTest(input, -2);
    }

    [Theory]
    [InlineData("3 . 4")]
    public void ProgramCatchesWrongOperations(string input)
    {
        RunTest(input, -3);
    }

    private static void RunTest(string input, int expectedCode)
    {
        var result = Program.Main(input.Split());
        Assert.Equal(expectedCode, result);
    }
}