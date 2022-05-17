# Introduction


## Source code structure

## Graph data structure

```fsharp
type Node = int
type Nodes = int array
type Weight = int
type Edges = (Node * Node * Weight) array
type AdjList = ( int list ) array

[<Struct>]
type Graph = Graph of Nodes * Edges * AdjList
```

`Graph` is the name of the data structure representing a simple undirected 
graph. Instead of working with references, as one would have done with C-like 
languages, we decided to use indices:

 - `Nodes` is an array of integers; 
 - `Edges` is an array of tuples and contains the index of the two adjacent 
 vertices and the edge's weight; 
 - `AdjList` is an array of lists of integers, with list `i` containing all 
incident edges of the `i-th` node in `Nodes` array.

`Graph.fs` contains a few auxiliary functions to build a graph instance and 
to interact with it. Note that we assume a `Graph` instance to be immutable 
even though, for example, `Nodes`, which is an alias for an array of integers, 
is a mutable structure. 
Using immutable structures such as lists would have slowed down graphs 
building and visit significantly. Immutable arrays are not natively supported.
