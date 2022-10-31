using System.Diagnostics.CodeAnalysis;
using Hw8.Common;
using Hw8.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{ 
    private readonly IParser _parser;
    private readonly ICalculator _calculator;

    public CalculatorController(IParser parser, ICalculator calculator)
    {
        _parser = parser;
        _calculator = calculator;
    }
    
    [HttpGet]
    public IActionResult Calculate(string val1,
        string operation,
        string val2)
    {
        ParseOutput parsed;
        try
        {
            parsed = _parser.ParseCalcArguments(new ParseInput(val1,operation, val2));
        }
        catch (Exception e) when(e is ArgumentException or InvalidOperationException)
        {
            return BadRequest(e.Message);
        }

        try
        {
            return Ok(_calculator.Calculate(parsed));
        }
        catch(Exception e) when (e is InvalidOperationException)
        {
            return Ok(e.Message);
        }
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}