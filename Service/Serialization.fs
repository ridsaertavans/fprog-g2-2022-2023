module Serialization

open Thoth.Json.Net
open Model.Employee
open Model.Hours
open Model.Department
open System.Text.Json
open System.Collections.Generic

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

//let encodeHours: Encoder<Hours> =
//    fun hours ->
//        Encode.object [ 
//            "date", Encode.datetime hours.Date
//            "amount", (Encode.int (fun (HourCount amount) -> Encode.string amount) hours.Amount)
//        ]

let amountDecoder: Decoder<HourCount> = 
    Decode.int
    |> Decode.andThen (fun a ->
        match HourCount.make a with
        | Ok hourCount -> Decode.succeed hourCount
        | Error error -> Decode.fail error
    )

let decodeHours: Decoder<Hours> =
    Decode.object (fun get ->
        { Date = get.Required.Field "date" Decode.datetime
          Amount = get.Required.Field "amount" amountDecoder })


let decodeName: Decoder<string> =
    Decode.string
    |> Decode.andThen (fun s ->
        match DepartmentName.make s with
        | Ok departmentName -> Decode.succeed s
        | Error error -> Decode.fail error
    )
