module lab1.KruskalUF

open lab1.Graphs
open lab1.UnionFind

let kruskalUF (G: Graph2) =
    let mutable U = initUF (getNodes G)
    // sort the elements in the graph, which is composed by an array of edges and their respective weight, by weight
    G   |> Array.sortBy (fun (_, _, weight) -> weight)
        // Folds G in a list containing the edges of the MST and the respective weight of each edge
        |> Array.fold (fun acc (u, v, weight) ->
            if not ((find U u) = (find U v)) then
                U <- union U u v
                (u, v, weight) :: acc
            else acc) 
            List.empty