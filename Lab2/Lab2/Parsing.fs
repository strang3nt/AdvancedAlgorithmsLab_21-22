module Lab2.Parsing

open Lab1.Parsing
open Lab1.Graphs
open TspGraph

let private name = "NAME: "
let private ty = "TYPE: "
let private comment = "COMMENT: "
let private dimension = "DIMENSION: "
let private edgeWeightType = "EDGE_WEIGHT_TYPE: "
let private displayDataType = "DISPLAY_DATA_TYPE: "
let private nodeCoordSect = "NODE_COORD_SECTION"
let private eof = "EOF"

let getGraph (nodes: float array array) weightType: Graph =
    let N = nodes.Length
    let M = ( nodes.Length * (nodes.Length - 1) ) / 2
    let sizes = [| N; M |]
    let edges : int array array = Array.zeroCreate M
    
    for i = 0 to N - 1 do
        for n = i + 1 to N - 1 do
            edges[i + n - 1] <- [| int nodes[i].[0]; int nodes[n].[0]; (w nodes[i].[1] nodes[i].[2] nodes[n].[1] nodes[n].[2] weightType) |]

    buildGraph edges sizes

let private (|Prefix|_|) (p:string) (s:string) =
    if s.StartsWith(p) then
        Some(s.Substring(p.Length))
    else
        None

let rec private parseMetadata pattern arr =
    match arr with
    | (Prefix pattern rest) :: _ -> rest
    | (Prefix nodeCoordSect _) :: _ -> "Value not found"
    | _ :: arrRest -> parseMetadata pattern arrRest
    | _ -> failwith "Unexpected parsing error: Lab2.Parsing::parseMetadata should never reach here"

let getNodeCoordSection (arr: string array): float array array =
    let startOf =  Array.findIndex (fun (str: string) -> (str.StartsWith(nodeCoordSect))) arr
    let endOf = Array.findIndexBack (fun (str: string) -> (str.StartsWith(eof))) arr
    let ar = Array.sub arr (startOf + 1) (endOf - startOf - 1)
    ar |> Array.map (fun str -> Array.map float (str.Trim().Split()))

let buildGraph filename: TspGraph =
    let arr = Array.toList (file filename)

    let weightType =
        match (parseMetadata edgeWeightType arr) with
        | "GEP" -> Geo
        | "EUC_2D" -> Eucl2d  
        | _ -> failwith "Error during parsing: could not find weight type"

    TspGraph (
        parseMetadata name arr,
        parseMetadata comment arr,
        parseMetadata dimension arr |> int,
        getGraph (getNodeCoordSection (List.toArray arr)) weightType
    )
