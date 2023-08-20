module Paidride.Hours.Serialization

open Thoth.Json.Net
open Model.Hours

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