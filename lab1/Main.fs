module lab1.Main

open lab1.Parsing
open lab1.SimpleKruskal

// For more information see https://aka.ms/fsharp-console-apps
open Plotly.NET
open Plotly.NET.ImageExport
open System.IO

let printData (graphsSize : int array) (runtimes : float array) = 
    let ratios = [ for i=0 to graphsSize.Length - 2 do yield System.Math.Round (runtimes[i+1]/runtimes[i]) ]
    let c_estimates = [ for i = 0 to graphsSize.Length - 1 do yield System.Math.Round (runtimes[i]/float graphsSize[i], 3) ]
    printfn "Size\tTime(ns)\tCostant\t\tRatio"
    printfn "%s" (String.replicate 50 "-")
    for i = 0 to graphsSize.Length - 1 do
        printfn $"{graphsSize[i]}\t{runtimes[i]}\t{c_estimates[i]}\t{ratios[i]}"
    printfn "%s" (String.replicate 50 "-")
    c_estimates

let printGraph (graphsSize : int array) (runtimes : float array) (reference : int list) = 
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
    |> Chart.withTitle("Simple Kruskal")
    |> Chart.savePNG(
        "./out/simpleKruskal",
        Width=500,
        Height=500
    )

let measureRunTime f input numCalls =
    let watch = System.Diagnostics.Stopwatch()
    watch.Start()
    for i = 1 to numCalls do
        f input |> ignore
    let time = float watch.ElapsedMilliseconds
    watch.Stop()
    time / float numCalls

let getRunTimeBySize l =
    Array.fold (fun acc (x : float array) -> Array.append acc [| (Array.average x) |]) Array.empty l

[<EntryPoint>]
let main argv =
    let files = 
        Directory.GetFiles (Directory.GetCurrentDirectory() + "/graphs/")
        |> Array.map Path.GetFileName
        |> Array.sort

    printfn "Found %i files" files.Length

    let graphs = [| for f in files do buildSimpleGraph f |]
    let graphsSize = [| for f in files do (getHeader f).[0] |] |> Array.distinct // get number of nodes per graph

    printfn "%i graphs built" graphs.Length

    // simple kruskal runtimes
    let skRunTimes = 
        Array.Parallel.map (fun g -> measureRunTime (simpleKruskal) g 10000) graphs
        |> Array.chunkBySize 4
        |> getRunTimeBySize

    let constant = 
        printData graphsSize skRunTimes 
        |> List.last
        |> round
        |> int

    let reference = [ for i in graphsSize do yield i * constant ]

    printGraph graphsSize skRunTimes reference

    printfn "Finished simple Kruskal"

    0
