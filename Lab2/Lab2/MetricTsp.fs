module Lab2.MetricTsp

open Lab1.Graphs
open Lab1.Prim
open System.Collections.Generic

// Optional list of children
type Children = int list option
// Array that contains at index i the children of Nodes[i] (the parent in the MST) or None
type Tree = Children array

let private children v (T: Tree): Children = T[v]

// Given the output of Prim (which in our implementation is an array edges
// instead of parents) build the type Tree: should cost O(P.Length) => O(N - 1)
let buildTree (P: Edge option array): Tree =
    let T = Array.create P.Length None
    Array.iteri (
        fun n e ->
            match e with
            | Some ( e ) -> 
                let (u, v, _) = e
                let p = if u = n then v else u
                match children p T with
                | Some ( chn ) -> T[p] <- Some ( n :: chn )
                | None -> T[p] <- Some ( n :: [] )
            | None -> () 
    ) P
    T

let preorder v T =
    let rec pre v (H: List<_>) T : unit =
        H.Add(v)
        match (children v T) with
        // if is internal continue preorder visit
        | Some ( ch ) -> for c in ch do pre c H T
        // else return
        | None -> ()
    let H = List()
    pre v H T
    H

let metricTsp (G: Graph) =
    let root = 0
    let H =
        // given a MST
        prim root G
        // build the tree-like data structure
        |> buildTree
        // visit in a preorder fashion the structure built while saving nodes visited into a list
        |> preorder root
    H.Add(root); H
