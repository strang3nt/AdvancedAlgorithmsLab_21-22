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
let private getEdge (adjList: AdjList) (Graph(V,E,l)) u v = let _,_,w = adjList[u] |> List.find (findEdge E u v) |> Array.get E in w

// Find (i,j), k s.t. they minimize w(i,k) + w(j,k) - w(i,j)
let selection (Graph (V,E,adjList)) (partialCircuit: Node list) =
    let rest = V |> Array.filter (fun x -> not (List.contains x partialCircuit))
    let checkNode (prevI, prevK, prevW) currK =
        let final = adjList[currK]
                    |> List.fold (fun prev -> fun e -> // fold on a list of edges' indexes
                        let u, v, currW = E[e]
                        let j = if u = currK then v else u
                        if Array.contains j rest then
                            prev
                        else
                            match prev,j with 
                            | None, _ -> Some (j, j, currK, currW)
                            | Some (i, pos, k, w), j ->
                                let w_ij = getEdge adjList (Graph(V,E,adjList)) j i
                                let w_ik = getEdge adjList (Graph(V,E,adjList)) i currK
                                let w_jk = getEdge adjList (Graph(V,E,adjList)) currK j
                                if w_ik + w_jk - w_ij < w then Some (j, i, currK, w_ik + w_jk - w_ij) else Some (j, pos, k, w)
                        ) None
        match final with 
        | None -> failwith "Algorithm broke"
        | Some (_,i,k,w) -> if prevW > w then i,k,w else prevI, prevK, prevW
    
    let circHeadIdx = V |> Array.findIndex (fun x -> x = List.head partialCircuit)
    let restHeadIdx = V |> Array.findIndex (fun x -> x = Array.head rest)

    let i,k,_ = rest |> Array.mapi (fun i -> fun _ -> i) |> Array.fold checkNode (circHeadIdx, restHeadIdx, 0)
    V[i]// ,V[k]

// Insert k between i and j
let insertion partialCircuit i _ =
    let circ1, circ2 = 
        partialCircuit
        |> List.findIndex (fun x -> x = i)
        |> List.splitAt <| partialCircuit
    circ1 @ [i] @ circ2

let CheapestInsertionHeuristic G = ConstructiveHeuristic initialisation selection insertion G