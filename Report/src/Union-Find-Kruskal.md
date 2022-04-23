## Union-find Kruskal

The Kruskal's algorithm based on the Union-Find data structure has been 
implemented as follows:

```fsharp
// Computes one of the MSTs of the given graph using the kruskal algorithm 
// with the Union-Find data structure in O(mlog n)
let kruskalUF (Graph (nodes, edges, adjList)) =

    let mutable U = initUF nodes

    // sort the elements in the graph, which is composed by an array of edges 
    // and their respective weight, by weight
    Graph (nodes, edges, adjList)
        |> sortedEdges

        // Folds the array of sorted edges to operate on the given graph and 
        // produces a list containing the edges of the
        // MST and the respective weight of each edge in O(m)
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
```

The complexity of the algorithm depends on the implementation of the data 
structure which is the following:

```fsharp
type Size = int
type 'a UFNode = 'a * Size
```

There is a type alias `Size` of `int` used to represent the size of the tree 

```fsharp


// Changes the parent of the j element to i in the UnionFind data structure uf 
// and updates the size of i accordingly in O(log n)
let change_root (uf : Map<'a,'a UFNode>) i j =
    let i, _ = i
    let j, j_size = j
        // O(log n)
    uf  |> Map.change j (fun j ->
            match j with
            | None -> failwith "No value associated to the given element"
            | Some (_, j_size) -> Some (i, j_size))
        // O(log n)
        |> Map.change i (fun i ->
            match i with
            | None -> failwith "No value associated to the given element"
            | Some (i, i_size) -> Some (i, i_size + j_size))

```

is $O(m\log n)$, in fact:

- `initUf` initialize the Union-Find data structure and is $O(n)$ with 
$n=|V|$, number of nodes of the graph
- `sortedEdges` sorts the edges with Introsort and has complexity $O(n\log n)$
- the fold function has $O(m)$ complexity with $m=|E|$, number of edges
- `find` has complexity $O(d)$ with $d=\text{depth of Union-Find}$
- `union` has complexity $O(d)$ with $d=\text{depth of Union-Find}$; given the 
"union-by-size" implementation of the function the depth of the Union-Find is 
$O(\log n)$
