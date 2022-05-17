module Lab3.Graph

open System.Collections.Generic
open Lab1.Graphs

type EdgesDict = Dictionary<KeyValuePair<Node, Node>, Weight>
[<Struct>]
type MinCutGraph = MinCutGraph of Nodes * Edges * EdgesDict

// returns the weight of an edge or 0, given 2 nodes u, v
let getEdgeWeight (u: Node) (v: Node) (MinCutGraph (_, _, esDict) as G: MinCutGraph) =
    match esDict.TryGetValue (KeyValuePair(u, v)) with
    | true, value -> value
    | _ -> 
        match esDict.TryGetValue (KeyValuePair(v, u)) with
        | true, value -> value
        | _ -> 0

let getEdgesDict (es: Edges): EdgesDict =
    es
    |> Array.map (fun (u, v, w) -> KeyValuePair(u, v), w)
    |> dict
    |> Dictionary
