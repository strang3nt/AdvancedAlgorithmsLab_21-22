module Lab3.Graph

open System.Collections.Generic
open Lab1.Graphs

// 2 dimensional array
type W = int[,]
type D = int array
[<Struct>]
type MinCutGraph = MinCutGraph of Nodes * Edges * AdjList * D * W

let buildW (ns: Nodes) (es: Edges): W =
    let W = Array2D.create ns.Length ns.Length 0
    es
    |> Array.iter (fun (u, v, w) -> W[u, v] <- w; W[v, u] <- w)
    W

let buildD (es: Edges) (adj : AdjList): D =
    adj
    |> Array.map (List.sumBy (fun e -> let (_, _, w) = es[e] in w))
