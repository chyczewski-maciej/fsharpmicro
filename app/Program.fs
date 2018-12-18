open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open System.Net.Http
open Newtonsoft.Json

[<CLIMutable>]
type User = { Login : string; Password : string }

let httpClient = new HttpClient()
let login (user: User) = 
    task { 
        let json = JsonConvert.SerializeObject(user)
        let! httpResult = httpClient.PostAsync("http://auth:5000/login", new StringContent(json));
        return int httpResult.StatusCode
    }
let loginHandler : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let! user = ctx.BindJsonAsync<User>()
            let! httpResponse = login user
            if(httpResponse = 401) then
                ctx.SetStatusCode 401
            return! next ctx
        }

let webApp =
    choose [
        POST >=> route "/login" >=> loginHandler
    ]

let configureApp (app : IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    // Add Giraffe dependencies
    services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    WebHostBuilder()
        .UseKestrel()
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .Build()
        .Run()
    0