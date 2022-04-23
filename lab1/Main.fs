module lab1.Main

open lab1.Utils
open lab1.Parsing
open lab1.Prim
open lab1.SimpleKruskal
open lab1.UnionFindKruskal

open System.IO

let printData (graphSize : int array) (avgEdgeSize : int array) (runtimes : int64 array) = 
    let ratios = [float 0] @ [ for i = 0 to graphSize.Length - 2 do yield (float runtimes[i+1] / float runtimes[i]) ]
    let c_estimates = [ for i = 0 to graphSize.Length - 1 do yield System.Math.Round ( float runtimes[i]/ float (graphSize[i] * avgEdgeSize[i]), 3) ]
    printfn "%9s\t%9s\t%9s\t%9s" "Size" "Time(ns)" "Costant" "Ratio"
    printfn "%s" (String.replicate 60 "-")
    for i = 0 to graphSize.Length - 1 do
        printfn "%9i\t%9i\t%9f\t%9.3f" graphSize[i] runtimes[i] c_estimates[i] ratios[i]
    printfn "%s" (String.replicate 60 "-")
    c_estimates

let printGraph (graphSize : int array) (runtimes : int64 array) (reference : int64 list) (switch : bool) (big: bool)= 
    [
        Chart.Line(graphSize, runtimes)
        |> Chart.withTraceInfo(Name="Measured time")
        |> Chart.withLineStyle(Width=2.0, Dash=StyleParam.DrawingStyle.Solid)

        Chart.Line(graphSize |> Array.distinct, reference)
        |> Chart.withTraceInfo(Name="Expected asymptotic growth")
        |> Chart.withLineStyle(Width=2.0, Dash=StyleParam.DrawingStyle.Solid) 
    ]
    |> Chart.combine
    |> Chart.withXAxisStyle("Graph size")
    |> Chart.withYAxisStyle("Run times")
    |> Chart.withTitle(if switch then "Prim" else "Simple Kruskal")
    |> Chart.savePNG(
        (if switch then
            "./out/prim" + (if big then "_big" else "")
        else
            "./out/simpleKruskal" + (if big then "_big" else "")),
        Width=(if big then 1600 else 800),
        Height=(if big then 1000 else 500)
    )

let measureRunTime f input numCalls =
    let watch = System.Diagnostics.Stopwatch()
    watch.Start()
    for _ = 1 to numCalls do
        f input |> ignore
    let time = watch.Elapsed.TotalMilliseconds * 1000000.0 // get nanoseconds
    watch.Stop()
    time / float numCalls

let getAverageBySize a =
    a
    |> Array.chunkBySize 4
    |> Array.fold (fun acc  x -> Array.append acc [| (Array.average x) |]) Array.empty

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
            str + (sprintf "\n%9i\t%9i\t%9i" ns.Length es.Length totalWeight)
        ) result
    |> printfn "%s"

let getRunTimeBySize l =
    Array.fold (fun acc  x -> Array.append acc [| (Array.average x) |]) Array.empty l

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

    let N_listDistinct = N_list |> Array.distinct
    let M_avgEdgeSize = 
        M_list 
        |> Array.map float
        |> getAverageBySize
        |> Array.map int

    printfn $"%i{graphs.Length} graphs built"

    let ufk = fun _ ->
    printfn "Union-Find Kruskal"
    
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

    let pr = fun _ -> 
        printfn "Prim"

        getResults (prim 0) graphs
        let prRunTimes = 
            Array.map (fun g -> measureRunTime (prim 0) g 1) graphs
            |> Array.chunkBySize 4
            |> getRunTimeBySize
            |> Array.map int64

        let constant = 
            printData N_listDistinct M_avgEdgeSize prRunTimes 
            |> List.last
            |> round
            |> int64
    
        let reference = [ for i=0 to (N_list.Length - 1) do yield  int64 (System.Math.Log2 N_list[i] * float M_list[i]) * constant ]
        printGraph N_list prRunTimes reference true true
        printGraph N_list prRunTimes reference true false
        printfn "Finished prim"
        
    let sk = fun _ ->
        printfn "Simple Kruskal"
        let skRunTimes = 
            Array.Parallel.map (fun g -> measureRunTime (simpleKruskal) g 10000) graphs
            |> Array.chunkBySize 4
            |> getRunTimeBySize
            |> Array.map int64

        let constant2 = 
            printData N_listDistinct M_avgEdgeSize skRunTimes 
            |> List.last
            |> round
            |> int64

        let reference2 = [ for i=0 to (N_list.Length - 1) do yield  int64 (N_list[i] * M_list[i]) * constant2 ]
        printGraph N_list skRunTimes reference2 false true
        printGraph N_list skRunTimes reference2 false false
        printfn "Finished simple kruskal"
    
    pr ()
    sk ()
    ufk()

    0
