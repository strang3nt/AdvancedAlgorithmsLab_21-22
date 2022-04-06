module lab1.Graphs

open FSharp.Collections

type Nodes = int array
type Weight = int
type Edges = (int * int * Weight) array
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

let totalWeight ( Graph (_, e, _) ) : int =
    Array.sumBy (fun (_, _, w) -> w) e

let private searchNode n (Graph (ns, _, _)) = 
    Array.findIndex (fun x -> n = x) ns

let private setEdge n1 n2 w i (Graph (ns, es, adj) as g) : unit =
    let idx1 = searchNode n1 g
    let idx2 = searchNode n2 g
    Array.set adj idx1 (es.Length :: adj[idx1]) 
    Array.set adj idx1 (es.Length :: adj[idx2])
    Array.set es i (idx1, idx2, w)

let buildGraph edges (sizes : int array) : Graph =    
    let nodes = Array.fold (fun acc (e : int array) -> Set.add (e[0]) acc |> Set.add(e[1]) ) Set.empty edges |> Set.toArray
    let g = Graph ( nodes, (Array.create sizes[1] (0,0,0)), (Array.create nodes.Length List.empty) )
    Array.iteri (fun i (e : int array) -> setEdge e[0] e[1] e[2] i g) edges
    g
