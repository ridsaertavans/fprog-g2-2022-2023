namespace Paidride

open Thoth.Json.Net
open Thoth.Json.Giraffe
open System;

// Name is the employees name. Department is the department's identifier
// which always consists of four uppercase letters followed by two digits.
type Employee =
    { Name: string; DepartmentId: string }


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

/// Hours registered on a specific date
///
/// Amount cannot be negative or larger than 16
/// Only the year, month and date of Date are used.
type Hours = { Date: DateTime; Amount: int }

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



