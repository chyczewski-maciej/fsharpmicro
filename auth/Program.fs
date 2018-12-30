open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe

[<CLIMutable>]
type User = { Login : string; Password : string }
let admin = { Login = "admin"; Password="admin123" }

let authenticate user = 
    let authenticated = (user = admin)
    if (authenticated) then printf "User %s logged in succesfully" user.Login
    authenticated
let webApp =
    choose [
        POST >=> route "/login" >=> bindJson<User>(fun user -> 
            let statusCode = if(authenticate user) then 200 else 401   
            setStatusCode statusCode)
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