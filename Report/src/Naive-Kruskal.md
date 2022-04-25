\newpage

## Naive Kruskal

Follows Naive Kruskal without auxiliary functions:

```fsharp
let simpleKruskal (Graph (ns, es, _) as G) =

    // Init A, it will hold the mst
    let mstEdges = List.empty
    let mstAdj = Array.create ns.Length List.empty
    let A = MST (mstEdges, mstAdj)

    // sort edges in non decreasing order
    sortedEdges G

    // iterate over edges
    |> Array.fold (
        fun acc e ->
            match e with

            // if adding edge to MST doesn't make it acyclical, add edge
            | e when (isAcyclical e A G) -> 
                let A = updateMst e A G
                es[e] :: acc

            // else don't add edge
            | _ -> acc
        ) List.empty
```
Cost is $O( n*m )$: 

 - `sortedEdge` function has worst case complexity $O(n\log(n))$, since it internally uses Introsort
 - the folding function has a $O(m)$ complexity, with $m = |M|$, the number of edges
 - `isAcyclical` has a $O(n)$ complexity.

The following code snippet is the function in charge of checking whether if the MST becomes cyclic by adding a new edge:

```fsharp
let rec cycleDfs 
    v 
    (G: Graph) 
    (MST (_, adj) as A : MST) 
    (nv: Visit array) 
    (ev: Visit array)
    : bool =

    nv[v] <- Visited

    // forall adjacent nodes to v
    List.forall (fun e ->

        // if edge e is not visited and it is part of MST
        if (ev[e] = NotVisited) then

            // get u, adjacent node of v in edge e
            let u = opposite v e G

            // if u is not visited
            if (nv[u] = NotVisited) then

                // set edge (u, v) as visited and continue
                ev[e] <- Visited
                (cycleDfs u G A nv ev)

            // else we have found a backedge, thus a cycle
            else false
        else true
        ) adj[v]
```

`cycleDfs` is a DFS based algorithm, that stops whenever it finds a cycle in a graph.
`MST` is a type that represents the current MST: this way the function checks only the tree and
nothing else. The function checks all the edges which are, in a MST, at most $n - 1$. Thus the overall cost
is $O( n )$. More precisely, it is $O( 2n )$, because the adjacency list holds each edge twice,
but the constant can be dropped.
