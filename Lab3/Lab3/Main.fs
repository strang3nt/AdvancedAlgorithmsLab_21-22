module Lab3.Main

open Parsing
open KargerStein
open StoerWagner
open Graph
open Lab1.Utils

open System
open System.IO

let karger_estimation_f m (n: int) (time: int64) =
    Math.Round (float time / (float (n*n) * Math.Pow((Math.Log2 (float n)), 3.0)), 3)

let karger_complexity m n =
    Math.Round (float (n*n) * Math.Pow((Math.Log2 (float n)), 3.0))

let parallel_karger_estimation_f m (n: int) (time: int64) =
    Math.Round (float time / ((float (n*n) * Math.Pow((Math.Log2 (float n)), 3.0)) / 6.0), 3)

let parallel_karger_complexity m n =
    Math.Round ((float (n*n) * Math.Pow((Math.Log2 (float n)), 3.0))/ 6.0)


let saveToCSV filename (N: int array) (M: int array) (minCut: int array) (rTs: int64 array) (instant: int64 array) (C: float array) (ratio: float list)=
    let dateTime = DateTime.UtcNow.ToString().Replace('/','-').Replace(' ','_')
    let writer = new StreamWriter (filename + "_" + dateTime + ".csv")
    writer.WriteLine "N, M, MinCut, Time(ns), Instant(ns), Constant, Ratio,"
    for i=0 to N.Length-1 do
        writer.WriteLine $"%9i{N[i]}, %9i{M[i]}, %9i{minCut[i]}, %9i{rTs[i]}, %9i{instant[i]}, %12.3f{C[i]}, %9.3f{ratio[i]},"
    writer.Close()

let karger_saveToCSV filename (N: int array) (M: int array) (minCut: int array) (rTs: int64 array) (instant: int64 array) (C: float array) (ratio: float list)=
    let dateTime = DateTime.UtcNow.ToString().Replace('/','-').Replace(' ','_')
    let writer = new StreamWriter (filename + "_" + dateTime + ".csv")
    writer.WriteLine "N, M, MinCut, Time(ns), Instant(ns), Constant, Ratio,"
    for i=0 to N.Length-1 do
        writer.WriteLine $"%9i{N[i]}, %9i{M[i]}, %9i{minCut[i]}, %9i{rTs[i]}, %9i{instant[i]}, %12.3f{C[i]}, %9.3f{ratio[i]},"
    writer.Close()

[<EntryPoint>]
let main _ =
    let files = getFiles (Directory.GetCurrentDirectory() +/ "../../.." +/ "dataset")
//    let files = getFiles (Directory.GetCurrentDirectory() +/ "dataset")

    let graphs = Array.Parallel.map buildGraph files
    
    printfn $"%i{graphs.Length} graphs"

    let graphsN = 
        graphs
        |> Array.map (fun (MinCutGraph (V, _, _, _, _ )) -> V.Length)

    let graphsM = 
        graphs
        |> Array.map (fun (MinCutGraph (_, E, _, _, _ )) -> E.Length)
    
    let reference f (m: int array) (n: int array) c =
        [| for i=0 to n.Length-1 do yield c * f m[i] n[i] |]

    let MNi_list =
            (graphsN, graphsM) ||> Array.mapi2 (fun i m n -> (int64 m) * (int64 n), i)
            |> Array.sort

    // ------ calculate and print Karger's data
    let data = 
        graphs
        |> Array.mapi (fun i (MinCutGraph (V, _, _, _, _ ) as g) -> 
            
            let k = int (floor ((Math.Log2(V.Length)) * (Math.Log2(V.Length))))
            let t_start = DateTime.Now.Ticks
            let (result, instant) = (Parallel_Karger g k)
            let t_end = DateTime.Now.Ticks - t_start
            printfn $"Graph {i} done: {result}"
            (result, instant, t_end))

    let minCuts = Array.map (fun (i,_,_) -> i) data
    let instant = Array.map (fun (_,i,_) -> i) data
    let runtimes = Array.map (fun (_,_,t) -> t) data
    let (c_estimates, ratios) = printData graphsN graphsM runtimes parallel_karger_estimation_f

    let referenceArray = reference parallel_karger_complexity graphsN graphsM (Array.last c_estimates)
    let orderedRunTimes = MNi_list |> Array.fold (fun acc (_, i) -> acc @ [runtimes[i]] ) List.empty |> Array.ofList
    let MN_list = MNi_list |> Array.map fst

    karger_saveToCSV (Directory.GetCurrentDirectory() +/ "out" +/ "Karger-Stein") graphsN graphsM  minCuts runtimes instant c_estimates ratios
    printGraphs (graphsN |> Array.map int64) runtimes MN_list orderedRunTimes (graphsN |> Array.map int64) referenceArray "Karger-Stein" "Karger-stein"
    // -----------
    
    graphs |> Array.iteri (fun i g ->
        let minCut = StoerWagner' g
        printfn $"Graph {i} has Min Cut weight of {CutWeight g minCut}")
    
    0
