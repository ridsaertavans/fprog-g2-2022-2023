///Provides types Department / DepartmentName
module Model.Department

open System.Text.RegularExpressions

type DepartmentName = private | DepartmentName of string

let (|DepartmentName|) (DepartmentName departmentName) = departmentName

module DepartmentName =
    let make (rawName : string) =
        let m = Validation.matches (Regex("^(?!.*  )[A-Za-z]+(?: [A-Za-z]+)*$")) rawName //Regex only letters and spaces, and no more then 2 consecutive spaces
        if m then
            Ok (DepartmentName rawName)
        else 
            Error "Invalid departmentname"

type DepartmentId = DepartmentId of string

let (|DepartmentId|) (DepartmentId departmentId) = departmentId

module DepartmentId =
    let make (rawId : string) =
        let m = common.matches (Regex("[A-Z]{4}\d{2}")) rawId //Regex 4 capital letters followed by 2 numbers
        if m then
            Ok (DepartmentId rawId)
        else 
            Error "Invalid id"

    let toRawString (DepartmentId id) = id
/// A department has an Id (four uppercase letters followed by two digits),
/// a Name (only letters and spaces, but cannot contain two or more consecutive spaces),
/// and a list of subdepartments (which may be empty)



type Department = { Id: DepartmentId;
                    Name: string;
                    Subdepartments: List<Department> }