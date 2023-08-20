module Serialization

open Thoth.Json.Net
open Model.Department

let decodeName: Decoder<string> =
    Decode.string
    |> Decode.andThen (fun s ->
        match DepartmentName.make s with
        | Ok departmentName -> Decode.succeed s
        | Error error -> Decode.fail error
    )