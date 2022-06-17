\newpage

# Stoer and Wagner's deterministic algorithm

The code for implementing the Stoer and Wagner's algorithm is the following:

```fsharp
// Calculates the global minimum cut of G in O(mn*log(n))
let rec StoerWagner (MinCutGraph (V, _, _, D, _) as G) =
//  Check the number of nodes in the graph in  O(n) with n = |V|
    if D |> Array.filter (fun x -> x > 0) |> Array.length = 2 then
//      Return the first node in the graph in O(n) with n = |V| 
//      and the weight of the associated minimum cut
        let minCut = [| V[D |> Array.findIndex (fun x -> x > 0)] |]
        minCut, CutWeight G minCut
    else
//      Calculates the st minimum cut of G in O(m*log(n))
        let c1, s, t = stMinCut G
        let w1 = CutWeight G c1
//      Calculates the global minimum cut in G-{s,t} in O(mn*log(n))
        let c2, w2 = StoerWagner (merge G s t)
//      Compares the weight of the two minimum cuts and 
//      returns the smallest one
        if w1 <= w2 then
            (c1, w1)
        else
            (c2, w2)
```

It uses an auxiliary function `stMinCut` that calculates an 
**s-t minimum cut** and it is implemented as follows:

```fsharp
// Calculates the st minimum cut in G in O(m*log(n))
let stMinCut (MinCutGraph (V, _, _, D, W)) =
//  Initialise the priority queue and the keys array in O(n) with n = |V|
    let Q = SortedSet<Key>()
    let keys = Array.create (V.Length) 0
    let mergedNodes = D |> Array.filter (fun x -> x = 0) |> Array.length
    [|0..keys.Length-1|] |> Array.iter (fun i -> 
        Q.Add (Key (keys[i], i)) |> ignore )
    
    let mutable s: int option = None
    let mutable t: int option = None
    
//  Iterate until the priority queue is empty O(m) times
    while Q.Count-mergedNodes > 0 do
        
//      Get the index of the node with the highest associated weight 
//      in the priority queue in O(1)
        let Key (_, u) as k = Q.Max
//      Remove the max element from the priority queue in O(1)
        Q.ExceptWith (seq { k })
        
        s <- t
        t <- Some u
        
//      Iterates through the nodes adjacent to u in O(n*log(n))
        W[u, *] |> Array.mapi (fun i w -> i, w)
        |> Array.filter (fun (_,x) -> x > 0)
        |> Array.iter (fun (v,_) ->
            let key = Key (keys[v], v)
//          Check if the element key is in the priority queue in O(log(n))
            if Q.Contains key then
//              Update the value of the key in O(1)
                keys[v] <- keys[v] + W[u, v]
//              Removes the the key from the priority queue in O(1)
                Q.ExceptWith (seq { key })
//              Adds the new key in priority queue in O(log(n))
                Q.Add (Key (keys[v], v)) |> ignore
            )

//  Gets the indexes of the nodes previously contracted in O(n)
    let merged = D |> Array.toList
                 |> List.mapi (fun i w -> w, i)
                 |> List.filter (fun (w, _) -> w = 0)
                 |> List.map snd
                 
//  Calculates the indexes of the nodes that has to be removed from V
    let toRemove = t.Value :: merged |> List.sort
//  Removes the contracted nodes and t from V and returns 
//  the generated cut with s and t
    List.foldBack (fun i V -> V |> Array.removeAt i) toRemove V, s.Value, t.Value
```

For the implementation of `stMinCut` we used a `SortedSet` which is relatable 
to a *MaxHeap* since:

- insertion is $O(\log(n))$
- removal of one element is $O(1)$ since, according to the documentation, 
the method `ExceptWith` is $O(n)$, with $n$ being the size of the function 
parameter. In our specific case it is $O(1)$ because we are removing only one 
element at a time
- lookup is $O(\log(n))$
- max lookup is $O(1)$
- max extraction is $O(1)$.

Hence the time complexity of `stMinCut` is $O(m \log(n))$, with $n=|V|$ and 
$m=|E|$, since:

- the main cycle has a complexity at most O(m)
- removal of an element has cost O(1)
- checking if a node is in the sorted set Q is $O(\log(n))$ 
- the inner cycle has cost O(n log(n)).

The time complexity of `StoerWagner` instead is $O(mn\log(n))$ since the check 
on the size of *V* is $O(n)$ and `stMinCut`, which is $O(m\log(n))$, is 
executed $O(n)$ times.


It can be noted that the implementation of `StoerWagner` and `stMinCut` 
differs slightly from the one specified in the theory since in our 
implementation of `Graph` we encode the contractions on a graph by operating 
on `D` and `W`, hence instead of checking conditions or operating directly on 
`V`, `E` and `A` we operate in `D` and `W`, using the other fields of `Graph` 
when needed for operations without side effects. Moreover we represent a 
mincut as a subset of `V` instead of a partition $(S,T)$ of `V` as specified 
in the theory and `StoerWagner` returns both a mincut (following our 
representation) and its weight.
