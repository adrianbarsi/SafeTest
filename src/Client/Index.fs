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
    countries : IEnumerable<Country>
}

type Msg =
    | GetCountries
    | SetCountries of IEnumerable<Country>

let countriesToString (countries : IEnumerable<Country>) = sprintf "%A" countries

let init(): Model * Cmd<Msg> =
    let model = {
        countries = []
    }
    model, Cmd.none

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | GetCountries ->
        model, Cmd.OfAsync.perform api.getCountries () SetCountries

    | SetCountries countries ->
        { model with countries = countries }, Cmd.none

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        button [ OnClick (fun _ -> dispatch GetCountries) ] [ str "Get Countries" ]
        span [] [ str (countriesToString model.countries) ]
    ]