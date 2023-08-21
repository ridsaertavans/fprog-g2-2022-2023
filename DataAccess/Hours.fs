/// HoursDataAccess implementing interface from Application
module DataAccess.Hours

open Model.Hours
open Application.Hours
open Paidride.Database
open Paidride.Store

/// Data access operations of the Hours component implemented using the simulated in-memory DB
let hoursDataAccess (store: Store) = { new IHoursDataAccess with
    
    member this.RegisterHoursForEmployee(name : string) (hours: Hours): unit = 
        InMemoryDatabase.insert (name, hours.Date) (name, hours.Date, (let (HourCount amount) = hours.Amount in amount)) store.hours
        |>ignore

    member this.GetHoursForEmployee(name : string): List<Hours> = 
       InMemoryDatabase.filter (fun (n, _, _) -> n = name) store.hours
       |>Seq.map (fun (_, date, amount) -> match HourCount.make amount with
                                            | Ok result -> Some { Hours.Date = date; Hours.Amount = result}
                                            | Error _ -> None )
       |>Seq.choose id
       |>Seq.toList
}
