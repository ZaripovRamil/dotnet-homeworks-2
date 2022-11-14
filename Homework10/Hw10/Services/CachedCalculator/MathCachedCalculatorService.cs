using Hw10.DbModels;
using Hw10.Dto;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
    private readonly ApplicationContext _dbContext;
    private readonly IMathCalculatorService _simpleCalculator;

    public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
    {
        _dbContext = dbContext;
        _simpleCalculator = simpleCalculator;
    }

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var cachedExpression = _dbContext.SolvingExpressions
            .FirstOrDefault(cached => cached.Expression == expression);
        if (cachedExpression != null) return new CalculationMathExpressionResultDto(cachedExpression.Result);
        var resultDto = await _simpleCalculator.CalculateMathExpressionAsync(expression);
        if (!resultDto.IsSuccess) return resultDto;
        _dbContext.SolvingExpressions.Add(new SolvingExpression(expression!, resultDto.Result));
        await _dbContext.SaveChangesAsync();
        return resultDto;
    }
}