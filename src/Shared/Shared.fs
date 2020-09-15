namespace Shared

open System.Collections.Generic

type Gender = Male | Female

type Person = {
    name : string
    age : int
    gender : Gender
}

type Country = {
    countryCode : string
    name : string
    postalPattern : string
    phonePattern : string
    federalSalesTax : double
    provinceTerminology : string
}

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type IApi = { 
    getCountries : unit -> Async<IEnumerable<Country>>
}