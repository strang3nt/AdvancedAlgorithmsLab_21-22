module lab1.Prim

open lab1.Graphs
open FSharpx.Collections
open System


let prim (s: Node) (g : Graph) : Edge list =
    let nodes, edges, adjLs = match g with Graph (n, e, l) -> n, e, l
    let keys : Weight array = Array.create nodes.Length Int32.MaxValue
    let parents = Array.create nodes.Length None
    let mutable graph (*: Edges*) = List.empty
    let zipped = Array.zip keys nodes
    let mutable queue = Array.toSeq zipped |> Heap.ofSeq false

    let updateWeigths (uIdx: int) (u: Node) (k: Weight) state (edgeIndex: int) =
        let (a, b, w) = edges[edgeIndex]
        let vIdx = if a = uIdx then b else a
        let v = nodes[vIdx]
        let mutable newQueue = Heap.empty false
        while not queue.IsEmpty do
            let (ck, cv), tail = queue.Uncons ()
            queue <- tail
            if cv = v then
                newQueue <- 
                    if w < ck then
                        Array.set parents vIdx (Some uIdx)
                        newQueue.Insert (w, cv)
                    else newQueue.Insert (ck, cv)
            else
                newQueue <- newQueue.Insert (ck, cv)
        
        queue <- newQueue
        state

    while not queue.IsEmpty do
        let (k, u), tail = queue.Uncons()
        queue <- tail
        let uIdx = searchNode u g
        let list = Array.get adjLs uIdx
        let updateWeigthsOfU = updateWeigths uIdx u k
        List.fold updateWeigthsOfU List.empty list
        |> ignore
        graph <-
            match (Array.get parents uIdx) with | None -> [] | Some p -> [p, uIdx, k]
            |> List.append graph

    graph
    

    
    

