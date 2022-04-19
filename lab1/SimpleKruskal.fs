module lab1.SimpleKruskal

open Graphs

type Visit =
    | Visited
    | NotVisited

type EdgeVisit = Visit
type NodeVisit = Visit

let rec cycleDfs v (A: bool array) (Graph (_,_, adj) as G) (nv: Visit array) (ev: Visit array): bool = 
    nv[v] <- Visited
    List.forall (fun e -> 
        if (ev[e] = NotVisited && A[e]) then
            let u = opposite v e G
            if (nv[u] = NotVisited) then
                ev[e] <- Visited
                (cycleDfs u A G nv ev)
            else false
        else true
        ) adj[v]

let isAcyclical e (A: bool array) (Graph (ns, es, adj) as G) : bool =
    let (n1, _, _) = es[e]
    A[e] <- true
    let nv = Array.create ns.Length NotVisited
    let ev = Array.create es.Length NotVisited
    let res = cycleDfs n1 A G nv ev
    A[e] <- false
    res
    
let simpleKruskal (Graph (_, es, _) as G) = 
    let A = Array.create es.Length false
    sortedEdges G 
    |> Array.fold (
        fun acc e -> 
            match e with 
            | e when (isAcyclical e A G) -> A[e] <- true; es[e] :: acc 
            | _ -> acc 
        ) List.empty
