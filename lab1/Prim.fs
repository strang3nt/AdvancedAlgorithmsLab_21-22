module lab1.Prim

open lab1.Graphs
open FSharpx.Collections
open System


let prim (g : Graph) (s: Node) : Edges = // can change output to create a new graph
    let nodes, _, adjLs = match g with Graph (n, e, l) -> n, e, l
    let keys : Weight array = Array.create nodes.Length Int32.MaxValue
    let mutable graph : Edges = Array.empty
    keys.SetValue (0, s)
    let queue = Array.toSeq nodes |> Heap.ofSeq false
    let mutable queueArray = Heap.toSeq queue |> Array.ofSeq

    let updateWeigths (u: Node) s (v: Node) = 
        let weight = match w u v g with Some weight -> weight
        if Array.contains v queueArray && weight < (Array.get keys v) then 
            Array.set keys v weight
            graph <- Array.append graph [|(u, v, weight)|]
        s

    while not queue.IsEmpty do
        let u = queue.Head
        let list = Array.get adjLs u
        queueArray <- Array.removeAt 0 queueArray
        let updateWeigthsOfU = updateWeigths u
        List.fold updateWeigthsOfU List.empty list
        |> ignore

    graph
    

    
    

