/// Provides functionality (use-cases) for Departments
module Application.Department

/// Defines data access operations for Department functionality.
type IDepartmentDataAccess = 
    abstract RegisterHoursForEmployee : string -> string -> unit
    abstract getHoursForDepartment : string -> int

let registerHoursForEmployee (dataAccess : IDepartmentDataAccess) (id : string) (name : string) : unit = 
    dataAccess.RegisterHoursForEmployee id name

    //dataAccess.getHoursForDepartment id
    //|> List.map (fun hours -> (let (HourCount count) = hours.Amount in count))
    //|> List.sum