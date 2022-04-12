module lab1.SimpleKruskal

open Graphs
open System.Collections.Generic

type Visit =
    | Visited
    | NotVisited

type EdgeVisit = Visit
type NodeVisit = Visit

type DFSGraph = 
    DFSGraph of Graph * NodeVisit array * EdgeVisit array

let cycleDetectDfs v (A: bool array) (DFSGraph (Graph (_, es, adj) as G, nv, ev) as dfsG : DFSGraph) : bool = 
    let stack = new Stack<int>()
    stack.Push(v)
    let mutable notCycle = true
    while (stack.Count <> 0 && notCycle) do
        let s = stack.Pop()
        if(nv[s] = NotVisited) then
            nv[s] <- Visited
        else  notCycle <- false
   
        for e in adj[s] do
            if A[e] && (ev[e] = NotVisited) then         
                let u = opposite s e G
                if nv[u] = NotVisited then
                    ev[e] <- Visited
                    stack.Push(u)

    notCycle

let isAcyclical e (A: bool array) (Graph (ns, es, _) as G) : bool =
    let (n1, _, _) = es[e]
    A[e] <- true
    let nv = Array.create ns.Length NotVisited
    let ev = Array.create es.Length NotVisited

    let res = cycleDetectDfs n1 A (DFSGraph (G, nv, ev))
    A[e] <- false
    res
    
let simpleKruskal (Graph (_, es, _) as G) = 
    let A = Array.create es.Length false
    sortedEdges G 
    |> Array.fold (
        fun acc e -> 
            if (isAcyclical e A G) 
            then 
                A[e] <- true 
                es[e] :: acc 
            else acc 
        ) List.empty
