module lab1.Main

open lab1.Parsing
open lab1.SimpleKruskal

// For more information see https://aka.ms/fsharp-console-apps
open Plotly.NET
open Plotly.NET.ImageExport
open System.IO

let printData (graphsSize : int array) (runtimes : int array) = 
    let ratios = [float 0] @ [ for i = 0 to graphsSize.Length - 2 do yield System.Math.Round (float runtimes[i+1] / float runtimes[i], 3) ]
    let c_estimates = [ for i = 0 to graphsSize.Length - 1 do yield System.Math.Round (float runtimes[i]/ float graphsSize[i], 3) ]
    printfn "%9s\t%9s\t%9s\t%9s" "Size" "Time(ns)" "Costant" "Ratio"
    printfn "%s" (String.replicate 60 "-")
    for i = 0 to graphsSize.Length - 1 do
        printfn "%9i\t%9i\t%9.3f\t%9.3f" graphsSize[i] runtimes[i] c_estimates[i] ratios[i]
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
    |> Chart.withTitle("Simple Kruskal")
    |> Chart.savePNG(
        "./out/simpleKruskal",
        Width=800,
        Height=500
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
        |> Array.map (int)

    let constant = 
        printData graphsSize skRunTimes 
        |> List.last
        |> round
        |> int

    let reference = [ for i in graphsSize do yield i * constant ]

    printGraph graphsSize skRunTimes reference

    printfn "Finished simple Kruskal"

    0
