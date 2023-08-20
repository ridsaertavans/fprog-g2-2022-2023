module Paidride.Employee.Serialization

open Thoth.Json.Net
open Model.Employee

let encodeEmployee: Encoder<Employee> =
    fun employee ->
        Encode.object [ 
            "name", Encode.string employee.Name
            "department_id", Encode.string employee.DepartmentId 
        ]

let decodeEmployee: Decoder<Employee> =
    Decode.object (fun get ->
        { Name = get.Required.Field "name" Decode.string;
        DepartmentId = get.Required.Field "department_id" Decode.string })