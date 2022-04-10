module lab1.SimpleKruskal

open Graphs

type Visit =
    | Visited
    | NotVisited

type EdgeVisit = Visit
type NodeVisit = Visit

type DFSGraph = 
    DFSGraph of Graph * NodeVisit array * EdgeVisit array

let rec cycleDetectDfs v (A: bool array) (DFSGraph (Graph (_, _, adj) as G, nv, ev) as dfsG : DFSGraph) : bool = 
    nv[v] <- Visited
    List.forall (fun e -> 
        if (ev[e] = NotVisited && A[e]) then
            let u = opposite v e G
            if (nv[u] = NotVisited) then
                ev[e] <- Visited
                (cycleDetectDfs u A dfsG)
            else false
        else true
        ) adj[v]

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
