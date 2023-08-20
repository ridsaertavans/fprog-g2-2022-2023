module Paidride.HttpHandlers

open Giraffe
open Paidride.Department
open Paidride.Employee
open Paidride.Hours


let requestHandlers : HttpHandler =
    choose [ GET >=> route "/" >=> text "Paidride is running"
             // Dispatching and handling of Department component requests
             Department.handlers

             // Dispatching and handling of Employee component requests
             Employee.handlers

             // Dispatching and handling of Hours component requests
             Hours.handlers   
        ]