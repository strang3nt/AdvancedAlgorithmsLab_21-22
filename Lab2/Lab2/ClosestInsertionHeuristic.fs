module Lab2.ClosestInsertionHeuristic

open System.Collections.Generic
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
let selection (Graph (V,E,adjList)) (partialCircuit: int list) (visitedMap: Dictionary<int, bool>) =
    seq { 0 .. V.Length - 1 }
    |> Seq.filter (fun x -> not visitedMap[x])
    |> Seq.minBy (fun v -> 
        let (_, _, w) = 
            adjList[v]
            |> List.minBy (fun e -> 
                let x,y,w = E[e]
                let edgeConnection = visitedMap[x] || visitedMap[y]
                let w = if edgeConnection then w else System.Int32.MaxValue
                w)
            |> Array.get E
        w)

// Type that indicates what a weight in the list is generated from
// IJ and JK are for edges connecting a j node to either an i or a k node
// Sum indicates that the weight is the sum of some other nodes's weights
// NotRelated indicates weights of edges from j that are not needed for the result
type EdgeType = IJ | JK | Sum | NotRelated

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
        |> List.map (fun (i, j) ->
//  Get the weight for the edge (i,k)
            let w_ik = getEdgeWeight adjList E i k
            let _,w_jk_ij =
                adjList[j]
//  Find the (i,j) and (j,k) edges in the adjacency list and get their weights
                    |> List.map (fun e -> 
                        match E[e] with
                        | x,y,w when x = i || y = i -> IJ, w
                        | x,y,w when x = k || y = k -> JK, w
                        | _ -> NotRelated, 0)
//  Sum the weights according to formula
                    |> List.reduce (fun e1 -> fun e2 ->
                        let w = match e1 with
                                | IJ, w_ij -> -w_ij
                                | JK, w_jk -> w_jk
                                | Sum, w
                                | NotRelated, w -> w
                        match e2 with
                        | IJ, w_ij -> Sum, w - w_ij
                        | JK, w_jk -> Sum, w + w_jk
                        | Sum, w
                        | NotRelated, w -> Sum, w)
//  Return the calculated value and the j node for which it was calculated
            w_jk_ij + w_ik, j)
//  Get the vertex corresponding to the minimum value
        |> List.minBy (fun (w,_) -> w)

//  Split the circuit at the vertex j and insert k before it
    let circ1, circ2 = 
        partialCircuit
        |> List.findIndex (fun x -> x = j)
        |> List.splitAt <| partialCircuit
//  Set the node as visited
    visitedMap[k] <- true
//  Return the expanded circuit and the updated map
    circ1 @ [k] @ circ2, visitedMap


let closestInsertionHeuristic G = ConstructiveHeuristic initialisation selection insertion G
