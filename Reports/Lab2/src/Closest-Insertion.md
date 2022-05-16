\newpage

# Closest Insertion heuristic

As for the Nearest Neighbour heuristic, Closest Insertion also uses the `ConstructiveHeuristic` function for its implementation, so the definition boils down to this:

```fsharp
let closestInsertionHeuristic G =
    ConstructiveHeuristic initialisation selection insertion G
```

In order to understand how it works, we have to look at the implementation of the `initialisation`, `selection` and `insertion` functions for this heuristic.

First, let's look at the `insertion` function:
```fsharp
// Find the edge from the first node of the graph with minimum weight and
// use its adjacent vertices as the first 2 of the circuit, setting them 
// as visited
let initialisation (Graph (V,E,A) as G) =
    match V, A with
    | [||], _
    | _, [||] -> failwith "Invalid graph"
    | _, _ -> 
        let i = List.head A[0] |> (fun e -> opposite 0 e G)
        let visitedMap = [ for v in 0..V.Length-1 -> v, false]
            |> dict
            |> Dictionary
        visitedMap[0] <- true
        visitedMap[i] <- true
        i :: 0 :: List.Empty, visitedMap
```

The cost for this function is $O(n)$, because it needs to initialise the support structure for the visited nodes, which are the nodes that have been added to the solution, and will allow for faster processing in the next two functions.

As for the `selection` function, the heuristic selects the node with the minimum distance from the vertex. This is the implementation:

```fsharp
// Select the vertex k not in the current circuit with minimum distance 
// from it. The distance of a vertex is the minimum weight of an edge 
// from k to any node of the circuit
let selection (Graph (V,E,adjList) as G)
              (partialCircuit: int list)
              (visitedMap: Dictionary<int, bool>) =
    seq { 0 .. V.Length - 1 }
    |> Seq.filter (fun v -> not visitedMap[v])
    |> Seq.minBy (fun v -> 
        let e = List.find
                (fun e -> let u = opposite v e G in visitedMap[u])
                adjList[v]
        let (_, _, w) = E[e]
        w)
```

Note that, because the adjacency list for each node is ordered by the weight of the edge, it is sufficient to find the first edge that connects the node to the circuit.
Accessing to elements of `visitedMap` has $O(1)$ complexity, appearing in both `Seq.filter` and `Seq.minBy`, so there is no additional complexity compared to the theoretical $O(n)$ for these operations.
Because all the steps in the pipeline have complexity $O(n)$, the overall complexity is also $O(n)$.

Finally, the `insertion` function

```fsharp
// Find the i, j consecutive nodes in the circuit for which the triangular
// inequality with the given node k (w_ik + w_jk - w_ij) has the minimum 
// value, then insert k between i and j
let insertion k
              partialCircuit
              (Graph (_,E,adjList))
              (visitedMap: Dictionary<int, bool>) =
    let _, insertIdx = 
        partialCircuit
//  Associate each node with its predecessor in the circuit
        |> List.pairwise
//  For each node j in the circuit, its predecessor i, and the 
// target node k calculate (w_ik + w_jk - w_ij):
        |> List.mapi (fun insertAt (i, j) ->
//  Get the weight for the edges (i,k), (j,k) and (i,j)
            let w_ik = getEdgeWeight adjList E i k
            let w_jk= getEdgeWeight adjList E j k 
            let w_ij = getEdgeWeight adjList E i j
//  Return the calculated value and the index for which 
// it was calculated
            (w_ik + w_jk - w_ij), insertAt + 1)
//  Get the index corresponding to the minimum value
        |> List.minBy (fun (w,_) -> w)

//  Set the node as visited
    visitedMap[k] <- true
//  Return the expanded circuit and the updated map
    List.insertAt insertIdx k partialCircuit, visitedMap
```

where the function `getEdgeWeight` is defined as following:

```fsharp
let findEdge (Es: Edges) u v e = 
        let (x,y,_) = Es[e] in x=u && y=v || x=v && y=u

let getEdgeWeight (adjList: AdjList) E u v =
    let _,_,w = 
        adjList[u]
        |> List.find (findEdge E u v)
        |> Array.get E
    w
```
The complexity of `getEdgeWeight` is given by the most complex operation in the pipeline, which is `List.find`, and so it results in $O(n)$ and, as mentioned above, the complexity of accessing `visitedMap` is $O(1)$.
The pipeline for `insertion` is composed only in functions with complexity $O(n)$, but since the `List.mapi` function contains `getEdgeWeight`, the overall complexity is $O(n^2)$; specifically, since `getEdgeWeight` is called 3 times, it is $O(3n^2)$.

In the end, the whole `ClosestinsertionHeuristic` has time complexity $O(O(initialisation) + O(n * (O(selection) + O(insertion)))) = O(n + n*(n+n^2)) = O(n + n^3) = O(n^3)$.
