module Lab3.Main

open Parsing
open KargerStein
open StoerWagner
open Graph
open Lab1.Utils

open System

open System.IO

[<EntryPoint>]
let main _ =
//    let files = getFiles (Directory.GetCurrentDirectory() +/ "../../.." +/ "dataset")
    let files = getFiles (Directory.GetCurrentDirectory() +/ "dataset")

    let graphs = Array.Parallel.map buildGraph files
    
    printfn $"%i{graphs.Length} graphs"

    graphs
    |> Array.iteri (fun i (MinCutGraph (V, _, _, _, _ ) as g) -> 
        let k = int (floor (Math.Log2(V.Length))) 
        printfn $"Graph {i} has Min Cut weight of {Karger g k}")
    
    graphs |> Array.iteri (fun i g ->
        let minCut = StoerWagner g
        printfn $"Graph {i} has Min Cut weight of {CutWeight g minCut}")

    0
