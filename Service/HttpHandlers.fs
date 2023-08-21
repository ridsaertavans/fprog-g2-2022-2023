/// Dispatching of HTTP requests for all URLs across all components.
module Paidride.HttpHandlers

open Giraffe
open Paidride.Department
open Paidride.Employee
open Paidride.Hours

/// Composes all dispatching of HTTP requests into a single Giraffe HTTP handler. This handler is the used to "run"
/// Giraffe in the main function of the back-end
let requestHandlers : HttpHandler =
    choose [ GET >=> route "/" >=> text "Paidride is running"
             // Dispatching and handling of Department component requests
             Department.handlers

             // Dispatching and handling of Employee component requests
             Employee.handlers

             // Dispatching and handling of Hours component requests
             Hours.handlers   
        ]