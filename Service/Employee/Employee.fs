/// This module exposes use-cases of the Employee component as an HTTP Web service using Giraffe
module Paidride.Employee.Employee

open Giraffe
open Thoth.Json.Giraffe
open Microsoft.AspNetCore.Http
open Application.Employee


let getEmployees (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IEmployeeDataAccess> ()
        let employees = Application.Employee.retrieveAllEmployee dataAccess
        return! ThothSerializer.RespondJsonSeq employees Serialization.encodeEmployee next ctx
    }

let getEmployee (name: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IEmployeeDataAccess> ()
        let employee = Application.Employee.retrieveEmployee dataAccess name
        
        match employee with
        | None -> return! RequestErrors.NOT_FOUND "Employee not found!" next ctx
        | Some e -> return! ThothSerializer.RespondJson e Serialization.encodeEmployee next ctx
    }

let handlers : HttpHandler =
    choose [
        GET >=> choose [
            route "/employee" >=> getEmployees
            routef "/employee/%s" getEmployee
        ]
    ]
    