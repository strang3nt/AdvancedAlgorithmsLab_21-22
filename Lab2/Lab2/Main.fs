module Lab2.Main

open Lab1.Utils
open TspGraph
open Parsing
open MetricTsp
open NearestNeighbourHeuristic
open Utils

open System.IO

[<EntryPoint>]
let main _ =
    let files = getFiles (Directory.GetCurrentDirectory() +/ "tsp_dataset")
    let tspGraphs = Array.map buildGraph files
    
    let algorithm alg iterations =
        printfn "%9s\t%9s\t%9s" "Name" "Dimension" "Weight"
        printfn "%s" (String.replicate 60 "-")
        for tspGraph in tspGraphs do
            let TspGraph (name, comment, dimension, G) as _ = tspGraph
            alg G 
            |> Seq.toList 
            |> getTotalWeightFromTree G 
            |> printfn "%9s\t%9i\t%9i" name dimension
//        let runTimes = Array.map (fun g -> measureRunTime alg g iterations) tspGraphs
//    algorithm metricTsp 100
    algorithm nearestNeighbourHeuristic 100
    
    0