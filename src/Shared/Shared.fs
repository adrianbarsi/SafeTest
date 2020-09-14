namespace Shared

open System

type Gender = Male | Female

type Person = {
    name : string
    age : int
    gender : Gender
}

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IApi = { 
    getPerson : unit -> Async<Person>
}