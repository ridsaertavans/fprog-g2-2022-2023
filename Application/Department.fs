/// Provides functionality (use-cases) for Departments
module Application.Department

/// Defines data access operations for Department functionality.
type IDepartmentDataAccess = 
    abstract UpdateNameForDepartment : string -> string -> unit
    abstract GetHoursForDepartment : string -> int
    abstract GetOvertimeHoursForDepartment : string -> int

let registerHoursForEmployee (dataAccess : IDepartmentDataAccess) (id : string) (name : string) : unit = 
    dataAccess.UpdateNameForDepartment id name