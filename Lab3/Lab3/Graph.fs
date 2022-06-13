module Lab3.Graph

open System
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
    
//  Calculates the indexes of the edges that have been removed
    let eVIdxs =
//       Map adjList of v to a list containing also the index of the opposite node to v (=s)
         A[v] |> List.map (fun eIdx -> opposite v eIdx G, eIdx) 
//       Iterates the newly created list
         |> List.map (fun (s, eVIdx) -> 
//          Try to find s in the adjList of u
            match A[u] |> List.tryFind (fun eUIdx -> s = opposite u eUIdx G) with
//          s adjacent to v but not to u
            | None ->
                if s = u then
//                    A[u] <- A[u] |> List.removeAt (A[u] |> List.findIndex ( fun e -> e = eVIdx))
                    Some eVIdx
                else
    //              set E[eVIdx] to point to u
                    E[eVIdx] <- u, s, W[u, s]
    //              Add e to adjList of u and sort the new list by weight
                    A[u] <- (eVIdx :: A[u]) |> List.sortBy (edgeWeight E)
                    None
//          s adjacent to v and u
            | Some eUIdx ->
//                if s = u then None
//                else
    //          "Void" E[eVIdx]
//                E[eVIdx] <- -1, -1, 0
                
    //          Update E[eUidx]
                E[eUIdx] <- u, s, W[u,s]
                let removeIdx = A[s] |> List.findIndex (fun e -> e = eVIdx)
                
                A[u] <- A[u] |> List.sortBy (edgeWeight E)
                A[s] <- A[s] |> List.removeAt removeIdx |> List.sortBy (edgeWeight E)
                Some eVIdx
    )
         
//  Update E by shifting the edges in place of the removed ones
    let eVIdxs = eVIdxs |> List.filter (fun e -> e.IsSome) |> List.map (fun e -> e.Value) |> List.sort
    
    let shiftedIdxs =
        eVIdxs |> List.mapi (fun i eIdx ->
            let includedIdxs =
                [ E.Length-1..-1..E.Length-i-1 ]
                    |> List.filter (fun i -> eVIdxs |> List.contains i )
                    |> List.length
            
            let shiftedEIdx =
                [ E.Length-includedIdxs-i-1..-1..E.Length-eVIdxs.Length-i-1 ]
                    |> List.find (fun i -> not ( eVIdxs |> List.contains i ))
                    
            if eIdx > shiftedEIdx then
                None, eIdx
            else
            E[eIdx] <- E[shiftedEIdx]
            Some shiftedEIdx,eIdx
            )
//        |> List.filter (fun e -> e.IsSome)
//        |> List.map (fun e -> e.Value)
//        |> List.map (fun e -> e, false)
    
    let newE : Edges =
        E |> Array.truncate (E.Length-eVIdxs.Length)
            |> Array.map (fun (s, t, w) -> (if s > v then s-1 else s), (if t > v then t-1 else t), w )
    
        
    shiftedIdxs |> List.iter (fun (e1, e2) ->
        match e1 with
        | None ->
            let u, v, _ = E[e2]
            A[u] <- A[u] |> List.filter (fun e -> not (e = e2))
            A[v] <- A[v] |> List.filter (fun e -> not (e = e2))
        | Some e1 ->
            let u, v, _ = E[e1]
            A[u] <- A[u] |> List.map (fun e -> if e = e1 then e2 else e )
            A[v] <- A[v] |> List.map (fun e -> if e = e1 then e2 else e )
        )
    
    
    let newA : AdjList = A |> Array.removeAt v
        
//    let newA : AdjList =
//        A |> Array.removeAt v
//        |> Array.map (fun a ->
//            a |> List.map (fun e ->
//                let idx = shiftedIdxs |> List.tryFind (fun (eIdx,_) -> eIdx = e)
//                match idx with
//                | None -> e
//                | Some (_,e) -> e
//                )
//            )

// Create new W with appropriate dimension
    let newW = Array2D.init (V.Length-1) (V.Length-1) (fun i j ->
            let i = if i >= v then i+1 else i
            let j = if j >= v then j+1 else j
            W[i, j]
        )
    
// There are void edges in E, but in order to remove them we should update A as well
    MinCutGraph (V |> Array.removeAt v,
                 newE,
                 newA,
                 D |> Array.removeAt v,
                 newW)