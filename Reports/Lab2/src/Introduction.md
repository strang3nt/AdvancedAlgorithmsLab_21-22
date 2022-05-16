\newpage

# Introduction

The goal of the second assignment is to develop three different algorithms 
solving the Traveling Salesman Problem. We implemented, as per request, a 
2-approx algorithm, namely Metric TSP, and the two constructive heuristics:

 - Nearest Neighbour;
 - Closest Insertion.

Even though we had some doubts (mainly concerning performance) we decided to 
continue using the language `F#`, so that we could reuse most of the code from 
the previous assignment.

## Source code structure

The source code is organized roughly in the same way, with some slight changes 
to the repository structure in order to keep two projects in the same solution 
(or in different terms, two sub-projects within one project). The relevant 
files are:

 - `TspGraph.fs` contains the graph abstraction;
 - `Main.fs` executes all algorithms and prints all necessary data;
 - `Parsing.fs` parses the source files into a `TspGraph` type;
 - `MetricTsp.fs` implements the 2-approximation algorithm;
 - `ConstructiveHeuristic.fs` applies the initialisation, selection and 
 insertion logic, which are shared between the two heuristics;
 - `ClosestInsertionHeuristic.fs` implements the heuristic Closest Insertion;
 - `NearestNeighbourHeuristic.fs` implements the heuristic Nearest Neighbour.

The source code for this assignment is located in `Lab2/Lab2/`, while the 
source code from the previous assignment is in `Lab1/Lab1/`. Part of the code 
from the previous assignment is being reused, as mentioned in the introduction.

## Graph data structure

The graph itself is the same as the structure implemented during the previous 
assignment; from the previous report:

`Graph` is the name of the data structure representing a simple undirected 
graph. Instead of working with references, as one would have done with C-like 
languages, we decided to use indices:

 - `Nodes` is an array of integers; 
 - `Edges` is an array of tuples and contains the index of the two adjacent 
 vertices and the edge's weight; 
 - `AdjList` is an array of lists of integers, with list `i` containing all 
 incident edges of the `i-th` node in `Nodes` array.
     
We slightly changed the adjacency list: its structure remains the same but now 
each list of adjacent edges is ordered in ascending order by weight.
This small change reduces the complexity of the two heuristics which often 
need the least weighted edge adjacent to a certain vertex.

`Graph.fs` contains a few auxiliary functions to build a graph instance and 
to interact with it.

A couple of new functions were added to this file.

```fsharp
type Name = string
type Comment = string
type Dimension = int
type OptimalSolution = int

[<Struct>]
type TspGraph = 
    TspGraph of (Name * Comment * Dimension * OptimalSolution * Graph)
```

The code above represents the new `TspGraph` type, which is basically a Graph 
enriched with the data contained in the files provided, the latter will be 
useful when printing the results. 
