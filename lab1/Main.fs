module lab1.Main

open Parsing
open SimpleKruskal

// For more information see https://aka.ms/fsharp-console-apps
open Plotly.NET
open Plotly.NET.ImageExport
open System.IO

let printData (graphsSize : int array) (avgEdgeSize : int array) (runtimes : int64 array) = 
    let ratios = [float 0] @ [ for i = 0 to graphsSize.Length - 2 do yield (float runtimes[i+1] / float runtimes[i]) ]
    let c_estimates = [ for i = 0 to graphsSize.Length - 1 do yield System.Math.Round ( float runtimes[i]/ float (graphsSize[i] * avgEdgeSize[i]), 3) ]
    printfn "%9s\t%9s\t%9s\t%9s" "Size" "Time(ns)" "Costant" "Ratio"
    printfn "%s" (String.replicate 60 "-")
    for i = 0 to graphsSize.Length - 1 do
        printfn "%9i\t%9i\t%9f\t%9.3f" graphsSize[i] runtimes[i] c_estimates[i] ratios[i]
    printfn "%s" (String.replicate 60 "-")
    c_estimates

let printGraph (graphsSize : int array) (runtimes : int64 array) (reference : int64 list) = 
    [
        Chart.Line(graphsSize |> Array.distinct, runtimes)
        |> Chart.withTraceInfo(Name="Measured time")
        |> Chart.withLineStyle(Width=2.0, Dash=StyleParam.DrawingStyle.Solid)

        Chart.Line(graphsSize, reference)
        |> Chart.withTraceInfo(Name="Expected asymptotic growth")
        |> Chart.withLineStyle(Width=2.0, Dash=StyleParam.DrawingStyle.Solid) 
    ]
    |> Chart.combine
    |> Chart.withXAxisStyle("Graph size")
    |> Chart.withYAxisStyle("Run times")
    |> Chart.withTitle("Simple Kruskal")
    |> Chart.savePNG(
        "lab1"+/"out"+/"simpleKruskal",
        Width=800,
        Height=500
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
            let r : (int * int * int) list = f g
            let totalWeight = 
                r 
                |> List.sumBy (fun (_,_,w) -> w)
            str + (sprintf "\n%9i\t%9i\t%9i" ns.Length es.Length totalWeight)
        ) result
    |> printfn "%s"

[<EntryPoint>]
let main argv =
    let path = Directory.GetCurrentDirectory() +/ "lab1" +/ "graphs"
    let files = 
        Directory.GetFiles (path)
        |> Array.sort
        |> Array.truncate 30

    printfn "Found %i files" files.Length

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

    printfn "%i graphs built" graphs.Length

    getResults simpleKruskal graphs
    // simple kruskal runtimes
    let skRunTimes = 
        Array.map (fun g -> measureRunTime (simpleKruskal) g 1) graphs
        |> getAverageBySize
        |> Array.map int64

    let constant = 
        printData N_listDistinct M_avgEdgeSize skRunTimes 
        |> List.last
        |> round
        |> int64

    let reference = [ for i=0 to (N_list.Length - 1) do yield  int64 (N_list[i] * M_list[i]) * constant ]

    printGraph N_list skRunTimes reference

    printfn "Finished simple Kruskal"

    0
