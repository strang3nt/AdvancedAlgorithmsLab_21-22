module Lab2.ClosestInsertionHeuristic

open System.Collections.Generic
open System
open Lab1.Graphs
open Lab2.ConstructiveHeuristic

// Find the edge from the first node of the graph with minimum weight and
// use its adjacent vertices as the first 2 of the circuit, setting them 
// as visited
let initialisation (Graph (V,E,A) as G) =
    match V, A with
    | [||], _
    | _, [||] -> failwith "Invalid graph"
    | _, _ -> 
        let i = List.head A[0] |> (fun e -> opposite 0 e G)
        let visitedMap = [ for v in 0..V.Length-1 -> v, false] |> dict |> Dictionary
        visitedMap[0] <- true
        visitedMap[i] <- true
        i :: 0 :: List.Empty, visitedMap

// Select the vertex k not in the current circuit with minimum distance from it
// The distance of a vertex is the minimum weight of an edge from k to any node
// of the circuit
let selection (Graph (V,E,adjList) as G) (partialCircuit: int list) (visitedMap: Dictionary<int, bool>) =
    seq { 0 .. V.Length - 1 }
    |> Seq.filter (fun v -> not visitedMap[v])
    |> Seq.minBy (fun v -> 
        let e = List.find (fun e -> let u = opposite v e G in visitedMap[u]) adjList[v]
        let (_, _, w) = E[e]
        w)

// Find the i, j consecutive nodes in the circuit for which the triangular
// inequality with the given node k (w_ik + w_jk - w_ij) has the minimum value,
// then insert k between i and j
let insertion k partialCircuit (Graph (_,E,adjList)) (visitedMap: Dictionary<int, bool>) =
    let _, j = 
        partialCircuit
//  Associate each node with its predecessor in the circuit
        |> List.pairwise
//  For each node j in the circuit, its predecessor i, and the target node k
//  calculate (w_ik + w_jk - w_ij)
        |> List.mapi (fun insertAt (i, j) ->
//  Get the weight for the edge (i,k)
            let w_ik = getEdgeWeight adjList E i k
            let w_jk= getEdgeWeight adjList E j k 
            let w_ij = getEdgeWeight adjList E i j
//  Return the calculated value and the j node for which it was calculated
            (w_ik + w_jk - w_ij), insertAt + 1)
//  Get the vertex corresponding to the minimum value
        |> List.minBy (fun (w,_) -> w)

//  Set the node as visited
    visitedMap[k] <- true
//  Return the expanded circuit and the updated map
    List.insertAt j k partialCircuit, visitedMap


let closestInsertionHeuristic G = ConstructiveHeuristic initialisation selection insertion G
