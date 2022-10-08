open System
open Hw4.Parser
open Hw4.Calculator
open Hw4

let readArgs = Console.ReadLine().Split()
let Calculate (calcOptions:CalcOptions):float =
    calculate calcOptions.arg1 calcOptions.operation calcOptions.arg2


Console.Write (readArgs |> parseCalcArguments |> Calculate)

