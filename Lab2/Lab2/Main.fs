module Lab2.Main

open Lab1.Utils
open Parsing

open System.IO

[<EntryPoint>]
let main _ =
    let files = getFiles (Directory.GetCurrentDirectory() +/ "tsp_dataset")
    let tspGraphs = Array.map buildGraph files
    0
