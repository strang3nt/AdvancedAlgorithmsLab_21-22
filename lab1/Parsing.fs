module lab1.Parsing

open Graphs
open System.IO
open System.Linq

let (+/) path1 path2 = Path.Combine(path1, path2)

let file filename =
    filename
    |> File.ReadAllLines

let getHeader filename =
    let s = File.ReadLines( filename ).First()
    Array.map int (s.Split [| ' ' |])

let buildGraph (filename : string) : Graph =
    let edgesArray = 
        file filename 
        |> Array.skip 1 
        |> Array.map (fun str -> Array.map int (str.Split [| ' ' |]))
    let sizes = getHeader filename
    buildGraph edgesArray sizes
