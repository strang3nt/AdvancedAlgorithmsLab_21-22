module lab1.UnionFind

open Microsoft.FSharp.Collections

type 'a UFNode = 'a * int
let rec initUF (p: 'a array) : Map<'a,'a UFNode> =
    Map (p |> Array.map (fun x -> x, (x, 1)))

let rec find (uf: Map<'a,'a UFNode>) x =
    let parentOpt = uf |> Map.tryFind x
    match parentOpt with
    | None -> failwith (sprintf "No value associated to the given element %O" x)
    | Some (parent, size) ->
        if parent = x then
            parent,size
        else
            find uf parent

let size (uf : Map<'a,'a UFNode>) x =
    let xOpt = uf |> Map.tryFind x
    match xOpt with
    | None -> failwith (sprintf "No value associated to the given element %O" x)
    | Some (_, x_size) -> x_size
    
// Changes the parent of the j element to i in the UnionFind data structure uf and updates the size of i accordingly
let change_root (uf : Map<'a,'a UFNode>) i j =
    let i, _ = i
    let j, j_size = j
    uf  |> Map.change j (fun j ->
            match j with
            | None -> failwith "No value associated to the given element"
            | Some (_, j_size) -> Some (i, j_size))
        |> Map.change i (fun i ->
            match i with
            | None -> failwith "No value associated to the given element"
            | Some (i, i_size) -> Some (i, i_size + j_size))
    
let union (uf : Map<'a,'a UFNode>) x y =
    let x_root, x_size = find uf x
    let y_root, y_size = find uf y
    if x_root = y_root then
        uf
    elif x_size >= y_size then
        change_root uf (x_root, x_size) (y_root, y_size)
    else
        change_root uf (y_root, y_size) (x_root, x_size)
