/// DepartmentDataAccess implementing interface from Application
module DataAccess.Department

open Application.Department
open Model.Department
open Paidride.Database
open Paidride.Store


let getHoursFromEmployee (store: Store) (name: string) =
    store.hours
    |>InMemoryDatabase.filter (fun (n, _, _) -> n = name)
    |>Seq.map (fun (_, _, amount) -> amount)
    |>Seq.sum

let getOvertimeHoursFromEmployee (store: Store) (name: string) =
    store.hours
    |>InMemoryDatabase.filter (fun (n, _, _) -> n = name)
    |>Seq.map (fun (_, _, amount) -> max 0 amount - 8)
    |>Seq.sum

/// Data access operations of the Department component implemented using the simulated in-memory DB
let getDepartment (store: Store) (id: string): Option<Department> = 
    let rec buildDepartmentTree (currentId: string): Option<Department> =
        match InMemoryDatabase.lookup currentId store.departments with
        | Some (departmentId, departmentName, _) ->
            let subdepartments =
                InMemoryDatabase.filter (fun (_, _, parent) -> 
                    match parent with 
                    | Some parentId -> parentId = departmentId
                    | _ -> false
                    ) store.departments
                |> Seq.map (fun (subdepartmentId, _, _) -> subdepartmentId)
                |> Seq.choose buildDepartmentTree
                |> Seq.toList

            Some { Id = (DepartmentId departmentId)
                   Name = (DepartmentName departmentName)
                   Subdepartments = subdepartments }
        | None -> None

    buildDepartmentTree id

let departmentDataAccess (store : Store) = { new IDepartmentDataAccess with
    
    member this.UpdateNameForDepartment(id : string) (name: string): unit = 
        InMemoryDatabase.update (id, (name)) store.departments
        |>ignore

    member this.GetHoursForDepartment(id : string): int =
        let rec GetHoursForDepartmentTree (department: Department): int =
            let departmentEmployeeHours =
                store.employees
                |> InMemoryDatabase.filter (fun (_, departmentId) -> departmentId = DepartmentId.toRawString department.Id)
                |> Seq.map (fun (employeeName, _) -> getHoursFromEmployee store employeeName)
                |> Seq.sum

            let subdepartmentEmployeeHours = 
                department.Subdepartments
                |> Seq.map GetHoursForDepartmentTree
                |> Seq.sum

            departmentEmployeeHours + subdepartmentEmployeeHours
        
        let department = getDepartment store id

        match department with
        | None -> 0
        | Some department -> GetHoursForDepartmentTree department 

    member this.GetOvertimeHoursForDepartment(id : string): int =
        let rec getHoursForDepartmentTree (department: Department): int =
            let departmentEmployeeHours =
                store.employees
                |> InMemoryDatabase.filter (fun (_, departmentId) -> departmentId = DepartmentId.toRawString department.Id)
                |> Seq.map (fun (employeeName, _) -> getOvertimeHoursFromEmployee store employeeName)
                |> Seq.sum

            let subdepartmentEmployeeHours = 
                department.Subdepartments
                |> Seq.map getHoursForDepartmentTree
                |> Seq.sum

            departmentEmployeeHours + subdepartmentEmployeeHours
        
        let department = getDepartment store id

        match department with
        | None -> 0
        | Some department -> getHoursForDepartmentTree department 
}