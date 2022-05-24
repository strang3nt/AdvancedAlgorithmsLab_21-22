module Lab3.KargerStein

open Graph
open System

let binarySearch (C: int array) r : int =

    // find i such that C[i - 1] <= r < C[i]
    // TODO: implement binary search
    [| 1..C.Length - 1 |]
    |> Array.findIndex (fun i -> C[i - 1] <= r && r < C[i])

let Random_Select (C: int array) =
    let rnd = Random()
    let r = rnd.Next( (Array.last C) )
    binarySearch C r

// TODO change v retrieval once we have adjacency list in MinCutGraph
let Edge_Select (MinCutGraph (_, _, _, D, W) as G) =
    
    let u = 
        D 
        // builds C
        |> Array.scan (+) 0
        |> Random_Select

    let v =
        [| for i = 0 to D.Length - 1 do yield W[u, i] |]
        // builds C
        |> Array.scan (+) 0
        |> Random_Select
    
    (u, v)

let Contract_Edge u v (MinCutGraph (V, _, _, D, W: W) as G) =
    D[u] <- D[u] + D[v] - 2 * W[u, v]
    D[v] <- 0
    W[u, v] <- 0
    W[v, u] <- 0
    for w = 0 to V.Length - 1 do
        if w <> u && w <> v then
            W[u, w] <- W[u, w] + W[v, w]
            W[w, u] <- W[w, u] + W[w, v]
            W[v, w] <- 0
            W[w, v] <- 0

let Contract (MinCutGraph (V, _, _, D, _) as G) k =
    let n = Array.filter (fun v -> v > 0) D |> Array.length
    for _ = 1 to n - k do
        let (u, v) = Edge_Select G
        Contract_Edge u v G
    G

let rec Recursive_Contract (MinCutGraph (_, _, _, D, W) as G) =
    let n = Array.filter (fun v -> v > 0) D |> Array.length
    if n <= 6 then
        let MinCutGraph (_, _, _, _, W) as G = Contract G 2
        W
        |> Seq.cast<int>
        |> Seq.find (fun w -> w > 0) //return weight of the only edge (u, v) in G to check this
    else 
        let t = int (ceil ( float n / Math.Sqrt(2) + 1.0 ))
        [| for i = 1 to 2 do yield (Contract G t) |> Recursive_Contract |]
        |> Array.min
        
let Karger (MinCutGraph (V, E, adj, D, W)) k =
    
    // # of edges of the supposedly min cut
    [| 1..k |]
    |> Array.map (fun _ -> Recursive_Contract(MinCutGraph (V, E, adj, (Array.copy D), (Array2D.copy W)))) // can be parallelelized!
    |> Array.min
