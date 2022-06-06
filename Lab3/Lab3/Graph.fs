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

let opposite n e ( MinCutGraph (_, es, _, _, _) ) =
    let (n1, n2, _) = es[e]
    if n1 = n then n2 else n1

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


let fixedMerge G u v =
//  Updates only D and W
    let MinCutGraph (V, E, A, D, W) as G = merge G u v
    
//  Map adjList of v to a list containing also the index of the opposite node to v (=s)
    A[v] |> List.map (fun eIdx -> opposite v eIdx G, eIdx) 
//       Iterates the newly created list
         |> List.iter (fun (sIdx, eVIdx) ->
//          Try to find s in the adjList of u
            let k =
                A[u] |> List.tryFind (fun eUIdx -> sIdx = opposite u eUIdx G)
                     
            match k with
    //      s adjacent to v but not to u
            | None ->
    //          set E[eVIdx] to point to u
                E[eVIdx] <- u, sIdx, W[u, sIdx]
    //          Add e to adjList of u and sort the new list by weight
                A[u] <- (eVIdx :: A[u]) |> List.sortBy (edgeWeight E)
                ()
    //      s adjacent to v and u
            | Some eUIdx ->
    //          "Void" E[eVIdx]
                E[eVIdx] <- -1, -1, 0
                
    //          Update E[eUidx]
                E[eUIdx] <- u, sIdx, W[u,sIdx]
                let _, removeIdx = A[sIdx] |> List.mapi (fun i e -> e,i) |> List.find (fun (e,_) -> e = eVIdx)
                A[u] <- A[u] |> List.sortBy (edgeWeight E)
                A[sIdx] <- A[sIdx] |> List.removeAt removeIdx |> List.sortBy (edgeWeight E)
                ()
    )
             
// Create new W with appropriate dimension
//    let newW = Array2D.create (V.Length-1) (V.Length-1) 0
//    [0..V.Length-1] |> newW[]
//    [0..V.Length-1] |> 
// There are void edges in E, but in order to remove them we should update A as well
    MinCutGraph (V |> Array.removeAt v,
                 E |> Array.map (fun (s, t, w) -> (if s > v then s-1 else s), (if t > v then t-1 else t), w ),
                 A |> Array.removeAt v,
                 D |> Array.removeAt v,
                 W |> Array.removeAt v )
