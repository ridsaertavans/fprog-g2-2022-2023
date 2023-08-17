module Paidride.HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open Application.Employee
open Thoth.Json.Giraffe


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

let requestHandlers : HttpHandler =
    choose [ GET >=> route "/employee" >=> getEmployees
             GET >=> routef "/employee/%s" getEmployee
             GET >=> route "/hello" >=> text "Paidride is running"
             //GET >=> routef "/employee/%s" getEmployee
             //POST >=> routef "/employee/%s/hours" registerHours
             //GET >=> routef "/employee/%s/hours" totalHoursFor
             //GET >=> routef "/employee/%s/overtime" overtimeFor 
        ]