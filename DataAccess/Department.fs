///DepartmentDataAccess implementing interface from Application
module DataAccess.Department

open Application.Department
open Model.Department
open Paidride.Database
open Paidride.Store

let getHoursFromEmployee (store : Store) (name : string) =
    store.hours
    |>InMemoryDatabase.filter (fun (n, _, _) -> n = name)
    |>Seq.map (fun (_, _, amount) -> amount)
    |>Seq.sum

let departmentDataAccess (store : Store) = { new IDepartmentDataAccess with
    
    member this.RegisterHoursForEmployee(id : string) (name: string): unit = 
        InMemoryDatabase.update (id, (name)) store.departments
        |>ignore

    member this.getHoursForDepartment(id : string): int =
        let rec retreiveHoursForDepartmentTree (department: Department): int =
            let departmentEmployeeHours =
                store.employees
                |> InMemoryDatabase.filter (fun (_, departmentId) -> (DepartmentId departmentId) = departmentId.Id)
                |> Seq.map (fun (employeeName, _) -> getHoursFromEmployee store employeeName)
                |> Seq.sum

            let subdepartmentEmployeeHours = 
                department.Subdepartments
                |> Seq.map retreiveHoursForDepartmentTree
                |> Seq.sum

            departmentEmployeeHours + subdepartmentEmployeeHours
        
        this.getHoursForDepartment id
    }