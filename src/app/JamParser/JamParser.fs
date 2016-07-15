module JamParser

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    let lib = new JamParserLib.JamParserLib()
    printfn "%s" lib.Message
    
    0 // return an integer exit code
