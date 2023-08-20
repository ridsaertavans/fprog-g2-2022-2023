module Model.Department

open System
open System.Text.RegularExpressions
/// A department has an Id (four uppercase letters followed by two digits),
/// a Name (only letters and spaces, but cannot contain two or more consecutive spaces),
/// and a list of subdepartments (which may be empty)


type DepartmentName = private | DepartmentName of string

let (|DepartmentName|) (DepartmentName departmentName) = departmentName

module DepartmentName =
    let make (rawName : string) =
        let m = Regex.Match (rawName, "^(?!.*\s{2,})[A-Za-z]+(?:\s[A-Za-z]+)*$")
        if m.Success then
            Ok (DepartmentName rawName)
        else 
            Error "Wrong value"

type Department = { Id: string;
                    Name: string;
                    Subdepartments: List<Department> }