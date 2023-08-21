/// This module exposes use-cases of the Department component as an HTTP Web service using Giraffe
module Paidride.Department.Department

open Giraffe
open Microsoft.AspNetCore.Http
open Application.Department
open Thoth.Json.Giraffe
open Thoth.Json.Net


let updateDepartmentName (id: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let! name = ThothSerializer.ReadBody ctx Serialization.decodeName

        match name with
        | Ok name ->
            let dataAccess = ctx.GetService<IDepartmentDataAccess> ()
            dataAccess.UpdateNameForDepartment id name
            return! text "Department name updated succesfull" next ctx
        | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
    }

let getDepartmentHours (id: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IDepartmentDataAccess> ()
        let hours = dataAccess.GetHoursForDepartment id
        return! ThothSerializer.RespondJson hours Encode.int next ctx
    }

let getDepartmentOvertimeHours (id: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let dataAccess = ctx.GetService<IDepartmentDataAccess> ()
        let hours = dataAccess.GetOvertimeHoursForDepartment id
        return! ThothSerializer.RespondJson hours Encode.int next ctx
    }

let handlers : HttpHandler =
    choose [
        PATCH >=> routef "/department/%s/name" updateDepartmentName
        GET >=> choose [
            routef "/department/%s/hours" getDepartmentHours
            routef "/department/%s/overtime" getDepartmentOvertimeHours
        ]
        
    ]