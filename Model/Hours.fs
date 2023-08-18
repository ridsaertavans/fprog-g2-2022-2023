///Provides types Hours / HourCount
module Model.Hours

open System

type HourCount = private | HourCount of int

let (|HourCount|) (HourCount hourCount) = hourCount

module HourCount =
    let make (rawHours : int) =
        if rawHours >= 0 && rawHours <= 16 then
            Ok (HourCount rawHours)
        else 
            Error "Wrong value"

/// Hours registered on a specific date
///
/// Amount cannot be negative or larger than 16
/// Only the year, month and date of Date are used.
type Hours = { Date: DateTime; Amount: HourCount }
