module Hw6Client
open System
open System.Net.Http

let getOperationName operation =
    match operation with
    | "+" -> "Plus"
    | "-" -> "Minus"
    | "*" -> "Multiply"
    | "/" -> "Divide"
    | _ -> operation

let handleQueryAsync(client : HttpClient) (url : string) =
    async {
        let! response = Async.AwaitTask (client.GetAsync url)
        let! res = Async.AwaitTask (response.Content.ReadAsStringAsync())
        return res
    }

let rec handleUserRequests client=
    Console.WriteLine("Insert your calculation request")
    let args = Console.ReadLine().Split(" ",StringSplitOptions.RemoveEmptyEntries )
    match args.Length with
        | 3 ->
            let url = $"http://localhost:5000/calculate?value1={args[0]}&operation={getOperationName args[1]}&value2={args[2]}";
            printfn $"result: {handleQueryAsync client url |> Async.RunSynchronously}"
            handleUserRequests client
        | 0 -> 0
        | _ ->
            printfn "Incorrect input format"
            handleUserRequests client
        
    
    
[<EntryPoint>]
let main _ =
    let client = new HttpClient(new HttpClientHandler())
    handleUserRequests client
    
    