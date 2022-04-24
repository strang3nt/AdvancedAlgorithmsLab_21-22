module lab1.Main

open lab1.Utils
open lab1.Parsing
open lab1.Prim
open lab1.SimpleKruskal
open lab1.UnionFindKruskal

open System.IO

let getResults f graphs : unit =
    let result = 
        sprintf "%9s\t%9s\t%9s\n" "Nodes" "Edges" "MST weight" +
        String.replicate 50 "-"
    graphs
    |> Array.fold ( fun str (Graphs.Graph (ns, es, _) as g) -> 
            let r : Graphs.Edge list = f g
            let totalWeight = 
                r 
                |> List.sumBy (fun (_,_,w) -> w)
            str + $"\n%9i{ns.Length}\t%9i{es.Length}\t%9i{totalWeight}"
        ) result
    |> printfn "%s"

[<EntryPoint>]
let main argv =
    
    let iterations = 1
    let path = Directory.GetCurrentDirectory() +/ "graphs"
    let files = 
        Directory.GetFiles path
        |> Array.sort

    printfn $"Found %i{files.Length} files"

    let graphs = Array.Parallel.map buildGraph files
    let sizes = [| for f in files do (getHeader f) |] // get number of nodes per graph
    let M_list = Array.map (fun (x: int array) -> x[1]) sizes
    let N_list = Array.map (fun (x: int array) -> x[0]) sizes
    let N_distinctList = N_list |> Array.distinct
    let MNi_list =
            (M_list, N_list) ||> Array.mapi2 (fun i m n -> m*n, i)
            |> Array.sort

    let reference f (m: int array) (n: int array) c =
        [| for i=0 to n.Length-1 do yield c * f m[i] n[i] |]
    
    let reference f c = reference f M_list N_list c
    
    let mLogNReference = reference MlogN
    let mnReference = reference MN

    printfn $"%i{graphs.Length} graphs built"
    
    getResults UnionFindKruskal graphs

    let algorithm f estimation_f reference filename name =
        printfn $"\n%s{name}"
        
        let runTimes = Array.map (fun g -> measureRunTime f g iterations) graphs
            
        let nAvgRunTime =
            runTimes |> Array.chunkBySize 4
            |> getRunTimeBySize
            |> Array.map int64
        
        let i64RunTimes =
            runTimes |> Array.map int64

        // array containing the hidden constants computed for each graph
        let C = printData N_list M_list i64RunTimes estimation_f
        
        let c = C |> Array.last
        
        let orderedRunTimes = MNi_list |> Array.fold (fun acc (_, i) -> acc @ [i64RunTimes[i]] ) List.empty |> Array.ofList
        let MN_list = MNi_list |> Array.map fst
        
        printGraphs N_distinctList nAvgRunTime MN_list orderedRunTimes N_list (reference c) filename name
        saveToCSV (Directory.GetCurrentDirectory() +/ "out" +/ filename) N_list M_list i64RunTimes C

        printfn $"Finished %s{name}\n"
    
    algorithm simpleKruskal MN_estimate_f mnReference "SimpleKruskal" "Simple Kruskal"
    algorithm UnionFindKruskal MlogN_estimate_f mLogNReference "UnionFindKruskal" "Union-Find Kruskal"
//    algorithm (prim 0) MlogN_estimate_f mLogNReference "Prim" "Prim"

    0
