module Lab3.StoerWagner

open System.Collections.Generic
open Lab1.Graphs
open Lab3.Graph

[<Struct>]
type Key = Key of (Weight * int)

// Calculates the edges that cross the given cut in G in O(m*log(n))
let CrossingEdges (MinCutGraph (V, E, _, _, _)) (S: Nodes) =
    E |> Array.filter (fun (uIdx,vIdx,_) ->
            (S |> Array.contains V[uIdx]) <> (S |> Array.contains V[vIdx])
        ): Edges
    
// Calculates the weight of the given cut in O(m*log(n))
let CutWeight G (S: Nodes) =
    CrossingEdges G S |> Array.sumBy (fun (_,_,w) -> w) : int
    
// Calculates the st minimum cut in G in O(m*log(n))
let stMinCut (MinCutGraph (V, _, _, D, W)) =
//  Initialise the priority queue and the keys array in O(n) with n = |V|
    let Q = SortedSet<Key>()
    let keys = Array.create (V.Length) 0
    let mergedNodes = D |> Array.filter (fun x -> x = 0) |> Array.length
    [|0..keys.Length-1|] |> Array.iter (fun i -> Q.Add (Key (keys[i], i)) |> ignore )
    
    let mutable s: int option = None
    let mutable t: int option = None
    
//  Iterate until the priority queue is empty O(m) times
    while Q.Count-mergedNodes > 0 do
        
//      Get the index of the node with the highest associated weight in the 
//      priority queue in O(1)
        let Key (_, u) as k = Q.Max
//      Remove the max element from the priority queue in O(1)
        Q.ExceptWith (seq { k })
        
        s <- t
        t <- Some u
        
//      Iterates through the nodes adjacent to u in O(n*log(n))
        W[u, *] |> Array.mapi (fun i w -> i, w)
        |> Array.filter (fun (_,x) -> x > 0)
        |> Array.iter (fun (v,_) ->
            let key = Key (keys[v], v)
//          Check if the element key is in the priority queue in O(log(n))
            if Q.Contains key then
//              Update the value of the key in O(1)
                keys[v] <- keys[v] + W[u, v]
//              Removes the the key from the priority queue in O(1)
                Q.ExceptWith (seq { key })
//              Adds the new key in priority queue in O(log(n))
                Q.Add (Key (keys[v], v)) |> ignore
            )

//  Gets the indexes of the nodes previously contracted in O(n)
    let merged = D |> Array.toList
                 |> List.mapi (fun i w -> w, i)
                 |> List.filter (fun (w, _) -> w = 0)
                 |> List.map snd
                 
//  Calculates the indexes of the nodes that has to be removed from V
    let toRemove = t.Value :: merged |> List.sort
//  Removes the contracted nodes and t from V and returns the generated cut
//  with s and t
    List.foldBack (fun i V -> V |> Array.removeAt i) toRemove V, s.Value, t.Value

// Calculates the global minimum cut of G in O(mn*log(n))
let rec StoerWagner (MinCutGraph (V, _, _, D, _) as G) =
//  Check the number of nodes in the graph in  O(n) with n = |V|
    if D |> Array.filter (fun x -> x > 0) |> Array.length = 2 then
//      Return the first node in the graph in O(n) with n = |V| and the weight
//      of the associated minimum cut
        let minCut = [| V[D |> Array.findIndex (fun x -> x > 0)] |]
        minCut, CutWeight G minCut
    else
//      Calculates the st minimum cut of G in O(m*log(n))
        let c1, s, t = stMinCut G
        let w1 = CutWeight G c1
//      Calculates the global minimum cut in G-{s,t} in O(mn*log(n))
        let c2, w2 = StoerWagner (merge G s t)
//      Compares the weight of the two minimum cuts and returns the smallest one
        if w1 <= w2 then
            (c1, w1)
        else
            (c2, w2)