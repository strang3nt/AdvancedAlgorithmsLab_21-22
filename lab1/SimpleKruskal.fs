module lab1.SimpleKruskal

open Graphs

type Visit =
    | Visited
    | NotVisited

let rec cycleDfs v (A: bool array) (Graph (_,_, adj) as G) (nv: Visit array) (ev: Visit array): bool = 
    nv[v] <- Visited

    // forall adjacent nodes to v
    List.forall (fun e ->

        // if edge e is not visited and it is part of MST
        if (ev[e] = NotVisited && A[e]) then

            // get u, adjacent node of v in edge e
            let u = opposite v e G

            // if u is not visited
            if (nv[u] = NotVisited) then

                // set edge (u, v) as visited and continue
                ev[e] <- Visited
                (cycleDfs u A G nv ev)

            // else we have found a backedge, thus a cycle
            else false
        else true
        ) adj[v]

let isAcyclical e (A: bool array) (Graph (ns, es, adj) as G) : bool =
    let (n1, _, _) = es[e]

    // suppose edge to check e is part of MST
    A[e] <- true

    // init auxiliary arrays nodes visit and edges visit
    let nv = Array.create ns.Length NotVisited
    let ev = Array.create es.Length NotVisited
    let res = cycleDfs n1 A G nv ev

    // remove edge from MST, it will eventually be added in main funciton
    A[e] <- false
    res
    
let simpleKruskal (Graph (_, es, _) as G) =

    // Init A, it will hold the mst
    let A = Array.create es.Length false

    // sort edges in non decreasing order
    sortedEdges G

    // iterate over edges
    |> Array.fold (
        fun acc e ->
            match e with

            // if adding edge to MST doesn't make it acyclical, add edge
            | e when (isAcyclical e A G) -> A[e] <- true; es[e] :: acc

            // else don't add edge
            | _ -> acc
        ) List.empty
