\newpage

## Nearest Neighbour heuristic

Thanks to the implementation of the function `ConstructiveHeuristic` the 
definition of the Nearest Neighbour heuristic is the following:

```fsharp
let nearestNeighbourHeuristic G = 
    ConstructiveHeuristic initialisation selection insertion G
```

Which means that for implementing it we only have to define the three 
functions `initialisation`, `selection` and `insertion` specific for the 
heuristic.

The `initialisation` function, which initialises the 
partial circuit and the support data structure is implemented as follows:

```fsharp
// Initialise the list of indexes of the partialCircuit with 
// the index of the first node of the given graph and create 
// a dictionary used to store information about the already 
// visited nodes in O(n)
let initialisation (Graph (V,_,_)) =
    match V with
    | [||] -> failwith "Invalid graph"
    | _ ->
        let visitedMap = [ for v in 0..V.Length-1 -> v, false] 
            |> dict 
            |> Dictionary

        visitedMap[0] <- true
        [0], visitedMap
```

it has a time complexity of $O(n)$ because it has to initialise the support 
structure used for tracking the nodes already inserted in the partial circuit, 
which enables faster operations later on.

The `selection` function instead has to select one node of the graph that 
respects a property that is heuristic specific and in the case of the Nearest 
Neighbour's it's implemented as follows:

```fsharp
let selection (Graph (_, _, adjList) as G) 
              (partialCircuitIndexes: int list) 
              (visitedMap: Dictionary<int, bool>) =
//  Gets the index of the last inserted node in the 
//  partial circuit
    let lastNodeIdx = partialCircuitIndexes |> List.head
//  Selects the adjacency list of the last node inserted in 
//  the partial circuit with complexity O(1)
    adjList[lastNodeIdx]
//  Returns the index of the first edge where the opposite 
//  node isn't in the partialCircuit, which means that, given 
//  the fact that the adjacency list is sorted in ascending 
//  order by weight, the node opposite node is the one with
//  minimum weight that must be selected.
//  The complexity of the method is O(n)
    |> List.find (fun e ->
        let uIdx = opposite lastNodeIdx e G
        not (visitedMap[uIdx])
    )
//  Returns the index of the opposite node of the edge with 
//  minimum weight from the last inserted node in O(1)
    |> (fun e ->
        opposite lastNodeIdx e G
    )
```

its overall complexity is $O(n)$ because in the pipeline of computations the 
most complex operation is `List.find`, which has complexity $O(n)$.

The `insertion` function instead has the task of inserting the selected node 
in the partial circuit following the indications of the heuristic and in this 
case it's implemented as follows:

```fsharp
// Inserts the given node in the head of the partial circuit 
// instead of appending it as specified by the actual heuristic, 
// in order to execute the operation in O(1) and then updates 
// the visited map setting as visited the inserted node with 
// complexity O(1)
let insertion vIdx 
              partialCircuitIndexes 
              (visitedMap: Dictionary<int, bool>) =
    visitedMap[vIdx] <- true
    vIdx :: partialCircuitIndexes, visitedMap
```

The function has complexity $O(1)$ because both the update of `visitedMap` 
and the insertion in the head of a list are operations with time complexity 
$O(1)$.

In the end the function `NearestNeighbourHeuristic`, with respect to 
`ConstructiveHeuristic`, has time complexity 
$O(O(initialisation) + O(n * (O(selection) + O(insertion)))) = O(n + n*(n+1)) 
= O(n + n^2) = O(n^2)$, with an hidden constant $c\approx 2.4$ as can be seen 
in \ref{fig:ONearestNeighbour} 

![Time complexity of NearestNeighbourHeuristic \label{fig:ONearestNeighbour}]{img/ONearestNeighbour.png}

### Variations

In order to reach the current time complexity we iterated through different 
implementations and for each of them we reported their performances.

![Comparison of performances of different implementations of Nearest Neighbour heuristic 1\label{fig:NearestNeighbourComparison1}]{img/NNComparison1.png}

The first one, the most naive, simply followed step by step the heuristic 
definition, without the use of the visited map and the use of the functions 
`List.filter` with a $O(log n)$ lamba expression and `List.minBy`, resulting 
in a total time complexity of $O(n^2 log n)$.

![Comparison of performances of different implementations of Nearest Neighbour heuristic 2\label{fig:NearestNeighbourComparison2}]{img/NNComparison2.png}

The second implementation had a `Map` data structure to keep track of the 
already visited nodes and the function `selection` used it in order to have a 
more performant lambda expression, achieving a $O(n^2)$ time complexity.

By better studying the dotnet collections, we found out that using a 
`Dictionary` data structure for keeping track of the visited nodes should have 
given better results and sorting the adjacency list when the graph was built 
should have reduced the hidden constant as well and we got small improvements.

An interesting fact was that the same implementation of the heuristic, without 
sorting the adjacency list when constructing the graph, was substantially 
faster because of the concatenation of non terminal pipeline operators, 
instead of the alternated use of terminal and non terminal ones (`List.filter|>List.head|>(fun u -> ...)`)

At last we went back to constructing graphs with a sorted adjacency list with 
respect to the weight of the edges and we changed the pipeline, with the use of 
`List.find` instead of `List.filter |> List.minBy` resulting in a significant 
improvement on run times as can be seen by \ref{fig:NearestNeighbourComparison2}
