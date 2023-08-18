module DataAccess.Hours

open Model.Hours
open Application.Hours
open Paidride.Database
open Paidride.Store

let hoursDataAccess (store : Store) = { new IHoursDataAccess with

    member this.GetHoursForEmployee(name : string): List<Hours> = 
       InMemoryDatabase.filter (fun (n, _, _) -> n = name) store.hours
       |>Seq.map (fun (_, date, amount) -> match HourCount.make amount with
                                            | Ok result -> Some { Hours.Date = date; Hours.Amount = result}
                                            | Error _ -> None )
       |>Seq.choose id
       |>Seq.toList
}
