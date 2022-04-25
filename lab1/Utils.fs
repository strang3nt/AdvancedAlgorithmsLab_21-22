module lab1.Utils

open System
open Plotly.NET
open Plotly.NET.ImageExport

let (+/) path1 path2 = IO.Path.Combine(path1, path2)
let MN m n =
    Math.Round (float m * float n, 3)

let MlogN m n =
    Math.Round (float m * Math.Log2 (float n), 3)
    
let MN_estimate_f m n (time: int64) =
    Math.Round (float time / (float m * float n), 3)
    
let MlogN_estimate_f m n (time: int64) =
    Math.Round (float time / (float m * Math.Log2 (float n)), 3)

let printData (graphsN : int array) (graphsM: int array) (runtimes : int64 array) (estimation_f: int -> int -> int64 -> float) = 
    let ratios = [float 0] @ [ for i = 0 to graphsN.Length - 2 do yield Math.Round (float runtimes[i+1] / float runtimes[i], 3) ]
    let c_estimates = [| for i = 0 to graphsN.Length - 1 do yield estimation_f graphsM[i] graphsN[i] runtimes[i]|]
    printfn "%9s\t%9s\t%9s\t%9s" "Size" "Time(ns)" "Constant" "Ratio"
    printfn "%s" (String.replicate 60 "-")
    for i = 0 to graphsN.Length - 1 do
        printfn $"%9i{graphsN[i]}\t%9i{runtimes[i]}\t%9.3f{c_estimates[i]}\t%9.3f{ratios[i]}"
    printfn "%s" (String.replicate 60 "-")
    c_estimates, ratios

let graph (x1 : int64 array) (y1 : int64 array) (x2: int64 array) (y2 : float array) (filename: string) (big: bool) (title: string) (x_label: string) = 
    [
        Chart.Line(x1, y1)
        |> Chart.withTraceInfo(Name="Measured time")
        |> Chart.withLineStyle(Width=2.0, Dash=StyleParam.DrawingStyle.Solid)

        Chart.Line(x2, y2)
        |> Chart.withTraceInfo(Name="Expected asymptotic growth")
        |> Chart.withLineStyle(Width=2.0, Dash=StyleParam.DrawingStyle.Solid) 
    ]
    |> Chart.combine
    |> Chart.withXAxisStyle(x_label)
    |> Chart.withYAxisStyle("Run times")
    |> Chart.withTitle(title)
    |> Chart.savePNG(
        "out" +/ filename + (if big then "_big" else ""),
        Width=(if big then 1920 else 800),
        Height=(if big then 1080 else 500)
    )

let printGraphs x1 y1 x2 y2 x3 y3 filename title =
    graph x1 y1 x3 y3 ("N"+filename) false title "N"
    graph x1 y1 x3 y3 ("N"+filename) true title "N"
    graph x2 y2 x2 y3 ("MN"+filename) false title "M*N"
    graph x2 y2 x2 y3 ("MN"+filename) true title "M*N"
    
let measureRunTime f input numCalls =
    let defaultLatency = Runtime.GCSettings.LatencyMode
    Runtime.GCSettings.LatencyMode <- Runtime.GCLatencyMode.SustainedLowLatency
    let watch = Diagnostics.Stopwatch()
    watch.Start()
    for _ = 1 to numCalls do
        f input |> ignore
    let time = watch.Elapsed.TotalMilliseconds * 1000000.0 // get nanoseconds
    watch.Stop()
    Runtime.GCSettings.LatencyMode <- defaultLatency
    time / float numCalls
    
let getRunTimeBySize (l: float[][]) =
    Array.fold (fun acc  x -> Array.append acc [| (Array.average x) |]) Array.empty l
    
let saveToCSV filename (N: int array) (M: int array) (rTs: int64 array) (C: float array) (ratio: float list)=
    let dateTime = DateTime.UtcNow.ToString().Replace('/','-').Replace(' ','_')
    let writer = new IO.StreamWriter (filename + "_" + dateTime + ".csv")
    writer.WriteLine "N M Time(ns) Constant Ratio"
    for i=0 to N.Length-1 do
        writer.WriteLine $"%9i{N[i]} %9i{M[i]} %9i{rTs[i]} %12.3f{C[i]} %9.3f{ratio[i]}"
    writer.Close()
