module Hw6.Controllers

open Giraffe
open Microsoft.AspNetCore.Http


let getInputFromRequest (request:HttpRequest)=
    let parseArg (str: string) =
        match request.Query.TryGetValue str with
        | true, arg -> Some arg
        | _ -> None
    match parseArg "value1" with
    |Some value1->
        match parseArg "operation" with
            |Some operation->
                match parseArg "value2" with
                |Some value2->Ok([|value1.ToString();operation.ToString();value2.ToString()|])
                | _ -> Error("Could not parse value2")
            | _ -> Error("Could not parse operation")
    | _ -> Error("Could not parse value1")
    
    
    
let getInputFromContext (context:HttpContext) =
    getInputFromRequest context.Request
    

let calculatorHandler:HttpHandler=
    fun next ctx->
        let result = MaybeBuilder.maybe{
            let! args = getInputFromContext ctx
            let! parsedArgs = Parser.parseCalcArguments args
            return parsedArgs|||>Calculator.calculate
        }
        match result with
        | Ok ok ->
            (setStatusCode 200 >=> text (ok.ToString())) next ctx            
        | Error error ->
            match error with
            |"DivideByZero"->
                (setStatusCode 200 >=> text "DivideByZero") next ctx
            | _ -> (setStatusCode 400 >=> text (error.ToString())) next ctx
    
    
   