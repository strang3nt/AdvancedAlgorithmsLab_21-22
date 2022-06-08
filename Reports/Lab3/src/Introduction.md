# Introduction

The task is to build the implementations of:

 - Karger-Stein random algorithm for Min Cuts
 - Stoer-Wagner deterministic algorithm.

The language of choice is again `F#`.

## Source code structure

The directory `Lab3/Lab3` contains the source code for the third assignment.
This is how we organized the source code directory:

 - `Graph.fs` and `Parsing.fs` contain the graph abstraction and the parsing mechanism
 - `Lab3/Lab3/dataset` contains all the graphs to be parsed
 - `FibonacciHeap.fs` and `StoerWagner.fs` both contain Stoer-Wagner's implementation
 - `Karger.fs` is Karger's implementation.

## Graph data structure

```fsharp
// 2 dimensional array
type W = int[,]
type D = int array
[<Struct>]
type MinCutGraph = MinCutGraph of Nodes * Edges * AdjList * D * W
```

The structure changes slightly from the previous assignments, while we still have
the adjacency list, list of nodes and list of edges we added the arrays D and W.
W is ment to be the adjacency matrix, in fact it's a 2 dimensional array.
D is the array that contains the weights of all edges adjacent to a node: for example
`D[i]` contains the weight of all edges adjacent to the node `Nodes[i]`.
