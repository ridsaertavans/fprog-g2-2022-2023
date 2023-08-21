module Paidride.Department.Department

open Giraffe
open Microsoft.AspNetCore.Http
open Application.Department
open Model.Department
open Thoth.Json.Giraffe


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

let getDepartmentHours (id: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        match DepartmentId.make id with
        | Ok id ->
            let dataAccess = ctx.GetService<IDepartmentDataAccess> ()
            //dataAccess.RegisterHoursForEmployee id
            return! text "Success getdepartmenthours" next ctx
        | Error error -> return! RequestErrors.BAD_REQUEST error next ctx
    }


let handlers : HttpHandler =
    choose [
        PATCH >=> routef "/department/%s/name" updateDepartmentName
        GET >=> routef "/department/%s/hours" getDepartmentHours
    ]