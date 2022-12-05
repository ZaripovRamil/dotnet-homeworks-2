using Hw10.DbModels;
using Hw10.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculator : MathCalculator.MathCalculator
{
    private readonly ApplicationContext _dbContext;
    private IMemoryCache Cache { get; }
    public MathCachedCalculator(ApplicationContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        Cache = cache;
    }

    public override async Task<CalculationMathExpressionResultDto>  CalculateMathExpressionAsync(string? expression)
    {
        if (Cache.TryGetValue(expression, out double expRes)) return new CalculationMathExpressionResultDto(expRes);
        var resultDto = await base.CalculateMathExpressionAsync(expression);
        if (!resultDto.IsSuccess) return resultDto;
        Cache.Set(expression, resultDto.Result);
        return resultDto;
    }
}