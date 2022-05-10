module Lab2.Utils

open System.Diagnostics
open System.Runtime
open FSharp.Collections
open Lab1.Graphs

let getTotalWeightFromTree G H =
    H 
    |> List.pairwise 
    |> List.sumBy ( 
        fun (u, v) -> 
            match (w u v G) with
            | Some (n) -> n
            | None -> failwith "Utils::getTotalWeightFromTree: each element of input list should pairwise match an edge"
            )
 
let measureRunTime f input numCalls =
    let defaultLatency = GCSettings.LatencyMode
    GCSettings.LatencyMode <- GCLatencyMode.SustainedLowLatency
    let watch = Stopwatch()
    watch.Start()
    for _ = 1 to numCalls do
        f input |> ignore
    let time = watch.Elapsed.TotalMilliseconds * 1000000.0 // get nanoseconds
    watch.Stop()
    GCSettings.LatencyMode <- defaultLatency
    time / float numCalls
