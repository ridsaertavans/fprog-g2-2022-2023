/// Module with validation function
module Validation

open System.Text.RegularExpressions

/// Validate that a given string matches the provided regular expression by returning a boolean if it matches
let matches (re : Regex) (s: string) = if re.IsMatch s then true else false