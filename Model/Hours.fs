module Hours
open System

/// Hours registered on a specific date
///
/// Amount cannot be negative or larger than 16
/// Only the year, month and date of Date are used.
type Hours = { Date: DateTime; Amount: int }