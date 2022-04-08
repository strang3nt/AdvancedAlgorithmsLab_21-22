module lab1.Prim

open lab1.Graphs
open FSharpx.Collections
open System

type Key = int

let prim (g : Graph) (s: Node) : SimpleGraph =
    let (nodes, lists) = g
    let keys : Key array = Array.create nodes.Length Int32.MaxValue
    let mutable graph : SimpleGraph = Array.empty
    keys.SetValue (0, s)
    let queue = Array.toSeq nodes |> Heap.ofSeq false
    let mutable queueArray = Heap.toSeq queue |> Array.ofSeq

    let updateWeigths (u: Node) s ((v, w) : Edge) = 
        if Array.contains v queueArray && w < (Array.get keys v) then 
            Array.set keys v w
            graph <- Array.append graph [|(u, v, w)|]
        s

    while not queue.IsEmpty do
        let u, adjEdg = Array.get lists queue.Head
        queueArray <- Array.removeAt 0 queueArray
        let updateWeigthsOfU = updateWeigths u
        Array.fold updateWeigthsOfU List.empty adjEdg
        |> ignore

    graph
    

    
    

