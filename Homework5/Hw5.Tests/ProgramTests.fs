module Hw5.Tests.ProgramTests

open System
open Hw5
open Program
open Xunit        

[<Theory>]
[<InlineData("1", "+","1")>]
[<InlineData("1","-","1")>]
[<InlineData("1","*", "1")>]
[<InlineData("1.0","/","1")>]
[<InlineData("a","+","1")>]   
[<InlineData("1.0",".","1")>]        
[<InlineData("1","/","0")>]        
let ``program never fails on 3 args`` (a1, a2, a3) =
    let args = [|a1;a2;a3|]
    let acceptableResults = [|0;1;2;3|]
    Array.contains (main args) acceptableResults
    |>Assert.True
    
[<Fact>]
let ``program never fails on null`` =
    (0, main null)
    |>Assert.Equal