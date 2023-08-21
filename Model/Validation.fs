///Module with common function for models
module Validation

open System.Text.RegularExpressions

let matches (re : Regex) (s: string) = if re.IsMatch s then true else false