module lab1.Main

open lab1.Parsing
open lab1.SimpleKruskal

// For more information see https://aka.ms/fsharp-console-apps
open Plotly.NET
open Plotly.NET.ImageExport
open System.IO

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
    let skRunTimes = Array.Parallel.map (fun g -> measureRunTime (simpleKruskal) g 10000) graphs

    Chart.Line(graphsSize, skRunTimes |> Array.chunkBySize 4 |> getRunTimeBySize)
    |> Chart.withXAxisStyle("Graph size")
    |> Chart.withYAxisStyle("Run times")
    |> Chart.withTitle("Simple Kruskal")
    |> Chart.savePNG(
        "./out/simpleKruskal",
        Width=500,
        Height=500
    )

    printfn "Finished simple Kruskal"

    0
