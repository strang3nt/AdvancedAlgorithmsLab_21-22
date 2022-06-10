# Originalities

## Persistent Karger

```fsharp

[<Struct>]
type DWGrahp = DWGraph of PersistentVector<int> * PersistentHashMap<(int * int), int>

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


// TODO change v retrieval once we have adjacency list in MinCutGraph
let Edge_Select (DWGraph(D, W)) =

    let u = 
        D 
        // builds C
        |> PersistentVector.fold (
            fun acc w -> 
                Array.append 
                    acc
                    [| ((Array.last acc) + w) |]) [| 0 |]
        |> Random_Select

    let v =
        [| for i = 0 to D.Length - 1 do yield W[u, i] |]
        // builds C
        |> Array.fold (
            fun acc w -> 
                Array.append 
                    acc
                    [| ((Array.last acc) + w) |]) [| 0 |]
        |> Random_Select
    
    (u, v)

let Contract_Edge u v (DWGraph (D, W)) =
    
    let D = 
        D
        |> PersistentVector.update u (D[u] + D[v] - ( 2 * W[u, v] ))
        |> PersistentVector.update v 0
    
    let W =
        W
        |> PersistentHashMap.add (u, v) 0
        |> PersistentHashMap.add (v, u) 0
    
    let W = 
        [| 0 .. D.Length - 1 |]
        |> Array.fold (
            fun acc w ->
                if w <> u && w <> v then
                    acc
                    |> PersistentHashMap.add (w, u)(W[u, w] + W[v, w])
                    |> PersistentHashMap.add (u, w) (W[w, u] + W[w, v])
                    |> PersistentHashMap.add (v, w) 0
                    |> PersistentHashMap.add (w, v) 0
                else acc
        ) W

    DWGraph (D, W)

let Contract (DWGraph (D, _) as G) k =
    let n = PersistentVector.fold (fun acc v -> if v > 0 then acc + 1 else acc) 0 D
    let mutable G = G
    for _ = 1 to n - k do
        let (u, v) = Edge_Select G
        G <- Contract_Edge u v G
    G

let rec Recursive_Contract (DWGraph (D, W) as G) =
    let n = PersistentVector.fold (fun acc v -> if v > 0 then acc + 1 else acc) 0 D
    if n <= 6 then
        let (DWGraph (_, W)) = Contract G 2
        W
        |> PersistentHashMap.toSeq
        |> Seq.map snd
        |> Seq.find (fun w -> w > 0) //return weight of the only edge (u, v) in G to check this
        |> fun r -> r, DateTime.Now.Ticks
    else
        let t = int (ceil ( float n / Math.Sqrt(2) + 1.0 ))
        [| for _ = 1 to 2 do yield (Contract G t) |> Recursive_Contract |] // should pass a copy
        |> Array.min

let Karger (MinCutGraph (_, _, _, D, W)) k =
    
    // preparatory operations
    let t_start = DateTime.Now.Ticks
    let G = DWGraph (
        D |> PersistentVector.ofSeq,
        W |> Array2D.mapi (fun u v w -> (u, v), w) |> Seq.cast<(int * int) * int> |> PersistentHashMap.ofSeq
    )

    // # of edges of the supposedly min cut
    [| 1..k |]
    |> Array.map (fun _ -> Recursive_Contract G) // can be parallelelized!
    |> Array.min
    |> fun (r, t_end) -> (r, t_end - t_start)

```

One of the ideas we had during Karger implementation was to use persistent structures: the main reason why we
though of this solution is that the most expensive operation we do in Karger-Stein is `D`, `W` copy. 
Using a persistent structure would have removed the need of creating copies by having an instance of a collection that is
bound to the current scope.

This should have significantly lowered the total complexity, as all operations involving `PersistentVector` and `PersistentHashMap` yield a _theoretical_ cost same as `Array` and `Array2D`. This wasn't the case though: theoretical cost is "for all practical purposes" $O(1)$ but the real cost is `$O(\log_32(n)$` (both structures are implemented using trees).
Performances are much slower compared to normal arrays, mainly due to a slower lookup.

For reference we report the links to the documentation:

 - [PersistentVector](https://fsprojects.github.io/FSharpx.Collections/PersistentVector.html)
 - [PersistentHashMap](https://fsprojects.github.io/FSharpx.Collections/PersistentHashMap.html).

## Parallel Karger

```fsharp
let Karger (MinCutGraph (_, _, _, D, W)) k =
    
    let t_start = DateTime.Now.Ticks
    // # of edges of the supposedly min cut
    [| 1..k |]
    |> Array.Parallel.map (fun _ -> Recursive_Contract(DWGraph (D, W, D.Length))) // can be parallelelized!
    |> Array.min
    |> fun (weight, t_end) -> (weight, t_end - t_start)

```

We sketch a theoretical complexity of this parallel version of Karger-Stein.
Normal Karger-Stein algorithm has a $O(n^2\log^3(n))$. If we have `p` processors in our machine then
total complexity becomes:

 - `Recursive_Contract` is executed $\log^2(n)$ times sequentially, now that we have `p` processors
   the number of executions "lowers" to $\log^2(n)/\ p$
 - the total complexity becomes $O(n^2\log^3(n)/\ p)$.
