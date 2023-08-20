///HoursDataAccess implementing interface from Application
module DataAccess.Department

open Model.Department
open Application.Department
open Paidride.Database
open Paidride.Store

let departmentDataAccess (store : Store) = { new IDepartmentDataAccess with
    
    member this.RegisterHoursForEmployee(id : string) (name: string): unit = 
        InMemoryDatabase.update (id, (name)) store.departments
        |>ignore
    }