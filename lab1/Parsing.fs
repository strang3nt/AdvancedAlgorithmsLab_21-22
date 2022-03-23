module lab1.Parsing

open lab1.Graphs
open System
open System.IO

let file filename =
    let path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "/graphs/" + filename
    File.ReadAllLines(path)

let buildGraph (filename : string) : Graph =
    let str = file filename
    let mutable g : Graph = Array.empty, Array.empty
    for s in str do
        let edgeArr = Array.map (fun x -> int x ) (s.Split [| ' ' |])

        if not (nodeExists g edgeArr[0]) then
            g <- addNode (g) (edgeArr[0])

        if not (nodeExists g edgeArr[1]) then
            g <- addNode (g) (edgeArr[1])

        g <- addEdge g edgeArr[1] edgeArr[2] edgeArr[3]
    g
