/// Provides functionality (use-cases) for Departments
module Application.Department

open Model.Department

/// Defines data access operations for Department functionality.
type IDepartmentDataAccess = 
    //abstract GetDepartment : string -> Option<Department>
    abstract RegisterHoursForEmployee : string -> string -> unit
    abstract getHoursForDepartment : string -> int

let registerHoursForEmployee (dataAccess : IDepartmentDataAccess) (id : string) (name : string) : unit = 
    dataAccess.RegisterHoursForEmployee id name