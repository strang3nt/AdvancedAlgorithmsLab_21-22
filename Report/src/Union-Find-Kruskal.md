## Union-find Kruskal

The Kruskal's algorithm based on the Union-Find data structure has been 
implemented as follows:

```fsharp
// Computes one of the MSTs of the given graph using the kruskal algorithm 
// with the Union-Find data structure in O(mlog n)
let UnionFindKruskal (Graph (nodes, edges, adjList)) =
    let mutable U = initUF nodes
    // sort the edges in the graph by weight
    Graph (nodes, edges, adjList)
        |> sortedEdges
        // Folds the array of sorted edges to operate on the given graph and 
        // produces a list containing the edges of the MST and the respective 
        // weight of each edge in O(m)
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

The complexity of the algorithm depends on the implementation of the methods 
of the Union-Find data structure, its internal representation is the following:

```fsharp
type Size = int                 // Size of the current set
type 'a UFNode = 'a * Size      // Pair representing the parent and the size 
                                // of the set
```

while externally the data structure is represented in the code as a `Map<'a,'a UFNode>`, 
with `'a` a type variable replaced at runtime by the effective type stored in the Union-Find.

It follows the code of the most important methods of the Union-Find data 
structure:

The `initUF` function initializes the Union-Find data structure starting from 
an array of objects with a complexity $O(n)$, with $n = p.Length$.

```fsharp
let rec initUF (p: 'a array) : Map<'a,'a UFNode> =
    Map (p |> Array.map (fun x -> x, (x, 1)))
```

The `find` function finds the root of the node `x` in the given Union-Find `uf` in $O(k)$, with $k = \max uf \text{depth} = \log n$

```fsharp
let rec find (uf: Map<'a,'a UFNode>) x =
    uf  |>  Map.find x
        |>  (fun (parent, size) ->
                if parent = x then
                    parent, size
                else
                    find uf parent
            )
```

The `union` function merges the sets in the Union-Find `uf` associated to the 
nodes `x` and `y` with a union-by-size policy, therefore the maximum depth of 
`uf` is $O(\log n)$, hence the complexity of the function is $O(k)$, with $k = \max uf \text(depth) = \log n$

```fsharp
let union (uf : Map<'a,'a UFNode>) x y =
    // O(log n)
    let x_root, x_size = find uf x
    // O(log n)
    let y_root, y_size = find uf y
    if x_root = y_root then
        uf
    elif x_size >= y_size then
        // O(log n)
        change_root uf (x_root, x_size) (y_root, y_size)
    else
        // O(log n)
        change_root uf (y_root, y_size) (x_root, x_size)
```

From the complexity of the methods of the Union-Find data structure it's 
possible to determine the complexity of the Union-Find Kruskal's algorithm, 
which is $O(m\log n)$, in fact:

- `initUf` has complexity $O(n)$ and is executed only once;
- `sortedEdges` sorts the edges with Introsort and has complexity $O(n\log n)$;
- the fold function iterates through the edges, hence it has complexity $O(m)$ with $m=|E|$, number of edges;
- `find` has complexity $O(\log n)$ and it's executed $O(m)$ times;
- `union` has complexity $O(\log n)$ and it's executed $O(m)$ times.
