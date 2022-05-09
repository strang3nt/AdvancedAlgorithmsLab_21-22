module Lab2.NearestNeighbourHeuristic

open Lab1.Graphs
open Lab2.ConstructiveHeuristic

// Initialise the list of indexes of the partialCircuit with the index of the
// first node of the given graph and create the map used to store information
// about the already visited nodes in O(n)
let initialisation (Graph (V,_,_)) =
    match V with
    | [||] -> failwith "Invalid graph"
    | _ ->
        let visitedMap = Map.ofArray ({ 0..V.Length-1} |> Seq.toArray |> Array.map (fun vIdx -> vIdx,false))
        0 :: List.Empty, visitedMap |> Map.change 0 (fun _ -> Some true)
    
let selection (Graph (_, E, adjList) as G) (partialCircuitIndexes: int list) (visitedMap: Map<int, bool>) =
    let lastNodeIdx = partialCircuitIndexes |> List.head
//  Selects the adjacency list of the last node inserted in the partial circuit
//  with complexity O(n)
    adjList[lastNodeIdx]
//  Filters the adjacency list of the last node by checking the visited map and
// removing nodes already in the partial circuit with complexity O(n)
    |> List.filter (fun e ->
        let uIdx = opposite lastNodeIdx e G
        not (visitedMap[uIdx])
    )
//  Takes the node associated to the edge with the minimum weight in O(n)
    |> List.minBy (fun eIdx ->
        let (_, _, w) = E[eIdx]
        w
    )
//  O(1)
    |> (fun e ->
        opposite lastNodeIdx e G
    )

// Inserts the given node in the head of the partial circuit instead of
// appending it as specified by the actual heuristic, in order to execute the
// operation in O(1) and then updates the visited map setting as visited the
// inserted node with complexity O(log n)
let insertion vIdx partialCircuitIndexes visitedMap =
    vIdx :: partialCircuitIndexes, visitedMap |> Map.change vIdx (fun _ -> Some true)
    

let nearestNeighbourHeuristic G = ConstructiveHeuristic initialisation selection insertion G
