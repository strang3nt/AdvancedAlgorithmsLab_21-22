module lab1.SimpleKruskal

open Graphs

let sortedEdges (g : Graph2): (int * int * Weight) List = 
    Array.sortBy (fun (_, _, weight) -> weight) g
    |> Array.toList

let isAcyclical A n1 n2 : bool =
    let mutable found1 = false
    let mutable found2 = false

    for (idx1, idx2, _) in A do
        if (idx1 = n1 || idx2 = n1) then
            found1 <- true
        if (idx1 = n2 || idx2 = n2) then
            found2 <- true

    if found1 <> found2 then true else false

let simpleKruskal (g : Graph2) : (int * int * Weight) list = 
    sortedEdges g
    |> List.fold ( fun acc (n1, n2, w) -> 
        if (isAcyclical acc n1 n2) then acc @ [ (n1, n2, w) ] else acc ) List.empty
