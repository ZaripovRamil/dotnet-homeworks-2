using System.Diagnostics.CodeAnalysis;
using Hw9.Dto;
using Hw9.Services.MathCalculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw9.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly IMathCalculator _mathCalculator;

        public CalculatorController(IMathCalculator mathCalculator)
        {
            _mathCalculator = mathCalculator;
        }
        
        [HttpGet]
        [ExcludeFromCodeCoverage]
        public IActionResult Calculator()
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
}