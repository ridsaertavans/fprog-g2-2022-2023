///DepartmentDataAccess implementing interface from Application
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
                   Name = departmentName
                   Subdepartments = subdepartments }
        | None -> None

    buildDepartmentTree id

let departmentDataAccess (store : Store) = { new IDepartmentDataAccess with
    
    member this.RegisterHoursForEmployee(id : string) (name: string): unit = 
        InMemoryDatabase.update (id, (name)) store.departments
        |>ignore

    member this.getHoursForDepartment(id : string): int =
        let rec getHoursForDepartmentTree (department: Department): int =
            let departmentEmployeeHours =
                store.employees
                |> InMemoryDatabase.filter (fun (_, departmentId) -> departmentId = DepartmentId.toRawString department.Id)
                |> Seq.map (fun (employeeName, _) -> getHoursFromEmployee store employeeName)
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