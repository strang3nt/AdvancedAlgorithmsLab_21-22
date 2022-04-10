module lab1.Prim

open lab1.Graphs
open FSharpx.Collections
open System


let prim (s: Node) (g : Graph) : Edges = // TODO: change output to create a new graph
    let nodes, edges, adjLs = match g with Graph (n, e, l) -> n, e, l
    let keys : Weight array = Array.create nodes.Length Int32.MaxValue
    let mutable graph : Edges = Array.empty
    Array.set keys s 0
    let mutable queue = Array.toSeq nodes |> Heap.ofSeq false
    let mutable queueArray = Heap.toSeq queue |> Array.ofSeq
    
    let updateWeigths (u: int) s (edgeIndex: int) =
        let (a, b, w) = edges[edgeIndex]
        let v = if a = u then b else a
        if Array.contains nodes[v] queueArray && w < (Array.get keys v) then 
            Array.set keys v w
            graph <- Array.append graph [|(u, v, w)|]
        s

    while not queue.IsEmpty do
        let u = (searchNode queue.Head g)
        let list = Array.get adjLs u
        let updateWeigthsOfU = updateWeigths u
        List.fold updateWeigthsOfU List.empty list
        |> ignore
        queueArray <- Array.removeAt 0 queueArray
        queue <- queue.Tail()

    graph
    

    
    

