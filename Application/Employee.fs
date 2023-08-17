module Application.Employee

open Model.Employee

type IEmployeeDataAccess = 
    abstract RetreiveAllEmployee : unit -> List<Employee>

let retreiveAllEmployee (dataAccess : IEmployeeDataAccess) : List<Employee> =
    dataAccess.RetreiveAllEmployee ()
    