module Lab2.ConstructiveHeuristic

open Lab1.Graphs

// Performs the selection of the edge of the given graph and the insertion in
// the partialCircuit until the TSP is completed (at most n times), hence the
// function has complexity O(n* (O(selection) + O(insertion)) )
let rec loop selection insertion (partialCircuit: int list) (Graph (V, _, _) as G) visitedMap =
    let n = V |> Array.length
    if partialCircuit.Length = n then
        0 :: partialCircuit
    else
        let node = selection G partialCircuit visitedMap
        let partialCircuit, visitedMap = insertion node partialCircuit G visitedMap
        loop selection insertion partialCircuit G visitedMap 
        
// Initialize the tsp for the algorithm as specified by the chosen heuristic
// and then loops between the selection and the insertion of the heuristic with
// complexity O(O(initialisation) + O(loop)) = O(O(initialisation) + O(n* (O(selection) + O(insertion)) )
let ConstructiveHeuristic initialisation selection insertion (G: Graph) =
    let TSP, visitedMap = initialisation G
    loop selection insertion TSP G visitedMap
