module Paidride.HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open Application.Employee
open Application.Hours
open Thoth.Json.Net
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

let totalHoursFor (name: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IHoursDataAccess> ()
        let result = Application.Hours.getHoursForEmployee dataAccess name
        return! ThothSerializer.RespondJson result Encode.int next ctx
    }

let overtimeFor (name: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IHoursDataAccess> ()
        let result = Application.Hours.getOvertimeForEmployee dataAccess name
        return! ThothSerializer.RespondJson result Encode.int next ctx
    }

let requestHandlers : HttpHandler =
    choose [ GET >=> route "/employee" >=> getEmployees
             GET >=> routef "/employee/%s" getEmployee
             GET >=> routef "/employee/%s/hours" totalHoursFor
             GET >=> routef "/employee/%s/overtime" overtimeFor 
             GET >=> route "" >=> text "Paidride is running"
             //POST >=> routef "/employee/%s/hours" registerHours
        ]