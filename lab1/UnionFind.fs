module lab1.UnionFind

open System
open Microsoft.FSharp.Collections

type Size = int
type 'a UFNode = 'a * Size

// Initialize the Union-Find data structure starting from an array of objects in O(n)
let rec initUF (p: 'a array) : Map<'a,'a UFNode> =
    Map (p |> Array.map (fun x -> x, (x, 1)))

// Find the root of the node in the given Union-Find in O(k), with k = uf depth = log n 
let rec find (uf: Map<'a,'a UFNode>) x =
    uf  |>  Map.find x
        |>  (fun (parent, size) ->
                if parent = x then
                    parent, size
                else
                    find uf parent
            )

// Returns the size of the "sub-tree" in the given Union-Find uf of the given node x in O(log n)
let size (uf : Map<'a,'a UFNode>) x =
    uf  |>  Map.find x
        |>  snd
    
// Changes the parent of the j element to i in the UnionFind data structure uf and updates the size of i accordingly in O(log n)
let change_root (uf : Map<'a,'a UFNode>) i j =
    let i, _ = i
    let j, j_size = j
        // O(log n)
    uf  |> Map.change j (fun j ->
            match j with
            | None -> failwith "No value associated to the given element"
            | Some (_, j_size) -> Some (i, j_size))
        // O(log n)
        |> Map.change i (fun i ->
            match i with
            | None -> failwith "No value associated to the given element"
            | Some (i, i_size) -> Some (i, i_size + j_size))
    
// Merges the trees in the Union-Find uf associated to the nodes x and y in O(k), with k = uf depth = log n
let union (uf : Map<'a,'a UFNode>) x y =
    // O(log n)
    let x_root, x_size = find uf x
    // O(log n)
    let y_root, y_size = find uf y
    if x_root = y_root then
        uf
    elif x_size >= y_size then
        // O(log n)
        change_root uf (x_root, x_size) (y_root, y_size)
    else
        // O(log n)
        change_root uf (y_root, y_size) (x_root, x_size)
