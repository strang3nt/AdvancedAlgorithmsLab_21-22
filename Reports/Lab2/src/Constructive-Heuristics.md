\newpage

## Constructive heuristics

Given the fact that all the various constructive heuristics have the same 
structure we decided to take advantage of the choice of using a functional 
programming language and implemented a function that handles the structure of 
the algorithms for all the heuristics; this means that in order to implement a 
specific heuristic we will have to define only the three operations and pass 
them as parameters.
That function, called `ConstructiveHeuristic` is written as follows:

```fsharp
// Initialize the tsp for the algorithm as specified by the chosen 
// heuristic and then loops between the selection and the insertion 
// of the heuristic with complexity 
// O(O(initialisation) + O(loop)) =
// = O(O(initialisation) + O(n* (O(selection) + O(insertion))))
let ConstructiveHeuristic initialisation 
                          selection 
                          insertion 
                          (G: Graph) =

    let TSP, visitedMap = initialisation G
    loop selection insertion TSP G visitedMap
```

Note that `initialisation`, `selection` and `insertion` are three functions 
passed as parameters that differentiate and specify each heuristic.

In order to implement the loop foreseen by the constructive heuristics 
structure we added the auxiliary function `loop` that handles it.

In general, regardless of the specific heuristic, the complexity of 
`ConstructiveHeuristic` is $O(O(initialisation) + O(loop))$, which is equal to 
$O(O(initialisation) + O(n * (O(selection) + O(insertion))))$ because the 
`loop` function gets executed $n$ times and it's implemented as follows:

```fsharp
// Performs the selection of the edge of the given graph and the 
// insertion in the partialCircuit until the TSP is completed 
// (n loops in total), hence the function has complexity 
// O(n* (O(selection) + O(insertion)) )
let rec loop selection 
             insertion 
             (partialCircuit: int list) 
             (Graph (V, _, _) as G) 
             visitedMap =

    let n = V |> Array.length
    if partialCircuit.Length = n then
        0 :: partialCircuit
    else
        let node = selection G partialCircuit visitedMap
        let partialCircuit, visitedMap = insertion node 
                                                   partialCircuit 
                                                   visitedMap
        loop selection insertion partialCircuit G visitedMap 
```

The `loop` function performs the `selection` of the node from the graph and 
the `insertion` in the partial circuit by checking the length of the latter 
and:

- if it has length $=n$ it means that all the nodes are in the partial circuit 
so the partial circuit can be closed by adding the starting node
- otherwise it selects the next node by applying the `selection` function and 
then updates the partial circuit applying the `insertion` function.

Hence `ConstructiveHeuristic` has complexity $O(n*(O(selection) + O(insertion)))$.

Note the fact that the partial circuit is represented by a list of indexes of 
nodes of the given `Graph` and it is closed adding the last node in the head 
of the list; this is done because, with respects to what is specified in the 
specification of the heuristics, the implementation of the algorithms
produces a reversed solution in order to insert nodes in some cases in less 
than $O(n)$ (e.g. in the `NearestNeighbour` heuristic the insertion is 
$O(1)$).
