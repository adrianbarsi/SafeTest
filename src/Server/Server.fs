module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn
open Shared

open Dapper.FSharp
open Dapper.FSharp.MSSQL

open System.Data.SqlClient 

OptionTypes.register()

let connectionString = "Server=.\\SQLEXPRESS;Database=Clubs2;Trusted_Connection=True;"
let sqlConnection = new SqlConnection(connectionString)
sqlConnection.Open()

let getCountries () =
    select {
        table "country"
    } |> sqlConnection.SelectAsync<Country> |> Async.AwaitTask 

let api : IApi = {
    getCountries = getCountries
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