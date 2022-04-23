module lab1.Parsing

open lab1.Graphs
open System.IO
open System.Linq

let file filename =
    Directory.GetCurrentDirectory() + "/graphs/" + filename
    |> File.ReadAllLines

let getHeader filename =
    let s = File.ReadLines( Directory.GetCurrentDirectory() + "/graphs/" + filename ).First()
    Array.map int (s.Split [| ' ' |])

let buildGraph (filename : string) : Graph =
    let edgesArray = 
        file filename 
        |> Array.skip 1 
        |> Array.map (fun str -> Array.map int (str.Split [| ' ' |]))
    let sizes = getHeader filename
    buildGraph edgesArray sizes
