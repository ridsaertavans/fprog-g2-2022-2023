module Paidride.Store

open System
open Paidride.Database

/// Here a store is created that contains the following tables with the following attributes
///
/// employees (primary key is name)
/// - name
/// - department id
///
/// hours (primary key is compound: employee name and date)
/// - employee name (foreign key to employees)
/// - date
/// - amount (int)
///
/// departments
/// - id
/// - name
/// - super (nullable department id of which this is a subdepartment)
///
/// departmentDepartments
type Store() =
    member val employees: InMemoryDatabase<string, string * string> =
        [ "Jeroen", "AAAA13"
          "Bob", "BBBB37"
          "Ernst", "CCCC42" ]
        |> Seq.map (fun t -> fst t, t)
        |> InMemoryDatabase.ofSeq

    member val hours: InMemoryDatabase<string * DateTime, string * DateTime * int> =
        [ "Jeroen", DateTime(2023, 5, 1), 8
          "Jeroen", DateTime(2023, 5, 2), 10
          "Jeroen", DateTime(2023, 5, 3), 9
          "Ernst", DateTime(2023, 5, 1), 10
          "Ernst", DateTime(2023, 5, 2), 9
          "Bob", DateTime(2023, 5, 8), 10
          "Bob", DateTime(2023, 5, 2), 8 ]
        |> Seq.map (fun (n, v, p) -> (n, v), (n, v, p))
        |> InMemoryDatabase.ofSeq

    member val departments : InMemoryDatabase<string, string * string * Option<string>> =
        [ "AAAA13", "Lost and Found", Some "CCCC42"
          "BBBB37", "Customer Service", Some "CCCC42"
          "CCCC42", "Human Resources", None ]
        |> Seq.map (fun (x, y, z) -> x, (x, y, z))
        |> InMemoryDatabase.ofSeq
