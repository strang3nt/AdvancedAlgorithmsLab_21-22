module Lab2.Main

open Lab1.Utils
open Lab1.Graphs
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
    
    let algorithm alg iterations algName =
        printfn $"%s{algName} algorithm:"
        printfn "%9s\t%9s\t%9s\t%9s" "Name" "Dimension" "Weight" "Time"
        printfn "%s" (String.replicate 60 "-")
        let names, dimensions, weights, runTimes =
            tspGraphs |> Array.map (fun tspG ->
                    let TspGraph (name, _, dimension, G) as _ = tspG
                    let runTime = int (measureRunTime alg G iterations)
                    (alg G) |> Seq.toList 
                    |> getTotalWeightFromTree G
                    |> (fun weight ->
                        printfn $"%9s{name}\t%9i{dimension}\t%9i{weight}\t%9i{runTime}"
                        (name, dimension, weight, runTime))
                )
                |> Array.fold (fun (N, D, W, T) (n, d, w, t) -> N @ [n], D @ [d], W @ [w], T @ [t]) (List.Empty, List.Empty, List.Empty, List.Empty)
        printf "\n \n"
        0
            
//    algorithm metricTsp 100
    algorithm nearestNeighbourHeuristic 100 "Nearest Neighbour"