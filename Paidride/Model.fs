namespace Paidride

open Thoth.Json.Net
open Thoth.Json.Giraffe
open System;

open Employee
open Hours

module Employee =
    let encode: Encoder<Employee> =
        fun employee ->
            Encode.object
                [ "name", Encode.string employee.Name
                  "department_id", Encode.string employee.DepartmentId ]

    let decode: Decoder<Employee> =
        Decode.object (fun get ->
            { Name = get.Required.Field "name" Decode.string
              DepartmentId = get.Required.Field "department_id" Decode.string })

module Hours =
    let encode: Encoder<Hours> =
        fun hours ->

            Encode.object
                [ "date", Encode.datetime hours.Date
                  "amount", Encode.int hours.Amount ]

    let decode: Decoder<Hours> =
        Decode.object (fun get ->
            { Date = get.Required.Field "date" Decode.datetime
              Amount = get.Required.Field "amount" Decode.int })



