module lab1.SimpleKruskal

open Graphs

let isAcyclical A n1 n2 : bool =
    let mutable found1 = false
    let mutable found2 = false
    Seq.tryFind (fun (v1, v2, _) -> 
        if (v1 = n1 || v2 = n1) then
            found1 <- true
        if (v1 = n2 || v2 = n2) then
            found2 <- true
        found1 && found2
    ) A |> ignore
    not (found1 && found2)
    
let simpleKruskal (Graph (_, g, _)) = 
    Array.sortBy (fun (_, _, weight) -> weight) g 
    |> Array.fold (fun acc ((n1, n2, w) as e) -> if (isAcyclical acc n1 n2) then e :: acc else acc ) List.empty
