module Lab2.NearestNeighbourHeuristic

open System.Collections.Generic
open Lab1.Graphs
open Lab2.ConstructiveHeuristic

// Initialise the list of indexes of the partialCircuit with the index of the
// first node of the given graph and create a dictionary used to store
// information about the already visited nodes in O(n)
let initialisation (Graph (V,_,_)) =
    match V with
    | [||] -> failwith "Invalid graph"
    | _ ->
        let visitedMap = [ for v in 0..V.Length-1 -> v, false] |> dict |> Dictionary
        visitedMap[0] <- true
        [0], visitedMap
    
let selection (Graph (_, _, adjList) as G) (partialCircuitIndexes: int list) (visitedMap: Dictionary<int, bool>) =
//  Gets the index of the last inserted node in the partial circuit
    let lastNodeIdx = partialCircuitIndexes |> List.head
//  Selects the adjacency list of the last node inserted in the partial circuit
//  with complexity O(1)
    adjList[lastNodeIdx]
//  Returns the index of the first edge where the opposite node isn't in
//  the partialCircuit, which means that, given the fact that the adjacency list is sorted
//  in ascending order by weight, the node opposite node is the one with
//  minimum weight that must be selected.
//  The complexity of the method is O(n)
    |> List.find (fun e ->
        let uIdx = opposite lastNodeIdx e G
        not (visitedMap[uIdx])
    )
//  Returns the index of the opposite node of the edge with minimum weight from
//  the last inserted node in O(1)
    |> (fun e ->
        opposite lastNodeIdx e G
    )

// Inserts the given node in the head of the partial circuit instead of
// appending it as specified by the actual heuristic, in order to execute the
// operation in O(1) and then updates the visited map setting as visited the
// inserted node with complexity O(log n)
let insertion vIdx partialCircuitIndexes (visitedMap: Dictionary<int, bool>) =
    visitedMap[vIdx] <- true
    vIdx :: partialCircuitIndexes, visitedMap
    

let nearestNeighbourHeuristic G = ConstructiveHeuristic initialisation selection insertion G
