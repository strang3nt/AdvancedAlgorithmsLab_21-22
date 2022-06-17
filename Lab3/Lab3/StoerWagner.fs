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
    
let stMinCut (MinCutGraph (V, E, A, D, W) as G) =
        
    let Q = SortedSet<Key>()
    let keys = Array.create V.Length 0
    [|0..V.Length-1|] |> Array.iter (fun i -> Q.Add (Key (keys[i], i)) |> ignore )
    
    let mutable s: int option = None
    let mutable t: int option = None
    
    while Q.Count > 0 do
        
        let Key (_, uIdx) as k = Q.Max
        Q.ExceptWith (seq { k })
        
        s <- t
        t <- Some uIdx
        
        A[uIdx] |> List.iter (fun eIdx ->
            let vIdx = opposite uIdx eIdx G
//            let _, _, w = E[eIdx]
            let key = Key (keys[vIdx], vIdx)
            if Q.Contains key then
                keys[vIdx] <- keys[vIdx] + W[uIdx, vIdx]
                Q.ExceptWith (seq { key })
                Q.Add (Key (keys[vIdx], vIdx)) |> ignore
            )
    V |> Array.removeAt t.Value, s.Value, t.Value
    
let rec StoerWagner (MinCutGraph (V, _, _, _, _) as G) =
    if V.Length = 2 then
        [| V[0] |]
    else
        let c1, s, t = stMinCut G
        let c2 = StoerWagner (fixedMerge G s t)
        if CutWeight G c1 <= CutWeight G c2 then
            c1
        else
            c2
            
let stMinCut' (MinCutGraph (V, E, _, D, W) as G) =
        
    let Q = SortedSet<Key>()
    let keys = Array.create (V.Length) 0
    let mergedNodes = D |> Array.filter (fun x -> x = 0) |> Array.length
    [|0..keys.Length-1|] |> Array.iter (fun i -> Q.Add (Key (keys[i], i)) |> ignore )
    
    let mutable s: int option = None
    let mutable t: int option = None
    
    while Q.Count-mergedNodes > 0 do
        
        let Key (_, uIdx) as k = Q.Max
        Q.ExceptWith (seq { k })
        
        s <- t
        t <- Some uIdx
        
        W[uIdx, *] |> Array.mapi (fun i w -> i, w)
        |> Array.filter (fun (_,x) -> x > 0)
        |> Array.iter (fun (vIdx,_) ->
//            let eIdx = E |> Array.findIndex (fun (u,v,_) -> (u = uIdx && v = vIdx) || (u = vIdx && v = uIdx))
//            let vIdx = opposite uIdx eIdx G
            let key = Key (keys[vIdx], vIdx)
            if Q.Contains key then
                keys[vIdx] <- keys[vIdx] + W[uIdx, vIdx]
                Q.ExceptWith (seq { key })
                Q.Add (Key (keys[vIdx], vIdx)) |> ignore
            )
            
    let merged = D |> Array.toList
                 |> List.mapi (fun i w -> w, i)
                 |> List.filter (fun (w, _) -> w = 0)
                 |> List.map snd
                 
    let toRemove = t.Value :: merged |> List.sort
//    [| toRemove.Length-1..-1..0 |] |> Array.map (fun i -> V |> Array.removeAt toRemove[i])
    List.foldBack (fun i V -> V |> Array.removeAt i) toRemove V, s.Value, t.Value
    
let rec StoerWagner' (MinCutGraph (V, _, _, D, _) as G) =
    if D |> Array.filter (fun x -> x > 0) |> Array.length = 2 then
        [| V[D |> Array.findIndex (fun x -> x > 0)] |]
    else
        let c1, s, t = stMinCut' G
        let c2 = StoerWagner' (merge G s t)
        if CutWeight G c1 <= CutWeight G c2 then
            c1
        else
            c2
            
