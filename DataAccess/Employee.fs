﻿/// EmployeeDataAccess implementing interface from Application
module DataAccess.Employee

open Model.Employee
open Application.Employee
open Paidride.Database
open Paidride.Store

/// Data access operations of the Employee component implemented using the simulated in-memory DB
let employeeAccess (store: Store) = { new IEmployeeDataAccess with
    member this.RetrieveAllEmployee (): List<Employee> =
        InMemoryDatabase.all store.employees
        |> Seq.map (fun (name, departmentId) -> { Employee.Name = name; Employee.DepartmentId = departmentId } )
        |> Seq.toList

    member this.RetrieveEmployee (name : string): Option<Employee> = 
        let employee = InMemoryDatabase.lookup name store.employees
        
        match employee with
        | None -> None
        | Some (name, departmentId) -> Some { Name = name; DepartmentId = departmentId }
}
