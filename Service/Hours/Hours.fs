/// This module exposes use-cases of the Hours component as an HTTP Web service using Giraffe
module Paidride.Hours.Hours

open Giraffe
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Microsoft.AspNetCore.Http
open Application.Hours


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


let handlers : HttpHandler =
    choose [
        GET >=> choose [
            routef "/employee/%s/hours" totalHoursFor
            routef "/employee/%s/overtime" overtimeFor 
        ]
        POST >=> routef "/employee/%s/hours" registerHours
    ]
    