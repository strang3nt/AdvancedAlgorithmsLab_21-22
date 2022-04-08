module lab1.Graphs

open FSharp.Collections

type Node = int
type Nodes = Node array
type Weight = int

type Edge = int * Weight // int is index of node in the Nodes type
type Edges = Edge array

type AdjList = (int * Edges) array

type Graph = Nodes * AdjList

// assumes inputs are simple graphs
type Graph2 = (Node * Node * Weight) array

let w ((_, l) : Graph) (n1: int) (n2: int) : Weight =
    let (_, edges) = l[n1]
    let (_, w) = Array.find (fun (x, _) -> x = n2) edges
    w
    
let areAdjacent ((_, l): Graph) (n1 : int) (n2 : int) : bool =
    let (_, edges) = l[n1]
    match Array.tryFind (fun (x, _) -> x = n2) edges with 
    | Some (_) -> true
    | None -> false

let addNode ((ns, l) : Graph) (n : Node) : Graph =
    (Array.append ns [| n |], Array.append l [| (ns.Length, Array.empty) |] )

let addEdge ((ns, l) : Graph) (n1 : Node) (n2 : Node) (w : Weight): Graph =
    let idx1 = Array.findIndex (fun x -> x = n1) ns
    let idx2 = Array.findIndex (fun x -> x = n2) ns
    let edge1 = idx1, w
    let edge2 = idx2, w 
    let (_, oldAdjEdge1) = l[idx1]
    let (_, oldAdjEdge2) = l[idx2]
    (
        ns, 
        (Array.updateAt idx1 (idx1, (Array.append oldAdjEdge1 [| edge1 |])) l) |> (Array.updateAt idx2 (idx2, (Array.append oldAdjEdge2 [| edge2 |])))
    )

let nodeExists ((ns, _) : Graph) (n : Node) : bool =
    match Array.tryFind(fun x -> x = n) ns with
    | Some (_) -> true
    | _ -> false
    
let getNodes (G: Graph2) =
    G   |> Array.map (fun (u, v, _) -> [|u; v|])
        |> Array.collect id
        |> Array.distinct