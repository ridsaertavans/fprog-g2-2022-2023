module Paidride.Web

open Paidride
open Paidride.Database
open Paidride.Store
open Giraffe
open Thoth.Json.Net
open Thoth.Json.Giraffe


let getEmployees: HttpHandler =
    fun next ctx ->
        task {
            let store = ctx.GetService<Store>()

            let employees =
                InMemoryDatabase.all store.employees
                |> Seq.map (fun (name, departmentId) -> { Employee.Name = name
                                                          DepartmentId = departmentId } )

            return! ThothSerializer.RespondJsonSeq employees Employee.encode next ctx
        }

let getEmployee (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let store = ctx.GetService<Store>()

            let employee =
                InMemoryDatabase.lookup name store.employees


            match employee with
            | None -> return! RequestErrors.NOT_FOUND "Employee not found!" next ctx
            | Some (name, departmentId) ->
                return! ThothSerializer.RespondJson { Name = name; DepartmentId = departmentId } Employee.encode next ctx

        }

let registerHours (name: string) : HttpHandler =
    fun next ctx ->
        task {

            let! hours = ThothSerializer.ReadBody ctx Hours.decode

            match hours with
            | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
            | Ok { Date = d ; Amount = a } ->
                let store = ctx.GetService<Store>()

                InMemoryDatabase.insert (name, d) (name, d, a) store.hours
                |> ignore


                return! text "OK" next ctx
        }

let totalHoursFor (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let store = ctx.GetService<Store>()

            let total =
                InMemoryDatabase.filter (fun (n, _, _) -> n = name) store.hours
                |> Seq.map (fun (_, _, a) -> a)
                |> Seq.sum

            return! ThothSerializer.RespondJson total Encode.int next ctx
        }


let overtimeFor (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let store = ctx.GetService<Store>()

            let total =
                InMemoryDatabase.filter (fun (n, _, _) -> n = name) store.hours
                |> Seq.map (fun (_, _, a) -> max 0 (a - 8))
                |> Seq.sum

            return! ThothSerializer.RespondJson total Encode.int next ctx
        }



let routes: HttpHandler =
    choose [ GET >=> route "/employee" >=> getEmployees
             GET >=> routef "/employee/%s" getEmployee
             POST >=> routef "/employee/%s/hours" registerHours
             GET >=> routef "/employee/%s/hours" totalHoursFor
             GET >=> routef "/employee/%s/overtime" overtimeFor ]
