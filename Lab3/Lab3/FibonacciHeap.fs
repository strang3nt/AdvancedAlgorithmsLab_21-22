module Lab3.FibonacciHeap

type MaxHeap<'T> = Tree of 'T option

type FibonacciHeap<'T> = MaxHeap<'T> array

let MakeHeap<'T> () =
    Tree None : MaxHeap<'T>
    
let insert MH x =
    ()