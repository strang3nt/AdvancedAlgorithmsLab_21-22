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
    let tspGraphs = 
        Array.map buildGraph files
        |> Array.sortBy (fun (TspGraph (_, _, dimension, _, _)) -> dimension)
    
    printfn "%9s\t%9s\t%9s\t%9s\t%9s" "Name" "Dimension" "TSP" "TSP*" "Error"
    printfn "%s" (String.replicate 80 "-")
    for tspGraph in tspGraphs do
        let TspGraph (name, comment, dimension, optimalSolution, G) as _ = tspGraph
        let weight =
            metricTsp G
            |> Seq.toList
            |> getTotalWeightFromTree G 
        printfn $"%9s{name}\t%9i{dimension}\t%9i{weight}\t%9i{optimalSolution}\t%9f{(float weight-float optimalSolution)/(float optimalSolution)}"
    0
