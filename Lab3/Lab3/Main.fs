module Lab3.Main

open Lab1.Parsing
open Lab1.Utils

open System.IO


let main _ =
    let files = getFiles (Directory.GetCurrentDirectory() +/ "dataset")

    let graphs = Array.Parallel.map buildGraph files
    
    printfn $"%i{graphs.Length} graphs"