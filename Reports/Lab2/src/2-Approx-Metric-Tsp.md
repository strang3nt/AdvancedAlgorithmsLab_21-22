\newpage

# 2-approximation Metric Tsp

```fsharp
// Optional list of children
type Children = int list option
// Array that contains at index i the children of Nodes[i] 
// (the parent in the MST) or None
type Tree = Children array

let private children v (T: Tree): Children = T[v]

// Given the output of Prim (which in our implementation is an array edges
// instead of parents) build the type Tree: should cost O(P.Length) => O(N)
let buildTree (P: Edge option array): Tree =
    let T = Array.create P.Length None
    Array.iteri (
        fun n e ->
            match e with
            | Some ( e ) -> 
                let (u, v, _) = e
                let p = if u = n then v else u
                match children p T with
                | Some ( chn ) -> T[p] <- Some ( n :: chn )
                | None -> T[p] <- Some ( n :: [] )
            | None -> () 
    ) P
    T

let preorder v T =
    let rec pre v (H: List<_>) T : unit =
        H.Add(v)
        match (children v T) with
        // if is internal continue preorder visit
        | Some ( ch ) -> for c in ch do pre c H T
        // else return
        | None -> ()
    let H = List()
    pre v H T
    H

let metricTsp (G: Graph) =
    let root = 0
    let H =
        // given a MST
        prim root G
        // build the tree-like data structure
        |> buildTree
        // visit in a preorder fashion the structure built 
        // while saving nodes visited into a list
        |> preorder root
    H.Add(root); H
```

The code above contains the implementation of the 2-approximation algorithm, it builds the MST using Prim.
The implementation follows closely the one from the lectures, which basically does a preorder visit on the
MST built using a MST algorithm: we chose Prim because it was the fastest between the ones we implemented 
during the last assignment.

`buildTree` is the only function that wasn't in the pseudo-code provided: the output of Prim is a list that 
contains at a given index the parent in the MST, it could have been complicated to implement a preorder visit
using only the list provided by Prim. Thus `buildTree` builds a convenience structure which is a `Tree` of `Children`:

 - `Tree` is an array of `Children` at index i there are the children of the node with index i in the graph
 - `Children` is an optional array (contains elements or no elements) and simply is a list of ints (which are
 indices to nodes in the graph).

The time complexity is the following:

 - `prim` is $O(m\log n)$
 - `buildTree` is $O(n)$ on the graph (nodes)
 - the preorder visits at most twice all edges thus costs $O(m)$.

Thus the overall complexity is $O(m\log n)$.
