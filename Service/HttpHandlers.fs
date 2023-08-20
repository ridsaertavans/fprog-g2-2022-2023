module Paidride.HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open Application.Employee
open Application.Hours
open Application.Department
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

let registerHours (name: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let! hours = ThothSerializer.ReadBody ctx Serialization.decodeHours

        match hours with
        | Ok hours ->
            let dataAccess = ctx.GetService<IHoursDataAccess> ()
            dataAccess.RegisterHoursForEmployee name hours
            return! text "Hour registration succesfull" next ctx
        | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
    }

let updateDepartmentName (id: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let! name = ThothSerializer.ReadBody ctx Serialization.decodeName

        match name with
        | Ok name ->
            let dataAccess = ctx.GetService<IDepartmentDataAccess> ()
            dataAccess.RegisterHoursForEmployee id name
            return! text "Department name updated succesfull" next ctx
        | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
    }

let requestHandlers : HttpHandler =
    choose [ GET >=> route "/" >=> text "Paidride is running"
             GET >=> route "/employee" >=> getEmployees
             GET >=> routef "/employee/%s" getEmployee
             GET >=> routef "/employee/%s/hours" totalHoursFor
             GET >=> routef "/employee/%s/overtime" overtimeFor 
             POST >=> routef "/employee/%s/hours" registerHours
             PATCH >=> routef "/department/%s/name" updateDepartmentName
        ]