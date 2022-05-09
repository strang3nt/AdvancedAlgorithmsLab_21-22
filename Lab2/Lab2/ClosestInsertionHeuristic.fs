module Lab2.ClosestInsertionHeuristic

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
let private getEdge (adjList: AdjList) (Graph(V,E,l)) u v = if u = v then 0 else let _,_,w = adjList[u] |> List.find (findEdge E u v) |> Array.get E in w

// Find (i,j), k s.t. they minimize w(i,k) + w(j,k) - w(i,j)
let selection (Graph (V,E,adjList)) (partialCircuit: Node list) =
    let rest = V |> Array.filter (fun x -> not (List.contains x partialCircuit))
    let _, k = rest
                |> Array.map (fun k -> 
                    adjList[k-1]
                    |> List.map (fun e -> let i,_,w = E[e] in w, i)
                    |> List.minBy (fun (w,_) -> w))
                |> Array.minBy (fun (w,_) -> w)
    k

type EdgeType = IJ | JK | Sum | NotRelated

let decreaseIndex i k = 
    if i = 0 then 0
    else if i <> k then
            i - 1
         else if k - 2 <= 0 then 0
            else i - 2 

// Insert k between i and j
let insertion partialCircuit k (Graph (V,E,adjList))=
    let circuitArray = partialCircuit |> Array.ofList
    let _, i = 
        circuitArray
        |> Array.mapi (fun i -> fun v -> (v, circuitArray[decreaseIndex i k]))
        //|> Array.removeAt 0
        |> Array.map (fun (j, i) -> 
            let w_ik = getEdge adjList (Graph (V,E,adjList)) i k
            let _,_,_, w_jk_ij = adjList[j]
                                |> List.map (fun e -> 
                                    let x,y,w = E[e]
                                    match x,y with
                                    | (x,y) when (x = i) || (y = i) -> IJ, i, j, w
                                    | (x,y) when (x = k) || (y = k) -> JK, j, k, w
                                    | _ -> NotRelated, 0, 0, 0)
                                |> List.reduce (fun (_,_,_,w) -> fun e ->
                                    match e with
                                    | IJ, i, j, w_ij -> Sum, i, j, w - w_ij
                                    | JK, j, k, w_jk -> Sum, j, k, w + w_jk
                                    | Sum,_,_,w
                                    | NotRelated,_,_,w -> Sum, 0,0, w)
            w_jk_ij + w_ik, i)
        |> Array.minBy (fun (w,_) -> w)

    let circ1, circ2 = 
        partialCircuit
        |> List.findIndex (fun x -> x = i)
        |> List.splitAt <| partialCircuit
    circ1 @ [k] @ circ2

let ClosestInsertionHeuristic G = ConstructiveHeuristic initialisation selection insertion G