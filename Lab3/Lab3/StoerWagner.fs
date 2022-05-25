module Lab3.StoerWagner

open System.Collections.Generic
open Lab1.Graphs
open Lab3.Graph

type Key = int * Weight


let CrossingEdges (MinCutGraph (V, E, _, _, _)) (S: Nodes) =
    E |> Array.filter (fun (uIdx,vIdx,_) ->
            (S |> Array.contains V[uIdx]) <> (S |> Array.contains V[vIdx])
        ): Edges
    
let CutWeight G (S: Nodes) =
    CrossingEdges G S |> Array.sumBy (fun (_,_,w) -> w) : int
    
    
let merge (MinCutGraph (V, E, A, D, W) as G) =
    G
    
let rec codiceBello (PQ: SortedSet<Key>) A s t =
    let uIdx, _ = PQ.Max
    match A[]
    
let stMinCut (MinCutGraph (V, E, A, D, W) as G) =
    let Q = SortedSet<Key>()
    [|0..V.Length-1|] |> Array.iter (fun idx -> Q.Add(idx,0) |> ignore)
    let mutable s = -1
    let mutable t = -1
    
    while Q.Count > 0 do
        let u, _ = Q.Max
        s <- t
        t <- u
        A[]
        for uIdx, vIdx, w in A[uIdx] do
            if Q.Contains (V[vIdx], 0) then
                Q.Add 
    V,V[0],V[1]
    
let rec GlobalMinCut (MinCutGraph (V, _, _, _, _) as G) =
    let CutWeight = CutWeight G
    if V.Length = 2 then
        [| V[0]; V[1] |] : Nodes
    else
        let c1, _, _ = stMinCut G
        let c2 = GlobalMinCut (merge G)
        if CutWeight c1 <= CutWeight c2 then
            c1
        else
            c2