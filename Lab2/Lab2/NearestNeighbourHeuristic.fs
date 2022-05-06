module Lab2.NearestNeighbourHeuristic

open Lab1.Graphs
open Lab2.ConstructiveHeuristic

let initialisation (Graph (V,_,_)) =
    match V with
    | [||] -> failwith "Invalid graph"
    | _ -> V[0] :: List.Empty
    
let selection (Graph (V,E,adjList)) (partialCircuit: Node list) =
    let lastNode = partialCircuit |> List.last
    let lastNodeIndex = lastNode - 1
    adjList[lastNodeIndex] |> List.filter (fun x ->
        let u, v, _ = E[x]
        if u = lastNode then
            not (partialCircuit |> List.contains v)
        else
            not (partialCircuit |> List.contains u)
    )
    |> List.sortBy (fun x ->
        let _, _, w = E[x]
        w
    )
    |> List.head
    |> (fun x ->
        let i,j,_ = E[x]
        if V[i] = lastNode then
            V[j]
        else
            V[i]
    )

let insertion v partialCircuit =
    partialCircuit @ [v]
    

let nearestNeighbourHeuristic G = ConstructiveHeuristic initialisation selection insertion G