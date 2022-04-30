module Lab1.Prim

open Graphs
open FSharpx.Collections
open System
open System.Collections.Generic

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
