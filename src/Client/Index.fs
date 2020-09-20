module Index

open Elmish
open Fable.Remoting.Client
open Shared
open Fable.React
open Fable.React.Props
open Fulma

open System.Collections.Generic

let api =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder 
    |> Remoting.buildProxy<IApi>

type Model = {
    countries : Country list
}

type Msg =
    | GetCountries
    | SetCountries of IEnumerable<Country>

let init(): Model * Cmd<Msg> =
    let model = {
        countries = []
    }
    model, Cmd.none

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =    
    match msg with
    | GetCountries ->
        model, Cmd.OfAsync.perform api.getCountries () SetCountries

    | SetCountries (newCountries : IEnumerable<Country>) ->
        { model with countries = Seq.toList newCountries }, Cmd.none

let countryToRow (country : Country) : ReactElement =
    tr [] [
        td [] [ str country.countryCode ]
        td [] [ str country.name ]
        td [] [ str country.postalPattern ]
        td [] [ str country.phonePattern ]
        td [] [ str (sprintf "%f" country.federalSalesTax) ]
        td [] [
            Button.button [] [ Icon.icon [] [ i [ ClassName "fas fa-edit" ] [] ] ]
            br []
            Button.button [] [ Icon.icon [] [ i [ ClassName "fas fa-info" ] [] ] ]
            br []
            Button.button [] [ Icon.icon [] [ i [ ClassName "fas fa-trash" ] [] ] ]
        ]
    ]

let countriesToTable (countries : Country list) : ReactElement =
    let headerRow : ReactElement = 
        tr [] [
            th [] [ str "CountryCode" ]
            th [] [ str "Name" ]
            th [] [ str "PostalPattern" ]
            th [] [ str "PhonePattern" ]
            th [] [ str "FederalSalesTax" ]
            th [] [ str "Actions" ]
        ]

    let countryRows : ReactElement list = List.map countryToRow countries

    Table.table [ Table.TableOption.CustomClass "countryTable" ] (headerRow :: countryRows)
    
let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        button [ OnClick (fun _ -> dispatch GetCountries) ] [ str "Get Countries" ]
        match model.countries with
        | [] -> span [] [ str "No Countries" ]
        | countries-> countriesToTable countries
    ]