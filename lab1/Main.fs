module lab1.Main

open lab1.Parsing
open lab1.SimpleKruskal
open lab1.KruskalUF

// For more information see https://aka.ms/fsharp-console-apps
open Plotly.NET
open Plotly.NET.ImageExport
open System.IO

let printData (graphsSize : int array) (runtimes : int array) = 
    let ratios = [float 0] @ [ for i = 0 to graphsSize.Length - 2 do yield System.Math.Round (float runtimes[i+1] / float runtimes[i], 3) ]
    let c_estimates = [ for i = 0 to graphsSize.Length - 1 do yield System.Math.Round (float runtimes[i]/ float graphsSize[i], 3) ]
    printfn "%9s\t%9s\t%9s\t%9s" "Size" "Time(ns)" "Constant" "Ratio"
    printfn "%s" (String.replicate 60 "-")
    for i = 0 to graphsSize.Length - 1 do
        printfn $"%9i{graphsSize[i]}\t%9i{runtimes[i]}\t%9.3f{c_estimates[i]}\t%9.3f{ratios[i]}"
    printfn "%s" (String.replicate 60 "-")
    c_estimates

let printGraph (graphsSize : int array) (runtimes : int array) (reference : int list) = 
    [
        Chart.Line(graphsSize, runtimes)
        |> Chart.withTraceInfo(Name="Measured time")
        |> Chart.withLineStyle(Width=2.0, Dash=StyleParam.DrawingStyle.Solid)

        Chart.Line(graphsSize, reference)
        |> Chart.withTraceInfo(Name="Expected asymptotic growth")
        |> Chart.withLineStyle(Width=2.0, Dash=StyleParam.DrawingStyle.Solid) 
    ]
    |> Chart.combine
    |> Chart.withXAxisStyle("Graph size")
    |> Chart.withYAxisStyle("Run times")
    |> Chart.withTitle("Union-Find Kruskal")
    |> Chart.savePNG(
        "out"+/"unionFindKruskal",
        Width=1920,
        Height=1080
    )

let measureRunTime f input numCalls =
    let watch = System.Diagnostics.Stopwatch()
    watch.Start()
    for i = 1 to numCalls do
        f input |> ignore
    let time = watch.Elapsed.TotalMilliseconds * float 1000000 // get nanoseconds
    watch.Stop()
    time / float numCalls

let getRunTimeBySize l =
    Array.fold (fun acc  x -> Array.append acc [| (Array.average x) |]) Array.empty l

[<EntryPoint>]
let main argv =
    let path = Directory.GetCurrentDirectory() +/ "graphs"
    let files = 
        Directory.GetFiles (path)
        |> Array.sort
//        |> Array.truncate 40

    printfn $"Found %i{files.Length} files"

    let graphs = Array.Parallel.map buildGraph files
    let sizes = [| for f in files do (getHeader f) |] // get number of nodes per graph
    let M_list = Array.map (fun (x: int array) -> x[1]) sizes
    let N_list = Array.map (fun (x: int array) -> x[0]) sizes
    let N_listDistinct = N_list |> Array.distinct

    printfn $"%i{graphs.Length} graphs built"

    // kruskal Union-Find based runtimes
    let kruskalUFTimes = 
//        Array.Parallel.map (fun g -> measureRunTime (kruskalUF) g 100) graphs
        Array.map (fun g -> measureRunTime (kruskalUF) g 20) graphs
        |> Array.chunkBySize 4
        |> getRunTimeBySize
        |> Array.map (int)

    let constant = 
        printData N_listDistinct kruskalUFTimes 
        |> List.last
        |> round
        |> int

    let reference = [ for i in N_listDistinct do yield i * constant ]

    printGraph N_listDistinct kruskalUFTimes reference

    printfn "Finished Union-Find Kruskal"

    0
