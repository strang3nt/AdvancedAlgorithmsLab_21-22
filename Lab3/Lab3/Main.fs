module Lab3.Main

open Parsing
open KargerStein
open Graph
open Lab1.Utils

open System

open System.IO

[<EntryPoint>]
let main _ =
    let files = 
        getFiles (Directory.GetCurrentDirectory() +/ "dataset")

    let graphs = Array.Parallel.map buildGraph files
    
    printfn $"%i{graphs.Length} graphs"

    graphs
    |> Array.iteri (fun i (MinCutGraph (V, _, _, _, _ ) as g) -> 
        let k = int (floor ((Math.Log2(V.Length)) * (Math.Log2(V.Length)))) 
        printfn $"{Karger g k}")

    0
