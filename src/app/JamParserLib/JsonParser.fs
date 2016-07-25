namespace JamParserLib

open FParsec

type Json = JString of string
          | JNumber of float
          | JBool of bool
          | JNull
          | JList of Json list
          | JObject of Map<string, Json>

module Parser =
    let stringLiteral =
        let validChars = satisfy (fun c -> c <> '\\' && c <> '"')
        between (pstring "\"") (pstring "\"") (manyChars validChars)
        
    let jstring =
        stringLiteral |>> JString
    
    let jnumber =
        pfloat |>> JNumber
        
    let jbool =
        stringReturn "true" (JBool true) <|> stringReturn "false" (JBool false)
        
    let jnull =
        stringReturn "null" JNull

    let jvalue, jvalueRef = createParserForwardedToRef()

    let jlist =
        between (pstring "[" .>> spaces) (spaces >>. pstring "]") (sepBy jvalue (pstring "," .>> spaces) ) |>> JList

    let jobject =
        let keyValue = (stringLiteral .>> spaces) .>>. (pstring ":" .>> spaces >>. jvalue)
        between (pstring "{" .>> spaces) (spaces >>. pstring "}") (sepBy keyValue (pstring "," .>> spaces)) |>> (Map.ofList >> JObject)

    do jvalueRef := jobject <|> jlist <|> jstring <|> jnumber <|> jbool <|> jnull
    let json = jvalue
    
    let parse s = run json s
