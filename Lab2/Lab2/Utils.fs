module Lab2.Utils

open FSharp.Collections
open Lab1.Graphs

let getTotalWeightFromTree G H =
    H 
    |> List.pairwise 
    |> List.sumBy ( 
        fun (u, v) -> 
            match (w u v G) with
            | Some (n) -> n
            | None -> failwith "Utils::getTotalWeightFromTree: each element of input list should pairwise match an edge"
            )
 