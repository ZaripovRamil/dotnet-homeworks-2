module Hw5.Calculator

[<Literal>] 
let Plus = "+"

[<Literal>] 
let Minus = "-"

[<Literal>] 
let Multiply = "*"

[<Literal>] 
let Divide = "/"

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline calculate value1 operation value2: 'a =
    match operation with
    | CalculatorOperation.Plus -> value1 + value2
    | CalculatorOperation.Minus -> value1 - value2
    | CalculatorOperation.Multiply -> value1 * value2
    | CalculatorOperation.Divide -> value1 / value2
let inline calculateTuple (value1, operation, value2): 'a =
    calculate value1 operation value2