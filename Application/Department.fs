module Application.Department

open Model.Department

type IDepartmentDataAccess = 
    abstract RegisterHoursForEmployee : string -> string -> unit

let registerHoursForEmployee (dataAccess : IDepartmentDataAccess) (id : string) (name : string) : unit = 
    dataAccess.RegisterHoursForEmployee id name