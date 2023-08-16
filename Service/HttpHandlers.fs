module Paidride.HttpHandlers

open Giraffe

let requestHandlers : HttpHandler =
    choose [
        // A basic example of handling a GET request
        route "/hello" >=> GET >=> text "Paidride is running"
        // See the following example on how to process a POST request, decode JSON and return different responses
    ]