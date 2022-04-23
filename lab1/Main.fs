module lab1.Main

open lab1.Utils
open lab1.Parsing
open lab1.SimpleKruskal
open lab1.UnionFindKruskal

open System.IO

[<EntryPoint>]
let main argv =
    
    let iterations = 100
    
    let path = Directory.GetCurrentDirectory() +/ "graphs"
    let files = 
        Directory.GetFiles (path)
        |> Array.sort

    printfn $"Found %i{files.Length} files"

    let graphs = Array.Parallel.map buildGraph files
    let sizes = [| for f in files do (getHeader f) |] // get number of nodes per graph
    let M_list = Array.map (fun (x: int array) -> x[1]) sizes
    let N_list = Array.map (fun (x: int array) -> x[0]) sizes

    printfn $"%i{graphs.Length} graphs built"

    printfn "Starting Union-Find Kruskal"
    
    // Union-Find Kruskal run times
    let kruskalUFTimes = 
        Array.map (fun g -> measureRunTime (UnionFindKruskal) g iterations) graphs
        |> Array.map (int)

    // array containing the hidden constants computed for each graph with the Union-Find Kruskal algorithm
    let C = printData N_list M_list kruskalUFTimes MlogN_estimate_f
    
    let constant =
        C   |> Array.average
            |> round

    let reference f =
        [| for i=0 to N_list.Length-1 do yield constant * f M_list[i] N_list[i] |]
    
    printfn $"Hidden constant: %f{constant}"
    
    printGraph N_list kruskalUFTimes (reference MlogN)
    saveToCSV (Directory.GetCurrentDirectory() +/ "out" +/ "unionFindKruskal") N_list M_list kruskalUFTimes C

    printfn "Finished Union-Find Kruskal"

    0
