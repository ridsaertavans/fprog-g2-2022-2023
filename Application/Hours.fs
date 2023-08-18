/// Provides functionality (use-cases) for getting hours of employees
module Application.Hours

open Model.Hours

type IHoursDataAccess = 
    //abstract RegisterHoursForEmployee : string -> Hours -> unit
    abstract GetHoursForEmployee : string -> List<Hours>
    //abstract GetOvertimeForEmployee : string -> List<Hours>

//let registerHoursForEmployee (dataAccess : IHoursDataAccess) (name : string) (hours : Hours) : unit = 
//    dataAccess.RegisterHoursForEmployee name hours

let getHoursForEmployee (dataAccess : IHoursDataAccess) (name : string) : int =
    dataAccess.GetHoursForEmployee name
    |> List.map (fun hours -> (let (HourCount count) = hours.Amount in count))
    |> List.sum

let getOvertimeForEmployee (dataAccess : IHoursDataAccess) (name : string) : int =
    dataAccess.GetHoursForEmployee name
    |> List.map (fun hours -> max 0 (let (HourCount count) = hours.Amount in count) - 8)
    |> List.sum
