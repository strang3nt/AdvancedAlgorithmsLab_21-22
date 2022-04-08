module lab1.Parsing

open lab1.Graphs
open System.IO

let file filename =
//    Directory.GetCurrentDirectory() + "/graphs/" +
    filename
    |> File.ReadAllLines

let getHeader filename =
    let s = Array.head (file filename)
    Array.map int (s.Split [| ' ' |])

let buildGraph (filename : string) : Graph =
    let str = file filename |> Array.skip 1
    let mutable g : Graph = Array.empty, Array.empty
    for s in str do
        let edgeArr = Array.map int (s.Split [| ' ' |])
        if not (nodeExists g edgeArr[0]) then
            g <- addNode (g) (edgeArr[0])
        if not (nodeExists g edgeArr[1]) then
            g <- addNode (g) (edgeArr[1])
        g <- addEdge g edgeArr[0] edgeArr[1] edgeArr[2]
    g

let buildSimpleGraph (filename : string) : Graph2 = 
    let arrayToTuple (l : string array) : Node * Node * Weight =
        (int l[0], int l[1], int l[2])
    file filename 
    |> Array.skip 1
    |> Array.Parallel.map (fun x -> arrayToTuple (x.Split [| ' ' |]))
