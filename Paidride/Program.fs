/// ====================================
/// ==== DO NOT CHANGE THIS FILE    ====
/// ====                            ====
/// ==== You do not have to alter   ====
/// ==== this file for the          ====
/// ==== assessment                 ====
/// ====================================
///                 ^
///EDITING ALLOWED, BRIGHTSPACE ANNOUNCEMENTS
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Paidride
open Paidride.Store
open Application.Employee
open Application.Hours
open Application.Department

let configureApp (app: IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe HttpHandlers.requestHandlers
    //app.UseGiraffe Web.routes

let store = Store();

let configureServices (services: IServiceCollection) =
    // Add Giraffe dependencies
    services
        .AddGiraffe()
        .AddSingleton<Store>(Store())
        .AddSingleton<IEmployeeDataAccess>(DataAccess.Employee.employeeAccess store)
        .AddSingleton<IHoursDataAccess>(DataAccess.Hours.hoursDataAccess store)
        .AddSingleton<IDepartmentDataAccess>(DataAccess.Department.departmentDataAccess store)
        .AddSingleton<Json.ISerializer>(ThothSerializer(skipNullField = false, caseStrategy = CaseStrategy.CamelCase))
    |> ignore

[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(configureApp)
                .ConfigureServices(configureServices)
            |> ignore)
        .Build()
        .Run()

    0
