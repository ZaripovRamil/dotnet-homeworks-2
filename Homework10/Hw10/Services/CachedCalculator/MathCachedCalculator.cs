using Hw10.DbModels;
using Hw10.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculator : MathCalculator.MathCalculator
{
    private readonly ApplicationContext _dbContext;
    private readonly IMemoryCache _cache;
    public MathCachedCalculator(ApplicationContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public override async Task<CalculationMathExpressionResultDto>  CalculateMathExpressionAsync(string? expression)
    {
        if (_cache.TryGetValue(expression, out double expRes)) return new CalculationMathExpressionResultDto(expRes);
        var resultDto = await base.CalculateMathExpressionAsync(expression);
        if (!resultDto.IsSuccess) return resultDto;
        _cache.Set(expression, resultDto.Result);
        return resultDto;
    }
}