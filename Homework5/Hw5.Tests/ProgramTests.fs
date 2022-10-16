module Hw5.Tests.ProgramTests

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
let test3args(a1, a2, a3) =
    let args = [|a1;a2;a3|]
    Assert.Equal(0, main args)
    
[<Fact>]
let testNull =
    Assert.Equal(0, main null)