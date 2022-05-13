\newpage

# Introduction

The goal of this second assignment is to build three different algorithms solving the Traveling
Salesman Problem, we implemented as per request a 2-approx algorithm and 2 heuristics:

 - Nearest Neighbor
 - Closest Insertion.

Even though we had some doubts (mainly concerning performance) we decided to continue using the language `F#`,
we could reuse most of the code from the previous assignment.

## Source code structure

The source code is organized roughly in the same way, we only slightly changed the repository structure in order to
keep 2 projects in the same solution (or in different terms, 2 sub-projects within one project). The relevant files 
are:

 - `TspGraph.fs` contains the graph abstraction
 - `Main.fs` executes all algorithms and prints all necessary data
 - `Parsing.fs` parses the source files into a `TspGraph` type
 - `MetricTsp.fs` implements the 2-approximation
 - `ClosestInsertionHeuristic.fs` implements the heuristic Closest Insertion
 - `ConstructiveHeuristic.fs` applies the initialization, selection and insertion logic, which is shared between the 2 heuristics
 - `NearestNeighbourHeuristic.fs` implements the heuristic Nearest Neighbor.

The source code for this assignment is located in Lab2/Lab2/, while the source code from the previous assignment is in Lab1/Lab1. We
use code from the previous assignment.

## Graph data structure

The Graph itself is the same as the structure implemented during the previous assignment, from the previous report:

    `Graph` is the name of the data structure representing a simple undirected 
    graph. Instead of working with references, as one would have done with C-like 
    languages, we decided to use indices:

     - `Nodes` is an array of integers; 
     - `Edges` is an array of tuples and contains the index of the two adjacent 
     vertices and the edge's weight; 
     - `AdjList` is an array of lists of integers, with list `i` containing all 
     incident edges of the `i-th` node in `Nodes` array.
     
We slightly changed the adjacency list: its structure remains the same but now each list of adjacent edges is ordered in ascending order by weight.
This small change reduces the complexity of the 2 heuristics which often need the least weighted edge adjacent to a certain vertex.

    `Graph.fs` contains a few auxiliary functions to build a graph instance and 
    to interact with it.

```fsharp
type Name = string
type Comment = string
type Dimension = int
type OptimalSolution = int

[<Struct>]
type TspGraph = TspGraph of (Name * Comment * Dimension * OptimalSolution * Graph)
```

The code above represents the new TspGraph type, which is basically a Graph enriched with the data contained
in the files provided, the latter will be useful when printing the results. 
