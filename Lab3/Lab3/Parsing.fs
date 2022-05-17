module Lab3.Parsing

open Lab1.Parsing
open Lab1.Graphs
open Lab3.Graph

let buildGraph (filename: string): MinCutGraph =
    let Graph (ns, es, _) as G = Lab1.Parsing.buildGraph filename
    MinCutGraph (
        ns,
        es,
        es |> getEdgesDict
    )
