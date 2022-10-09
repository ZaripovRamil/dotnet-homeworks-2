open System
open Hw4.Parser
open Hw4.Calculator

let readArgs = Console.ReadLine().Split()

readArgs |> parseCalcArguments |> calculateFromOptions|> printfn "%f"