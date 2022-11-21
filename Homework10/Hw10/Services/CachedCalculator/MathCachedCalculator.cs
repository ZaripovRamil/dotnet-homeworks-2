using Hw10.DbModels;
using Hw10.Dto;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculator : MathCalculator.MathCalculator
{
    private readonly ApplicationContext _dbContext;

    public MathCachedCalculator(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<CalculationMathExpressionResultDto>  CalculateMathExpressionAsync(string? expression)
    {
        var cachedExpression = _dbContext.SolvingExpressions
            .FirstOrDefault(cached => cached.Expression == expression);
        if (cachedExpression != null) return new CalculationMathExpressionResultDto(cachedExpression.Result);
        var resultDto = await base.CalculateMathExpressionAsync(expression);
        if (!resultDto.IsSuccess) return resultDto;
        _dbContext.SolvingExpressions.Add(new SolvingExpression(expression!, resultDto.Result));
        await _dbContext.SaveChangesAsync();
        return resultDto;
    }
}