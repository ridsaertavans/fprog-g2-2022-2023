module Paidride.HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open Application.Employee
open Thoth.Json.Giraffe


let getEmployees (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IEmployeeDataAccess> ()
        let employees = Application.Employee.retreiveAllEmployee dataAccess
        return! ThothSerializer.RespondJsonSeq employees Serialization.encodeEmployee next ctx
    }

let requestHandlers : HttpHandler =
    choose [ GET >=> route "/employee" >=> getEmployees
             GET >=> route "/hello" >=> text "Paidride is running"
             //GET >=> routef "/employee/%s" getEmployee
             //POST >=> routef "/employee/%s/hours" registerHours
             //GET >=> routef "/employee/%s/hours" totalHoursFor
             //GET >=> routef "/employee/%s/overtime" overtimeFor 
        ]