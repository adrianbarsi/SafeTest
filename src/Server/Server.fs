module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn
open Shared

open Dapper.FSharp
open Dapper.FSharp.MSSQL

open System.Data
open System.Data.SqlClient 

Dapper.FSharp.OptionTypes.register()

let connectionString = "Server=.\\SQLEXPRESS;Database=Clubs;Trusted_Connection=True;"
let sqlConnection = new SqlConnection()
sqlConnection.ConnectionString <- connectionString
sqlConnection.Open()

let dbConnection = sqlConnection :> IDbConnection

let getCountries () =
    select {
        table "country"
    } |> dbConnection.SelectAsync<Country> |> Async.AwaitTask 

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