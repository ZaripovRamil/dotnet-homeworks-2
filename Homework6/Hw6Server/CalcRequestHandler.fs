module Hw6.Controllers

open Giraffe
open Microsoft.AspNetCore.Http


let getInputFromRequest (request:HttpRequest)=
    let parseArg (str: string) =
        match request.Query.TryGetValue str with
        | true, arg -> Ok (arg.ToString())
        | _ -> Error $"Could not parse {str}"
    
    MaybeBuilder.maybe{
         let! parsedVal1 = parseArg "value1"
         let! parsedOp= parseArg "operation"
         let! parsedVal2 = parseArg "value2"
         return [|parsedVal1;parsedOp;parsedVal2|]   
    }
    
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