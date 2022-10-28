module Hw6Client

open System
open System.Net.Http
open System.Threading.Tasks

let getOperationName operation =
    match operation with
    | "+" -> Some "Plus"
    | "-" -> Some "Minus"
    | "*" -> Some "Multiply"
    | "/" -> Some "Divide"
    | _ -> None

let handleQueryAsync (client: HttpClient) (url: string) =
    async {
        let! response = Async.AwaitTask(client.GetAsync url)
        let! res = Async.AwaitTask(response.Content.ReadAsStringAsync())
        return res
    }

let rec handleUserRequestsAsync client =
    async {
        Console.WriteLine("Insert your calculation request")

        let args =
            Console
                .ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)

        match args.Length with
        | 3 ->
            let op =
                match getOperationName args[1] with
                | Some op -> op
                | None -> "None"

            let url =
                $"http://localhost:5000/calculate?value1={args[0]}&operation={op}&value2={args[2]}"

            let! result = handleQueryAsync client url
            printfn $"result: {result}"

            return! (handleUserRequestsAsync client)
        | 0 -> return ()
        | _ ->
            printfn "Incorrect input format"
            return! (handleUserRequestsAsync client)
    }

[<EntryPoint>]
let main _ =
    use client = new HttpClient()
    Async.RunSynchronously(handleUserRequestsAsync client)
    0
