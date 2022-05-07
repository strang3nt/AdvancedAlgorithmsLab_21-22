module Lab2.ConstructiveHeuristic

open Lab1.Graphs

let rec loop selection insertion (partialCircuit: int list) (Graph (V, _, _) as G) =
    let n = V |> Array.length
    if partialCircuit.Length = n then
        0 :: partialCircuit
    else
        let node = selection G partialCircuit
        let partialCircuit = insertion node partialCircuit
        loop selection insertion partialCircuit G
        
let ConstructiveHeuristic initialisation selection insertion (G: Graph) =
    let TSP = initialisation G
    loop selection insertion TSP G
