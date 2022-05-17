module Lab3.Main

open Lab3.Parsing
open Lab1.Utils

open System.IO

[<EntryPoint>]
let main _ =
    let files = getFiles (Directory.GetCurrentDirectory() +/ "dataset")

    let graphs = Array.Parallel.map buildGraph files
    
    printfn $"%i{graphs.Length} graphs"

    0
