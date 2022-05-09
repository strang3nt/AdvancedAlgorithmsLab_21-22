module Lab2.Main

open Lab1.Utils
open Lab2.Utils
open TspGraph
open Parsing
open MetricTsp
open NearestNeighbourHeuristic
open Utils

open System.IO

[<EntryPoint>]
let main _ =
    let files = getFiles (Directory.GetCurrentDirectory() +/ "tsp_dataset")
    let tspGraphs = 
        Array.map buildGraph files
        |> Array.sortBy (fun (TspGraph (_, _, dimension, _, _)) -> dimension)
    
    let algorithm alg iterations algName =
        printfn $"%s{algName} algorithm:"
        printfn "%9s\t%9s\t%9s\t%9s\t%9s\t%12s" "Name" "Dimension" "TSP" "TSP*" "Error" "Time"
        printfn "%s" (String.replicate 100 "-")
        let names, dimensions, weights, optimalSolutions, errors, runTimes =
            tspGraphs |> Array.map (fun tspG ->
                    let TspGraph (name, _, dimension, optimalSolution, G) as _ = tspG
                    let runTime = int64 (measureRunTime alg G iterations)
                    (alg G) |> Seq.toList 
                    |> getTotalWeightFromTree G
                    |> (fun weight ->
                        let error = float(weight-optimalSolution)/float optimalSolution
                        printfn $"%9s{name}\t%9i{dimension}\t%9i{weight}\t%9i{optimalSolution}\t%9f{error}\t%12i{runTime}"
                        (name, dimension, weight, optimalSolution, error, runTime))
                )
                |> Array.fold (fun (N, D, W, S, E, T) (n, d, w, s, e, t) -> N @ [n], D @ [d], W @ [w], S @ [s], E @ [e], T @ [t]) (List.Empty, List.Empty, List.Empty, List.Empty, List.Empty, List.Empty)
                
        saveToCSV   (Directory.GetCurrentDirectory() +/ "out" +/ algName)
                    (names |> List.toArray)
                    (dimensions |> List.toArray)
                    (weights |> List.toArray)
                    (optimalSolutions |> List.toArray)
                    (errors |> List.toArray)
                    (runTimes |> List.toArray)
        printf "\n \n"
        0
            
//    algorithm metricTsp 100
    algorithm nearestNeighbourHeuristic 1 "Nearest Neighbour"