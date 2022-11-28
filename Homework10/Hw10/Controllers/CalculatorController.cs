using System.Diagnostics.CodeAnalysis;
using Hw10.Dto;
using Hw10.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hw10.Controllers;

public class CalculatorController : Controller
{
    private readonly IMathCalculator _mathCalculator;

    public CalculatorController(IMathCalculator mathCalculator)
    {
        _mathCalculator = mathCalculator;
    }
        
    [HttpGet]
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult<CalculationMathExpressionResultDto>> CalculateMathExpression(string expression)
    {
        var result = await _mathCalculator.CalculateMathExpressionAsync(expression);
        return Json(result);
    }
}