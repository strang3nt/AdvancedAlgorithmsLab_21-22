module lab1.Graph

open FSharp.Collections

type Node = int
type Nodes = Node array
type Weight = int

type Edge = int * Weight // int is index of node in the Nodes type
type Edges = Edge array

type AdjList = Edges array

type Graph = Nodes * AdjList

let w ((_, l) : Graph) (n1: int) (n2: int) : Weight =
    let (_, w) = Array.find (fun (x, _) -> x = n2) l[n1]
    w
    
let areAdjacent ((_, l): Graph) (n1 : int) (n2 : int) : bool =
    match Array.tryFind (fun (x, _) -> x = n2) l[n1] with 
    | Some (_) -> true
    | None -> false

let addNode ((ns, l) : Graph) (n : Node) : Graph =
    (Array.append ns [| n |], l)

let addEdge ((ns, l) : Graph) (n1 : Node) (n2 : Node) (w : Weight): Graph =
    let idx1 = Array.findIndex (fun x -> x = n1) ns
    let idx2 = Array.findIndex (fun x -> x = n2) ns
    let edge1 = idx1, w
    let edge2 = idx2, w
    (
        ns, 
        (Array.updateAt idx1 (Array.append l[idx1] [| edge1 |]) l) |> (Array.updateAt idx2 (Array.append l[idx2] [| edge2 |]))
    )
