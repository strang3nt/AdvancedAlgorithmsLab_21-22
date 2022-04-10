module lab1.KruskalUF

open lab1.Graphs
open lab1.UnionFind

let kruskalUF (Graph (nodes, edges, adjList)) =
    let mutable U = initUF nodes
    // sort the elements in the graph, which is composed by an array of edges and their respective weight, by weight
    Graph (nodes, edges, adjList)
        |> sortedEdges
        // Folds the array of sorted edges in a list containing the edges of the MST and the respective weight of each edge in O(m)
        |> Array.fold (fun acc edge_idx ->
            let u_idx, v_idx, weight = edges[edge_idx]
            let u = nodes[u_idx]
            let v = nodes[v_idx]
            // O(log n)
            if not ((find U u) = (find U v)) then
                // O(log n)
                U <- union U u v
                (u, v, weight) :: acc
            else acc) 
            List.empty