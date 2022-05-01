module Lab2.Tests.Parsing

open NUnit.Framework
open Lab1.Graphs
open Lab2.Parsing
open Lab2.TspGraph
open System.IO

[<SetUp>]
let Setup () =
    ()

let (+/) path1 path2 = Path.Combine(path1, path2)
let fileName = "mockDataset.tsp"
let file = ".." +/ ".." +/ ".." +/ "tsp_dataset" +/ fileName

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

let tspGraph: TspGraph =
    TspGraph (
        "mockDataset.tsp",
        "This is a comment, will it be useful?",
        3,
        graphWithEucl
    )

let assertGraph expectedGraph actualGraph =
    let Graph (ns, es, adj) as _= expectedGraph
    let Graph (ns_, es_, adj_) as _ = actualGraph
    CollectionAssert.AreEqual(ns_, ns, "%A %A", [ ns_, ns])
    CollectionAssert.AreEqual(es_, es)
    CollectionAssert.AreEqual(adj_, adj,(sprintf "%A\n %A" adj_ adj))

[<Test>]
let ParsingWithEuclideanWorks () =
    assertGraph (getGraph nodes Eucl2d) graphWithEucl

[<Test>]
let ParsingOfFile() =
    let TspGraph (n, c, d, G) as _ = tspGraph
    let TspGraph (n_, c_, d_, G_) as _ = buildGraph file
    Assert.AreEqual (n, n_)
    Assert.AreEqual (c, c_)
    Assert.AreEqual (d, d_)
    assertGraph G G_
