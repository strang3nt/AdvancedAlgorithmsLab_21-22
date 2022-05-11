module Lab2.ClosestInsertionHeuristic

open Lab1.Graphs
open Lab2.ConstructiveHeuristic

// Find the edge from the first node of the graph with minimum weight and add
// use those vertices as the first 2 of the circuit
let initialisation (Graph (V,E,A)) =
    match V, A with
    | [||], _
    | _, [||] -> failwith "Invalid graph"
    | _, _ -> 
        let _, i = A[0]
                |> List.map (fun e -> let x,y,w = E[e] in if x = 0 then w,y else w,x) 
                |> List.minBy (fun (w,_) -> w)
        i :: 0 :: List.Empty, ()

// Select the vertex k not in the current circuit with minimum distance from it
// The distance is delta(C) = ...
let selection (Graph (V,E,adjList)) (partialCircuit: int list) _ =
    let rest = V |> Array.mapi (fun i -> fun _ -> i) |> Array.filter (fun x -> not (List.contains x partialCircuit))
    let _, k = rest
                |> Array.map (fun k -> 
                    adjList[k]
                    |> List.map (fun e -> let x,y,w = E[e] in w, (if x = k then x else y))
                    |> List.minBy (fun (w,_) -> w))
                |> Array.minBy (fun (w,_) -> w)
    k


// Type that indicates what a weight in the list is generated from
// IJ and JK are for edges connecting a j node to either an i or a k node
// Sum indicates that the weight is the sum of some other nodes's weights
// NotRelated indicates weights of edges from j that are not needed for the result
type EdgeType = IJ | JK | Sum | NotRelated

// Utility functions that allow to obtain the weight of the edge from u to v
let private findEdge (Es: Edges) u v e = let (x,y,_) = Es[e] in x=u && y=v || x=v && y=u
let private getEdgeWeight (adjList: AdjList) E u v =
    let _,_,w = 
        adjList[u]
        |> List.find (findEdge E u v)
        |> Array.get E
    w

// Find the i, j consecutive nodes in the circuit for which the triangular
// inequality with the given node k (w_ik + w_jk - w_ij) has the minimum value,
// then insert k between i and j
let insertion k partialCircuit (Graph (_,E,adjList)) _ =
    let _, i = 
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
//  Find the (i,j) and (j,k) edges and get their weights
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
//  Return the calculated value and the i node
            w_jk_ij + w_ik, j)
//  Get the minimum value
        |> List.minBy (fun (w,_) -> w)

//  Split the circuit at the vertex i and insert k before the vertex j
    let circ1, circ2 = 
        partialCircuit
        |> List.findIndex (fun x -> x = i)
        |> List.splitAt <| partialCircuit
    circ1 @ [k] @ circ2, ()


let closestInsertionHeuristic G = ConstructiveHeuristic initialisation selection insertion G