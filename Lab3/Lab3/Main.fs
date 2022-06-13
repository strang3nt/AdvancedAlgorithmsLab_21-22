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
    let files = getFiles (Directory.GetCurrentDirectory() +/ "../../.." +/ "dataset")
//    let files = getFiles (Directory.GetCurrentDirectory() +/ "dataset")

    let graphs = Array.Parallel.map buildGraph files
    
    printfn $"%i{graphs.Length} graphs"

//    let minCuts = graphs |> Array.mapi (fun i (MinCutGraph (V, _, _, _, _ ) as g) -> 
//        let k = int (floor (Math.Log2(V.Length)))
//        let minCut = Karger g k
//        printfn $"Graph {i} has Min Cut weight of {minCut}"
//        minCut
//        )
    
        
//    let writer = new StreamWriter (Directory.GetCurrentDirectory() +/ "../../.." +/ "out" +/ "minCuts" + ".txt")
//    minCuts |> Array.iter (fun minCut ->
//        writer.WriteLine $"%i{minCut}"
//    )
//    writer.Close()

    graphs |> Array.iteri (fun i g ->
        let minCut = StoerWagner' g
        printfn $"Graph {i} has Min Cut weight of {CutWeight g minCut}")
//    let MinCutGraph (V0, E0, A0, D0, W0) as g0 = graphs[0]
//    let MinCutGraph (V, E, A, D, W) as g1 = fixedMerge graphs[0] 5 8

    0
