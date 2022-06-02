module Lab3.StoerWagner

open System.Collections.Generic
open Lab1.Graphs
open Lab3.Graph

[<Struct>]
type Key = Key of (Weight * int)

let CrossingEdges (MinCutGraph (V, E, _, _, _)) (S: Nodes) =
    E |> Array.filter (fun (uIdx,vIdx,_) ->
            (S |> Array.contains V[uIdx]) <> (S |> Array.contains V[vIdx])
        ): Edges
    
let CutWeight G (S: Nodes) =
    CrossingEdges G S |> Array.sumBy (fun (_,_,w) -> w) : int
    
    
let merge (MinCutGraph (V, E, A, D, W) as G) u v =
    D[u] <- D[u] + D[v] - 2 * W[u, v]
    D[v] <- 0
    W[u, v] <- 0
    W[v, u] <- 0
    for w = 0 to V.Length - 1 do
        if w <> u && w <> v then
            W[u, w] <- W[u, w] + W[v, w]
            W[w, u] <- W[w, u] + W[w, v]
            W[v, w] <- 0
            W[w, v] <- 0
    MinCutGraph (V, E, A, D, W)
    
let stMinCut (MinCutGraph (V, E, A, D, W) as G) =
        
    let Q = SortedSet<Key>()
    let keys = Array.create V.Length 0
    
    [|0..V.Length-1|] |> Array.iter (fun idx -> Q.Add (Key (keys[idx], idx)) |> ignore )
    let mutable s: int option = None
    let mutable t: int option = None
    
    while Q.Count > 0 do
        let Key (_, uIdx) as k = Q.Max
        Q.ExceptWith (seq { k })
        s <- t
        t <- Some uIdx
        A[uIdx] |> List.iter (fun eIdx ->
            let vIdx = opposite uIdx eIdx G
//            let vIdx = V[vIdx]
            let _, _, w = E[eIdx]
            let key = Key (keys[vIdx], vIdx)
            if Q.Contains key then
                keys[vIdx] <- keys[vIdx] + w
                Q.ExceptWith (seq { key })
                Q.Add (Key (keys[vIdx], vIdx)) |> ignore
            )
    V |> Array.removeAt t.Value, s.Value, t.Value
    
let rec StoerWagner (MinCutGraph (V, _, _, _, _) as G) =
    let CutWeight = CutWeight G
    if V.Length = 2 then
        [| V[0]; V[1] |] : Nodes
    else
        let c1, s, t = stMinCut G
        let c2 = StoerWagner (merge G s t)
        if CutWeight c1 <= CutWeight c2 then
            c1
        else
            c2