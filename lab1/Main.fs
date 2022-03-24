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
    let time = int watch.ElapsedMilliseconds
    watch.Stop()
    time / numCalls

[<EntryPoint>]
let main argv =
    let files = 
        Directory.GetFiles (Directory.GetCurrentDirectory() + "/graphs/")
        |> Array.map Path.GetFileName
        |> Array.sort

    printfn "Found %i files" files.Length

    let mutable i = 1
    let graphs = [| 
        for f in files do 
            printfn "Build graph %i: %s" i f
            i <- i + 1
            buildSimpleGraph f 
    |]
    let graphsSize = [| for f in files do (getHeader f).[1] |] // get number of edges per graph

    printfn "%i graphs built" graphs.Length

    // i <- 0
    // let runTimesSimpleKruskal = [| 
    //     for g in graphs do 
    //         printfn "[ %s ]" (String.replicate i "*" + String.replicate (68 - i) "-")
    //         yield measureRunTime (simpleKruskal) g 10 
    //         i <- i + 1
    // |]

    let runTimesSimpleKruskal = Array.Parallel.map (fun g -> measureRunTime (simpleKruskal) g 10) graphs

    Chart.Point(graphsSize, runTimesSimpleKruskal)
    |> Chart.savePNG(
        "./out/simpleKruskal",
        Width=500,
        Height=500
    )

    0
