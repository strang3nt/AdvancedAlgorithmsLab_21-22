module Lab3.KargerStein

open Graph
open System


type DWGraph = DWGraph of (int array * int array * int)


let binarySearch (C: int array) r : int =
    // find i such that C[i - 1] <= r < C[i]
    let rec binarySearchAux (C: int array) r L R =
        // there is no L <= R check, value is supposed to always be found
        let m = int (floor ((float (L + R)) / 2.0))
        if C[m] <= r && C[m + 1] > r then
            m + 1
        else if C[m] <= r then
            binarySearchAux C r (m + 1) R
        else // C[m] > r
            binarySearchAux C r L (m - 1)
    binarySearchAux C r 0 (C.Length - 1)


let Random_Select (C: int array) =
    let rnd = Random()
    let r = rnd.Next( (Array.last C) )
    (binarySearch C r) - 1


let Edge_Select (DWGraph (D, W, _)) =
    let mutable cumulativeSum = 0
    let u = 
        Array.init 
            (D.Length + 1) 
            (fun i -> if i = 0 then 0 else cumulativeSum <- cumulativeSum + D[i - 1]; cumulativeSum)
        |> Random_Select

    cumulativeSum <- 0
    let v =
        Array.init 
            (D.Length + 1) 
            (fun i -> if i = 0 then 0 else cumulativeSum <- cumulativeSum + W[D.Length * u + i - 1]; cumulativeSum)
        |> Random_Select

    (u, v)

let Contract_Edge u v (DWGraph (D, W, n)) =
    D[u] <- D[u] + D[v] - 2 * W[D.Length * u + v]
    D[v] <- 0
    W[D.Length * u + v] <- 0
    W[D.Length * v + u] <- 0
    for w = 0 to D.Length - 1 do
        if w <> u && w <> v then
            W[D.Length * u + w] <- W[D.Length * u + w] + W[D.Length * v + w]
            W[D.Length * w + u] <- W[D.Length * w + u] + W[D.Length * w + v]
            W[D.Length * v + w] <- 0
            W[D.Length * w + v] <- 0

let Contract (DWGraph (D, W, n) as G) k =
    for _ = 1 to n - k do
        let (u, v) = Edge_Select G
        Contract_Edge u v G
    DWGraph(D, W, k)

let rec Recursive_Contract (DWGraph (D, W, n) as G) =
    if n <= 6 then
        let DWGraph (_, W, _) as _ = Contract G 2
        //returns weight of the only edge (u, v) in G
        W |> Array.find (fun w -> w > 0) |> fun weight -> (weight, DateTime.Now.Ticks)
    else
        let D_2 = Array.copy<int> D
        let W_2 = Array.copy<int> W
        let t = int (ceil ( float n / Math.Sqrt(2) + 1.0 ))
        min 
            (Contract (DWGraph (D, W, n)) t |> Recursive_Contract)
            (Contract (DWGraph (D_2, W_2, n)) t |> Recursive_Contract)
        
let Karger (MinCutGraph (_, _, _, D, W)) k =
    
    let t_start = DateTime.Now.Ticks
    let W =  W |> Seq.cast<int> |> Seq.toArray

    // # of edges of the supposedly min cut
    [| 1..k |]
    |> Array.map (fun _ -> Recursive_Contract(DWGraph ((Array.copy<int> D), (Array.copy<int> W), D.Length))) // can be parallelelized!
    |> Array.min
    |> fun (weight, t_end) -> (weight, t_end - t_start)
