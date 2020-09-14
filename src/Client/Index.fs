module Index

open Elmish
open Fable.Remoting.Client
open Shared
open Fable.React
open Fable.React.Props
open Fulma

type Model =
    { person : Person }

type Msg =
    | GetPerson
    | SetPerson of Person

let api =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder 
    |> Remoting.buildProxy<IApi>

let personToString (person : Person) =
    sprintf "Name: %s, Age: %i, Gender: %A" person.name person.age person.gender

let init(): Model * Cmd<Msg> =
    let model =
        { person = { name = ""; age = 0; gender = Male }
        }
    model, Cmd.none

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | GetPerson ->
        model, Cmd.OfAsync.perform api.getPerson () SetPerson
    | SetPerson person ->
        { model with person = person }, Cmd.none

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        button [ OnClick (fun _ -> dispatch GetPerson)] [ str "Get Person"]
        span [] [ str (personToString model.person)]
    ]