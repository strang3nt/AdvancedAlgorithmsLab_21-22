module Lab2.Main

open Lab1.Utils
open TspGraph
open Parsing
open MetricTsp
open Utils

open System.IO

[<EntryPoint>]
let main _ =
    let files = getFiles (Directory.GetCurrentDirectory() +/ "tsp_dataset")
    let tspGraphs = Array.map buildGraph files
    
    printfn "%9s\t%9s\t%9s" "Name" "Dimension" "Weight"
    printfn "%s" (String.replicate 60 "-")
    for tspGraph in tspGraphs do
        let TspGraph (name, comment, dimension, G) as _ = tspGraph
        metricTsp G 
        |> Seq.toList 
        |> getTotalWeightFromTree G 
        |> printfn "%9s\t%9i\t%9i" name dimension
    0
