module Lab2.MetricTsp

open Lab1.Graphs
open Lab1.Prim
open System.Collections.Generic

type Children = int list option
type Tree = Children array

let private children v (T: Tree): Children = T[v]

let buildTree (P: Edge option array): Tree =
    let T = Array.zeroCreate P.Length

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

let rec preorder v (H: List<Node>) T: unit =
    H.Add(v)
    match (children v T) with
    | Some (ch) -> 
        for c in ch do
            preorder c H T
    | None -> ()

let metricTsp (G: Graph) =
    let root = 0
    let T = prim root G |> buildTree
    let H = List<Node>()
    preorder root H T
    H
