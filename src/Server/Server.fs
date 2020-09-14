module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn
open Shared

open FSharp.Data.Sql

type Sql = SqlDataProvider<Common.DatabaseProviderTypes.MSSQLSERVER, "Server=.\\SQLEXPRESS;Database=Clubs;Trusted_Connection=True;">

let context = Sql.GetDataContext();

let person : Person = { name = "X"; age = 21; gender = Male }

let getPerson () =
    async {
        return person
    }

let api : IApi = {
    getPerson = getPerson
}

let webApp =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue api
    |> Remoting.buildHttpHandler

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
    }
run app