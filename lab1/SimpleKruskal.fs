module lab1.SimpleKruskal

open Graphs

type Visit =
    | Visited
    | NotVisited

let rec cycleDfs v (A: bool array) (Graph (_,_, adj) as G) (nv: Visit array) (ev: Visit array): bool = 
    nv[v] <- Visited
    List.forall (fun e ->                   // forall adjacent nodes to v
        if (ev[e] = NotVisited && A[e]) then                //   if edge e is not visited and it is part of MST
            let u = opposite v e G             //     get u, adjacent node of v in edge e
            if (nv[u] = NotVisited) then                    //     if u is not visited
                ev[e] <- Visited                            //       set edge (u, v) as visited and continue
                (cycleDfs u A G nv ev)
            else false                                      //     else we have found a backedge, thus a cycle
        else true
        ) adj[v]

let isAcyclical e (A: bool array) (Graph (ns, es, adj) as G) : bool =
    let (n1, _, _) = es[e]
    A[e] <- true                                                        // suppose edge to check e is part of MST
    let nv = Array.create ns.Length NotVisited
    let ev = Array.create es.Length NotVisited
    let res = cycleDfs n1 A G nv ev
    A[e] <- false                                                       // remove edge from MST, it will eventually be added in main funciton
    res
    
let simpleKruskal (Graph (_, es, _) as G) = 
    let A = Array.create es.Length false                        // Init A, it will hold the mst
    sortedEdges G                                                                     // sort edges in non decreasing order
    |> Array.fold (                                                           // iterate over edges
        fun acc e ->
            match e with 
            | e when (isAcyclical e A G) -> A[e] <- true; es[e] :: acc   // if adding edge to MST doesn't make it acyclical, add edge
            | _ -> acc                                                                // else don't add edge
        ) List.empty
