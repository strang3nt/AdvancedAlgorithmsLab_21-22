module Lab2.NearestNeighbourHeuristic

open Lab1.Graphs
open Lab2.ConstructiveHeuristic

let initialisation (Graph (V,_,_)) =
    match V with
    | [||] -> failwith "Invalid graph"
    | _ -> 0 :: List.Empty
    
let selection (Graph (_, E, adjList) as G) (partialCircuit: int list) =
    let lastNode = partialCircuit |> List.head
    adjList[lastNode]
    |> List.filter (fun e ->
        let u = opposite lastNode e G
        not (partialCircuit |> List.contains u)
    )
    |> List.minBy (fun e ->
        let (_, _, w) = E[e]
        w
    )
    |> (fun e ->
        opposite lastNode e G
    )

let insertion v partialCircuit =
    v :: partialCircuit
    

let nearestNeighbourHeuristic G = ConstructiveHeuristic initialisation selection insertion G
