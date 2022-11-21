using Hw10.DbModels;
using Hw10.Dto;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculator : IMathCalculator
{
    private readonly ApplicationContext _dbContext;
    private readonly IMathCalculator _simpleCalculator;

    public MathCachedCalculator(ApplicationContext dbContext, IMathCalculator simpleCalculator)
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