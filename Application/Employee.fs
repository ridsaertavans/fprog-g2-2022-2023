module Application.Employee

open Model.Employee

type IEmployeeDataAccess = 
    abstract RetrieveAllEmployee : unit -> List<Employee>
    abstract RetrieveEmployee: name: string -> Option<Employee>

let retrieveAllEmployee (dataAccess : IEmployeeDataAccess) : List<Employee> =
    dataAccess.RetrieveAllEmployee ()

let retrieveEmployee (dataAccess : IEmployeeDataAccess) (name : string) : Option<Employee> =
    dataAccess.RetrieveEmployee(name)
    