module Lab3.Graph

open System.Collections.Generic
open Lab1.Graphs

type W = Dictionary<KeyValuePair<Node, Node>, Weight>
type D = int array
[<Struct>]
type MinCutGraph = MinCutGraph of Nodes * Edges * D * W

// returns the weight of an edge or 0, given 2 nodes u, v
let getEdgeWeight (u: Node) (v: Node) (MinCutGraph (_, _, _, W) as G: MinCutGraph) =
    match W.TryGetValue (KeyValuePair(u, v)) with
    | true, value -> value
    | _ -> 
        match W.TryGetValue (KeyValuePair(v, u)) with
        | true, value -> value
        | _ -> 0

let getWeightedAdj (es: Edges): W =
    es
    |> Array.map (fun (u, v, w) -> KeyValuePair(u, v), w)
    |> dict
    |> Dictionary

let getWeightedDegree (es: Edges) (adj : AdjList): D =
    adj
    |> Array.map (List.sumBy (fun e -> let (_, _, w) = es[e] in w))
