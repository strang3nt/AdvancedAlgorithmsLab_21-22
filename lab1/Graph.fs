
open FSharp.Collections

type Node = int
type Weight = int
type something = Node * Node array
type Edge = Node * Node * Weight
type Graph = something array * Edge array

type Graph1 = something array * Edge array

type Neighbour = Node * Weight
type something2 = Node * Neighbour array
type Graph2 = something array

type Graph3 = Weight Weight array2D