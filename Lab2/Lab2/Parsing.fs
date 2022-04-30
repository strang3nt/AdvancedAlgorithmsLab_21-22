module Lab2.Parsing

open Lab1.Parsing
open Lab1.Graphs
open TspGraph

let createGraph (nodes: float array array) weightType: Graph =
    let N = nodes.Length
    let M = ( nodes.Length * (nodes.Length - 1) ) / 2
    let sizes = [| N; M |]
    let edges : int array array = Array.zeroCreate M
    
    //let mutable i = 0
    for i = 0 to N - 1 do
        for n = i + 1 to N - 1 do
                edges[i * N + n] <- [| int nodes[i].[0]; int nodes[n].[0]; (w nodes[i].[1] nodes[i].[2] nodes[n].[1] nodes[n].[2] weightType) |]

    buildGraph edges sizes
    
