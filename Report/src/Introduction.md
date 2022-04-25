# Introduction

The assignment required to implement three MST algorithms in a programming language of choice.
The algorithms are: 

 - Naive Kruskal, which from now on will be referred to either as Naive or Simple Kruskal
 - Union-find Kruskal
 - Prim.

We decided to use `F#`: it is a functional language that runs on Microsoft's .NET platform, 
and thus allowing us to use the .NET's API, while also supporting imperative programming, which was heavily used in this assignment.

## Source code structure

The source code is comprised of the following relevant files (or "modules" in `F#` jargon)

 - `Graph.fs` contains the graph abstraction
 - `Main.fs` executes all algorithms and prints all necessary data
 - `Parsing.fs` parses the source files into a `Graph` type 
 - `UnionFind.fs` is the structure used by Union-find Kruskal.
 - `UnionFindKruskal.fs` contains Union-find Kruskal
 - `Prim.fs` contains Prim's algorithm
 - `SimpleKruskal.fs` contains Naive Kruskal's algorithm

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