module Lab2.CheapestInsertionHeuristic

open Lab1.Graphs
open Lab2.ConstructiveHeuristic

// Find edge (0,i) with the minimum weight and make the nodes the first 2 of the circuit 
let initialisation (Graph (V,E,A)) =
    match V, A with
    | [||], _
    | _, [||] -> failwith "Invalid graph"
    | _, _ -> 
        let _, i = A[0]
                |> List.map (fun e -> let x,y,w = E[e] in if x = 0 then w,y else w,x) 
                |> List.min
        V[0] :: V[i] :: List.Empty

let private findEdge (Es: Edges) u v e = let (x,y,_) = Es[e] in (x=u && y=v) || (x=v && y=u)
let private getEdge (adjList: AdjList) E u v = let _,_,w = adjList[u] |> List.find (findEdge E u v) |> Array.get E in w

// Find (i,j), k s.t. they minimize w(i,k) + w(j,k) - w(i,j)
let selection (Graph (V,E,adjList)) (partialCircuit: Node list) =
    let rest = V |> Array.filter (fun x -> not (List.contains x partialCircuit))
    let checkNode (prevI, prevK, prevW) currK =
        let final = adjList[currK]
                    |> List.fold (fun prev -> fun j ->
                        match prev,j with 
                        | None, _ -> Some (j, 0, 0)
                        | Some (i, k, w), j ->
                            let w_ij = getEdge adjList E j i
                            let w_ik = getEdge adjList E i currK
                            let w_jk = getEdge adjList E currK j
                            if w_ik + w_jk - w_ij < w then Some (j, currK, w_ik + w_jk - w_ij) else Some (j, k, w)
                    ) None
        match final with 
        | None -> failwith "Algorithm broke"
        | Some (i,k,w) -> if prevW < w then i,k,w else prevI, prevK, prevW
    let i,k,_ = rest |> Array.fold checkNode (0, 0, 0)
    i,k

// Insert k between i and j
let insertion partialCircuit i k =
    let circ1, circ2 = 
        partialCircuit
        |> List.findIndex (fun x -> x = i)
        |> List.splitAt <| partialCircuit
    circ1 @ [k] @ circ2

let CheapestInsertionHeuristic G = ConstructiveHeuristic initialisation selection insertion G