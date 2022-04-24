module lab1.SimpleKruskal

open Graphs

type Visit =
    | Visited
    | NotVisited

type MST = 
    MST of int list * AdjList

let rec cycleDfs v (G: Graph) (MST (_, adj) as A : MST) (nv: Visit array) (ev: Visit array): bool = 
    nv[v] <- Visited

    // forall adjacent nodes to v
    List.forall (fun e ->

        // if edge e is not visited and it is part of MST
        if (ev[e] = NotVisited) then

            // get u, adjacent node of v in edge e
            let u = opposite v e G

            // if u is not visited
            if (nv[u] = NotVisited) then

                // set edge (u, v) as visited and continue
                ev[e] <- Visited
                (cycleDfs u G A nv ev)

            // else we have found a backedge, thus a cycle
            else false
        else true
        ) adj[v]

/// makes side effects
let updateMst e (MST (mstEdges, mstAdj): MST) (Graph (_, es, _): Graph): MST =
    let (v, u, _) = es[e]
    mstAdj[v] <- e :: mstAdj[v]
    mstAdj[u] <- e :: mstAdj[u]
    MST (e :: mstEdges, mstAdj)

let isAcyclical e (MST (edges, adjList)) (Graph (_, es, _) as G : Graph): bool =
    
    // suppose edge to check e is part of MST and add it to MST
    let adj = Array.copy adjList
    let A = updateMst e (MST (edges, adj)) G
    
    // init auxiliary arrays nodes visit and edges visit
    let nv = Array.create adj.Length NotVisited
    let ev = Array.create es.Length NotVisited
    
    let (v, _, _) = es[e]
    let res = cycleDfs v G A nv ev
    res
    
let simpleKruskal (Graph (ns, es, _) as G) =

    // Init A, it will hold the mst
    let mstEdges = List.empty
    let mstAdj = Array.create ns.Length List.empty
    let A = MST (mstEdges, mstAdj)

    // sort edges in non decreasing order
    sortedEdges G

    // iterate over edges
    |> Array.fold (
        fun acc e ->
            match e with

            // if adding edge to MST doesn't make it acyclical, add edge
            | e when (isAcyclical e A G) -> 
                let A = updateMst e A G
                es[e] :: acc

            // else don't add edge
            | _ -> acc
        ) List.empty
