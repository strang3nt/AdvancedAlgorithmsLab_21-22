## Union-find Kruskal

The Kruskal's algorithm based on the Union-Find data structure has been 
implemented as follows:

```fsharp
// Computes one of the MSTs of the given graph using the kruskal algorithm 
// with the Union-Find data structure in O(mlog n)
let UnionFindKruskal (Graph (nodes, edges, adjList)) =
    let mutable U = initUF nodes
    // sort the elements in the graph, which is composed by an array of edges 
    // and their respective weight, by weight
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
of the Union-Find data structure, which is internally represented as follows

```fsharp
type Size = int                 // Size of the current set
type 'a UFNode = 'a * Size      // Pair representing the parent and the size 
                                // of the set
```

finally the data structure is represented in the code as a `Map<'a,'a UFNode>`, 
with `'a` a type variable for indicating the effective type stored in the Union-Find.

It follows the code of the most important methods of the Union-Find data 
structure:

The `initUF` function initializes the Union-Find data structure starting from 
an array of objects with a complexity $O(n)$, with $n =$ length of the given 
array.

```fsharp
let rec initUF (p: 'a array) : Map<'a,'a UFNode> =
    Map (p |> Array.map (fun x -> x, (x, 1)))
```

The `find` function finds the root of the node in the given Union-Find in 
$O(k)$, with $k = uf depth = \log n $

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

The `union` function merges the sets in the Union-Find uf associated to the 
nodes x and y with a union-by-size policy.
The complexity of the method is $O(k)$, with $k = uf depth = \log n$

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

From the complexity of the methods of the Union-Find data structure it's 
possible to determine the complexity of the Union-Find Kruskal's algorithm, 
which is indeed $O(m\log n)$
- `initUf` initialize the Union-Find data structure and is $O(n)$ with 
$n=|V|$, number of nodes of the graph
- `sortedEdges` sorts the edges with Introsort and has complexity $O(n\log n)$
- the fold function has $O(m)$ complexity with $m=|E|$, number of edges
- `find` has complexity $O(d)$ with $d=\text{depth of Union-Find}$
- `union` has complexity $O(d)$ with $d=\text{depth of Union-Find}$; given the 
"union-by-size" implementation of the function the depth of the Union-Find is 
$O(\log n)$
