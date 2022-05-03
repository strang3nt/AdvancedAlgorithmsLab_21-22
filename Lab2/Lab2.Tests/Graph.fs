module Lab2.Tests.TspGraph

open NUnit.Framework
open Lab2.TspGraph

// [| coordx; coordy; euclDistance |]
let coord1 = [| 38.24; 20.42; 6 |]
let coord2 = [| 39.57; 26.15; 5 |]
let coord3 = [| 40.56; 25.32; 1 |]

[<Test>]
let EuclideanWeight() =
    let w1 = w coord1[0] coord1[1] coord2[0] coord2[1] Eucl2d
    let w2 = w coord1[0] coord1[1] coord3[0] coord3[1] Eucl2d
    let w3 = w coord2[0] coord2[1] coord3[0] coord3[1] Eucl2d

    Assert.AreEqual(int coord1[2], w1)
    Assert.AreEqual(int coord2[2], w2)
    Assert.AreEqual(int coord3[2], w3)
