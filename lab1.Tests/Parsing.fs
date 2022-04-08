module lab1.Tests

open lab1.Graphs
open NUnit.Framework
open System.IO

let (+/) path1 path2 = Path.Combine(path1, path2)
let fileName = "graph1.txt"
let file = ".." +/ ".." +/ ".." +/ "graphs" +/ "graph1.txt"

let graph = 
    Graph ( 
        [| 1; 3; 5; 4; 2|],
        [|
            (0, 1, -1)
            (0, 2, 3)
            (0, 3, 15)
            (0, 4, -20)
            (4, 1, 0)
            (4, 3, 30)
            (1, 3, -55)
            (1, 2, 10)
            (3, 2, 3 )
        |],
        [|
            [3; 2; 1; 0]
            [7; 6; 4; 0]
            [8; 7; 1]
            [8; 6; 5; 2];
            [5; 4; 3];
        |]
    )

[<Test>]
let buildGraph () =
    let Graph(ns, es, adj) as actualG = Parsing.buildGraph file
    let Graph (ns_, es_, adj_) as expectedG= graph
    CollectionAssert.AreEqual(ns_, ns, "%A %A", [ ns_, ns])
    CollectionAssert.AreEqual(es_, es)
    CollectionAssert.AreEqual(adj_, adj,(sprintf "%A\n %A" adj_ adj))
