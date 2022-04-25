## Prim

```fsharp
[<Struct>]
type Key = Key of (Weight * Node)

let prim s (Graph (ns, es, adj) as G : Graph) =

    // init heap structure
    let Q = SortedSet<Key>()
    let keys = Array.create ns.Length -1
    let Pi = Array.create ns.Length None

    // build heap
    Q.Add (Key (0, s)) |> ignore
    keys[0] <- Int32.MaxValue
    for i = 1 to ns.Length - 1 do
        Q.Add (Key (Int32.MaxValue, i)) |> ignore
        keys[i] <- Int32.MaxValue

    while Q.Count > 0 do

        // take min key and update heap
        let (Key (_, u) as k) = Q.Min
        Q.ExceptWith (seq { k })
        
        // update heap
        for e in adj[u] do
            let v = opposite u e G
            let k = Key (keys[v], v)
            let (_, _, w) = es[e]

            if Q.Contains k && w < keys[v] then
                Q.ExceptWith (seq { k })
                Q.Add (Key (w, v)) |> ignore
                keys[v] <- w
                Pi[v] <- Some(es[e])

    Pi
```

The algorithm above is Prim's algorithm. Due to the lack of a suitable heap implementation in either .NET's APi or other `F#` libraries, a structure called `SortedSet` was used:

 - insertion is $O(\log(n))$
 - removal of one element is $O(\log(n))$, but according to the documentation, the method `exceptWith` is `O(n)` which should be `O(1)` in this instance because we are removing only 1 element at a time
 - lookup is $O(\log(n))$
 - min lookup is constant
 - min extraction is $O(\log(n))$.
 
From the data reported above ([see Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedset-1?redirectedfrom=MSDN&view=net-6.0)), `SortedSet` seems to be equivalent to a heap data structure, at least regarding time complexity.

Complexity of the whole algorithm is then $O (m \log(n))$:

 - the main cycle has a complexity at most $O(m)$
 - removal of an element has cost $O(1)$
 - checking if a node is in the sorted set `Q` is $(\log(n))$
 - the inner cycle has cost $O(n\log(n))$.
