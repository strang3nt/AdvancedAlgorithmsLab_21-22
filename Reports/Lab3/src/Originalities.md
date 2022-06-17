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

 N	  M	    Time (seconds)	 Instant(seconds)   Time K/PK   Instant K/PK
---- ----- ---------------- ------------------ ----------- --------------
10	    14	    0,030	    0,00797	                0,850	    0,986
10	    10	    0,001	    0,00010	                3,126	    0,780
10	    12	    0,001	    0,00009	                3,448	    0,478
10	    11	    0,001	    0,00012	                3,051	    1,456
20	    24	    0,018	    0,00030	                2,855	    1,095
20	    24	    0,015	    0,00055	                3,389	    0,536
20	    27	    0,012	    0,00045	                4,177	    0,387
20	    25	    0,012	    0,00040	                4,170	    0,444
40	    52	    0,130	    0,00341	                3,179	    2,480
40	    54	    0,097	    0,00096	                4,006	    0,495
40	    51	    0,086	    0,00084	                4,558	    0,576
40	    50	    0,088	    0,00080	                4,510	    0,909
60	    82	    0,511	    0,00141	                4,353	    0,685
60	    72	    0,511	    0,00145	                4,461	    0,577
60	    83	    0,563	    0,00156	                4,313	    0,553
60	    79	    0,532	    0,00145	                4,705	    0,628
80	    101	    0,684	    0,00218	                5,502	    0,666
80	    105	    0,698	    0,00228	                5,288	    0,652
80	    108	    0,619	    0,00231	                5,284	    0,602
80	    108	    0,636	    0,00194	                5,265	    12,520
100	    128	    2,226	    0,00280	                4,389	    0,766
100	    120	    2,177	    0,00325	                4,190	    9,083
100	    125	    2,057	    0,00761	                4,369	    0,293
100	    133	    2,223	    0,00299	                4,236	    0,712
150	    197	    50,651	    0,25588	                0,836	    0,069
150	    206	    17,500	    0,00892	                2,392	    0,496
150	    195	    11,706	    0,01243	                3,800	    0,416
150	    198	    11,043	    0,00989	                3,842	    1,064
200	    276	    28,487	    0,02453	                4,377	    0,302
200	    260	    27,304	    0,02488	                4,362	    0,523
200	    269	    29,089	    0,01128	                5,043	    0,715
200	    274	    28,400	    0,03426	                4,647	    0,266
250	    317	    73,010	    0,04174	                4,731	    0,270
250	    322	    76,872	    0,05304	                4,894	    53,079
250	    338	    75,947	    0,02685	                5,236	    0,397
250	    326	    72,403	    0,02975	                4,901	    0,409
300	    403	    99,928	    0,06590	                5,060	    0,234
300	    393	    102,025	    0,08875	                5,127	    0,178
300	    408	    96,646	    0,11633	                4,507	    0,135
300	    411	    99,706	    0,10726	                5,126	    11,269
350	    468	    264,713	    0,12901	                4,658	    0,222
350	    475	    267,762	    0,06904	                4,669	    0,342
350	    462	    269,963	    0,07773	                4,836	    0,290
350	    474	    253,972	    0,03380	                3,975	    0,670
400	    543	    398,099	    0,09350	                4,723	    0,339
400	    527	    404,537	    0,09634	                5,168	    0,298
400	    526	    344,346	    0,10317	                3,985	    0,281
400	    525	    411,527	    0,06379	                4,694	    116,083
450	    595	    913,632	    0,06890	                4,376	    0,618
450	    602	    965,629	    0,13109	                4,622	    109,299
450	    593	    1.043,604	0,10328	                4,932	    0,325
450	    594	    908,318	    0,07987	                4,067	    354,120
500	    670	    1.182,420	0,08410	                3,558	    0,544
500	    671	    1.239,040	0,20064	                4,182	    0,265
500	    670	    1.176,348	0,15434	                3,435	    200,764
500	    666	    1.205,442	0,29174	                3,657	    0,174

: Parallel Karger with respect to sequential Karger

![Parallel Karger M*N big scale time over instance](img/MNKarger-Stein_big_parallel.png)

![Parallel Karger M*N time over instance](img/MNKarger-Stein_parallel.png)

![Parallel Karger big scale time over instance](img/NKarger-Stein_big_parallel.png)

![Parallel Karger time over instance](NKarger-Stein_parallel.png)

The execution considers a number of processors $p = 6$ (the machine we run the algorithm on has a 6 processors, 12 threads CPU), the complexity has been adjusted accordingly, this is shown by the graphs.
It is noticeable that while the complexity foresaw a p times faster execution time this is not the case in reality:

 - parallel execution in `F#` and in dotnet is handled the following way: by default are used as much threads as possible, in this instance 12 threads on a 6 processors machine
 - running times are on average 4 times faster on Parallel Karger's, while it should have been 6 times faster
 - min cut discovery time is on average slower than the sequential version of the algorithm.

All of the considerations above might be explained by the garbage collection interference: when garbage collection starts
all threads are stopped, that is why overall execution times are still better but when considering the single task it will
be worse than the sequential version.
