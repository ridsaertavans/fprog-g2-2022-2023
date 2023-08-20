﻿/// Provides functionality (use-cases) for Departments
module Application.Department

/// Defines data access operations for Department functionality.
type IDepartmentDataAccess = 
    abstract RegisterHoursForEmployee : string -> string -> unit

let registerHoursForEmployee (dataAccess : IDepartmentDataAccess) (id : string) (name : string) : unit = 
    dataAccess.RegisterHoursForEmployee id name