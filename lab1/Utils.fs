module lab1.Utils

open System
open Plotly.NET
open Plotly.NET.ImageExport

let (+/) path1 path2 = IO.Path.Combine(path1, path2)
let MN m n =
    Math.Round (float m * float n, 3)

let MlogN m n =
    Math.Round (float m * Math.Log2 (float n), 3)
    
let MN_estimate_f m n time =
    Math.Round (float time / float m * float n, 3)
    
let MlogN_estimate_f m n time =
    Math.Round (float time / (float m * Math.Log2 (float n)), 3)

let printData (graphsN : int array) (graphsM: int array) (runtimes : int array) (estimation_f: int -> int -> int -> float) = 
    let ratios = [float 0] @ [ for i = 0 to graphsN.Length - 2 do yield Math.Round (float runtimes[i+1] / float runtimes[i], 3) ]
    let c_estimates = [| for i = 0 to graphsN.Length - 1 do yield estimation_f graphsM[i] graphsN[i] runtimes[i]|]
    printfn "%9s\t%9s\t%9s\t%9s" "Size" "Time(ns)" "Constant" "Ratio"
    printfn "%s" (String.replicate 60 "-")
    for i = 0 to graphsN.Length - 1 do
        printfn $"%9i{graphsN[i]}\t%9i{runtimes[i]}\t%9.3f{c_estimates[i]}\t%9.3f{ratios[i]}"
    printfn "%s" (String.replicate 60 "-")
    c_estimates

    
let printGraph (graphsSize : int array) (runtimes : int array) (reference : float array) = 
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
    let defaultLatency = Runtime.GCSettings.LatencyMode
    Runtime.GCSettings.LatencyMode <- Runtime.GCLatencyMode.SustainedLowLatency
    let watch = Diagnostics.Stopwatch()
    watch.Start()
    for i = 1 to numCalls do
        f input |> ignore
    let time = watch.Elapsed.TotalMilliseconds * float 1000000 // get nanoseconds
    watch.Stop()
    Runtime.GCSettings.LatencyMode <- defaultLatency
    time / float numCalls
    
let saveToCSV filename (N: int array) (M: int array) (rTs: int array) (C: float array) =
    let dateTime = DateTime.UtcNow.ToString().Replace('/','-').Replace(' ','_')
    let writer = new IO.StreamWriter (filename + "_" + dateTime + ".csv")
    writer.WriteLine "n, m, runtime, c"
    for i=0 to N.Length-1 do
        writer.WriteLine $"%9i{N[i]}, %9i{M[i]}, %9i{rTs[i]}, %9.3f{C[i]}"
    writer.Close()