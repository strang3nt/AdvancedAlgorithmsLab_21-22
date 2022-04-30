module Lab1.Parsing

open System.IO
open System.Linq
open Graphs

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
