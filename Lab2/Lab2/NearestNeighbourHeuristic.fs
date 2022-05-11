module Lab2.NearestNeighbourHeuristic

open System.Collections
open System.Collections.Generic
//open FSharpx.Collections
open Lab1.Graphs
open Lab2.ConstructiveHeuristic

// Initialise the list of indexes of the partialCircuit with the index of the
// first node of the given graph and create a dictionary used to store
// information about the already visited nodes in O(n)
let initialisation (Graph (V,_,_)) =
    match V with
    | [||] -> failwith "Invalid graph"
    | _ ->
        let visited = [ for v in 0..V.Length-1 -> v, false] |> dict |> Dictionary
        visited[0] <- true
        [0], visited
    
let selection (Graph (_, E, adjList) as G) (partialCircuitIndexes: int list) (visited: Dictionary<int, bool>) =
    let lastNodeIdx = partialCircuitIndexes |> List.head
//  Selects the adjacency list of the last node inserted in the partial circuit
//  with complexity O(1)
    adjList[lastNodeIdx]
//  Filters the adjacency list of the last node by checking the visited map and
// removing nodes already in the partial circuit with complexity O(n)
    |> List.filter (fun e ->
        let uIdx = opposite lastNodeIdx e G
        not (visited[uIdx])
    )
    |> List.head
//  Takes the node associated to the edge with the minimum weight in O(n)
//    |> List.minBy (fun eIdx ->
//        let (_, _, w) = E[eIdx]
//        w
//    )
//  O(1)
    |> (fun e ->
        opposite lastNodeIdx e G
    )

// Inserts the given node in the head of the partial circuit instead of
// appending it as specified by the actual heuristic, in order to execute the
// operation in O(1) and then updates the visited map setting as visited the
// inserted node with complexity O(log n)
let insertion vIdx partialCircuitIndexes (visited: Dictionary<int, bool>) =
    visited[vIdx] <- true
    vIdx :: partialCircuitIndexes, visited
    

let nearestNeighbourHeuristic G = ConstructiveHeuristic initialisation selection insertion G
