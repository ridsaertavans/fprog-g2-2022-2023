module Application.Hours

open Model.Hours

type IHoursDataAccess = 
    abstract RegisterHoursForEmployee : string -> Hours -> unit
    abstract GetHoursForEmployee : string -> List<Hours>

let registerHoursForEmployee (dataAccess : IHoursDataAccess) (name : string) (hours : Hours) : unit = 
    dataAccess.RegisterHoursForEmployee name hours

//let getHoursForEmployee (dataAccess : IHoursDataAccess) (name : string) : int =
//    dataAccess.GetHoursForEmployee name
//    |> List.map (fun hours -> max 0 ((let (HourCount count) = hours.Count) - 8))
//    |> List.sum
