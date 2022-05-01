module Lab2.Tests.Parsing

open NUnit.Framework
open Lab1.Graphs
open Lab2.Parsing
open Lab2.TspGraph

[<SetUp>]
let Setup () =
    ()

let nodes = [|
 [| 1.0; 38.24; 20.42 |] 
 [| 2.0; 39.57; 26.15 |]
 [| 3.0; 40.56; 25.32 |]
|]
let graphWithEucl =
    Graph ( 
        [| 1; 2; 3 |],
        [|
            (0, 1, 6) // 5.8823294705414
            (0, 2, 5) // 5.4214758138352
            (1, 2, 1) // 1.2918978287775
        |],
        [|
            [1; 0]
            [2; 0]
            [2; 1]
        |]
    )

[<Test>]
let ParsingWithEuclideanWorks () =
    let Graph(ns, es, adj) as actualG = createGraph nodes Eucl2d
    let Graph (ns_, es_, adj_) as expectedG = graphWithEucl
    CollectionAssert.AreEqual(ns_, ns, "%A %A", [ ns_, ns])
    CollectionAssert.AreEqual(es_, es)
    CollectionAssert.AreEqual(adj_, adj,(sprintf "%A\n %A" adj_ adj))
