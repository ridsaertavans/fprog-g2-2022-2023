module DataAccess.Hours

open Model.Hours
open Application.Hours
open System
open Paidride.Database
open Paidride.Store

//let hoursDataAccess (store : Store) = new IHoursDataAccess with
//    member this.RegisterHoursForEmployee(name: string) (hours: Hours): unit =
//    InMemoryDatabase.insert (name, hours.Date) (name, hours.Date) (let (HourCount count) = hours.Count in count)

//    member this.getHoursForEmployee(name : string): List<Hours> = 
//       InMemoryDatabase.filter ()