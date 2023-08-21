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
            Error "Wrong value"

/// A department has an Id (four uppercase letters followed by two digits),
/// a Name (only letters and spaces, but cannot contain two or more consecutive spaces),
/// and a list of subdepartments (which may be empty)

type Department = { Id: string;
                    Name: string;
                    Subdepartments: List<Department> }