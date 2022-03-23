module lab1.SimpleKruskal

open Graphs

let sortedEdges ((_, edges) : Graph): (int * int * Weight) list = 

    let couple (index : int) (list : (int * Weight) array) : (int * int * Weight) list =
        Array.fold (fun acc (idx2, weight) -> acc @ [(index, idx2, weight)]) List.empty list
    Array.fold (fun acc (idx, l) -> acc @ (couple idx l)) List.Empty edges
    |> List.sortBy (fun (_, _, weight) -> weight) // TODO: remove duplicate edges: there will be edges (1, 2) and (2, 1) 

let isAcyclical A n1 n2 : bool =
    let mutable found1 = false
    let mutable found2 = false

    for (idx1, idx2, _) in A do
        if (idx1 = n1 || idx2 = n1) then
            found1 <- true
        if (idx1 = n2 || idx2 = n2) then
            found2 <- true

    if found1 <> found2 then true else false

let simpleKruskal (g : Graph) : (int * int * Weight) list = 
    sortedEdges g |> 
    List.fold (
        fun acc (n1, n2, w) -> 
            if (isAcyclical acc n1 n2) then acc @ [ (n1, n2, w) ] else acc) // check if accumulator is acyclical
        List.empty
