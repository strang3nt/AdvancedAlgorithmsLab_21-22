module lab1.Graphs

open System
open FSharp.Collections

type Node = int
type Nodes = Node array
type Weight = int
type Edge = int * int * Weight // refs to the nodes, not actual nodes
type Edges = Edge array
type AdjList = ( int list ) array // each node has its own adjacency list which is only a ref to edges

[<Struct>]
type Graph = Graph of Nodes * Edges * AdjList

let areAdjacent (n1 : int) (n2 : int) (Graph (_, es, adj)) : bool =
    match List.tryFind (fun x -> let (x, y, _) = es[x] in  n2 = x || n2 = y) adj[n1] with 
    | Some (_) -> true
    | None -> false

let w n1 n2 (Graph (_, es, adj) as g) : Weight Option =
    if (areAdjacent n1 n2 g) then
        let edgeIndex = List.find (fun x -> let (x, y, _) = es[x] in  n2 = x || n2 = y) adj[n1]
        let (_, _, w) = es[edgeIndex]
        Some w
    else None

let totalWeight ( Graph (_, es, _) ) : int =
    Array.sumBy (fun (_, _, w) -> w) es

let searchNode n (Graph (ns, _, _)) = 
    Array.findIndex (fun x -> n = x) ns

let private setEdge n1 n2 w i (Graph (_, es, adj) as g) : unit =
    let idx1 = searchNode n1 g
    let idx2 = searchNode n2 g
    adj[idx1] <- (i :: adj[idx1]) 
    adj[idx2] <- (i :: adj[idx2])
    es[i] <- (idx1, idx2, w)

let buildGraph (edges: int array array) (sizes : int array) : Graph =
    let nodes = Array.create (sizes[1]*2) -1
    Array.Parallel.iteri (fun i (e: int array) -> nodes[i*2] <- e[0]; nodes[i*2+1] <- e[1]) edges
    let nodes = nodes |> Array.distinct
    let g = Graph ( nodes, (Array.create sizes[1] (0,0,0)), (Array.create nodes.Length List.empty) )
    Array.iteri (fun i (e : int array) -> setEdge e[0] e[1] e[2] i g) edges
    g

let sortedEdges ( Graph (_, es, _) ) : int array =
    let sortedArr = Array.mapi ( fun x (_, _, w) -> ( w , x ) ) es
    Array.Sort sortedArr
    Array.map ( fun (_, x) -> x ) sortedArr

let opposite n e ( Graph (_, es, _) ) =
    let (n1, n2, _) = es[e]
    if n1 = n then n2 else n1
