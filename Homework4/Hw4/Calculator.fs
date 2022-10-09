module Hw4.Calculator

open System
open Hw4.CalculatorOperation
open CalcOptions


let calculate (value1: float) operation value2 =
    match operation with
    | CalculatorOperation.Plus -> value1 + value2
    | CalculatorOperation.Minus -> value1 - value2
    | CalculatorOperation.Multiply -> value1 * value2
    | CalculatorOperation.Divide -> value1 / value2
    | _ -> ArgumentOutOfRangeException() |> raise
    
let сalculate calcOptions =
    calculate calcOptions.arg1 calcOptions.operation calcOptions.arg2