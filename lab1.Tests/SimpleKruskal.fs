module lab1.Tests.SimpleKruskal

open lab1.Graphs
open lab1.SimpleKruskal
open NUnit.Framework

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
let Test0_isAcyclical_WholeGraph () =
    let Graph (_, es, _) as G = graph
    let A = Array.create es.Length true
    let checkIfAcyclical = isAcyclical 3 A G
    Assert.False ( checkIfAcyclical )

[<Test>]
let Test1_isAcyclical_WithTree () =
    let Graph (_, es, _) as G = graph
    let A = Array.create es.Length false
    A[0] <- true
    A[1] <- true
    A[3] <- true
    let checkIfAcyclical = isAcyclical 8 A G
    Assert.True ( checkIfAcyclical )

[<Test>]
let Test2_isAcyclical_WithRejectedNode () =
    let Graph (_, es, _) as G = graph
    let A = Array.create es.Length false
    A[0] <- true
    A[1] <- true
    A[3] <- true
    let checkIfAcyclical = isAcyclical 4 A G
    Assert.False ( checkIfAcyclical )

[<Test>]
let Test3_isAcyclical_MSTAlreadyFound () =
    let Graph (_, es, _) as G = graph
    let A = Array.create es.Length false
    A[1] <- true
    A[0] <- true
    A[3] <- true
    A[6] <- true
    let checkIfAcyclical = isAcyclical 2 A G
    Assert.False ( checkIfAcyclical )


[<Test>]
let Test0_simpleKruskal () =
    let Graph (_, es, _) as G = graph
    let expected =
        [
            es[1];
            es[0];
            es[3];
            es[6];
        ]
    let actual = simpleKruskal G
    CollectionAssert.AreEqual (expected, actual, (sprintf "%A\n %A" expected actual))
