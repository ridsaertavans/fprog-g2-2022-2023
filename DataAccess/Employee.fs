module DataAccess.Employee

open Model.Employee
open Application.Employee
open Paidride.Database
open Paidride.Store

let employeeAccess (store : Store) = { new IEmployeeDataAccess with
    member this.RetreiveAllEmployee (): List<Employee> =
        InMemoryDatabase.all store.employees
        |> Seq.map (fun (name, departmentId) -> { Employee.Name = name; Employee.DepartmentId = departmentId } )
        |> Seq.toList
}
