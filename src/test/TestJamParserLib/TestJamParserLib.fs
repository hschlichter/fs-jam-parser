namespace FsUnit.Test

open NUnit.Framework
open FsUnit

[<TestFixture>]
type EqualTests() =
    let lib = new JamParserLib.JamParserLib()
    
    [<Test>]
    member test.Message() =
        lib.Message |> should equal "Hello JamParserLib"

    [<Test>]
    member test.ValueTypeShouldEqualEquivalentValue() =
        1 |> should equal 1

    [<Test>]
    member test.ValueTypeShouldFailToEqualNonequivalentValue() =
        shouldFail (fun () -> 1 |> should equal 2)
