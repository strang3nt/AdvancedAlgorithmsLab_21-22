# On algorithms behaviour

 - How do the algorithms behave with respect to the various instances?
 - Is there an algorithm that is always better than the others?
 - Which of the three algorithms you have implemented is more efficient?

Based on our implementations each one of the three algorithms behaves 
accordingly to its complexity with respect of the various instances in fact, 
considering $m=|E|$ and $n=|V|$:


The naive version of Kruskal's algorithm has a theoretical time complexity of 
$O(m*n)$, and the hidden constant that has been experimentally found in 
`NaiveKruskal` is $c\approx$.
From the graph [](link to N graph) can be seen that the run times obtained 
define a quadratic-like function, which respects the theoretical results of 
the algorithm. Moreover on the graph [](link to MN graph) it's possible to 
see how the run times are linear respect to $m*n$, confirming once again the 
theoretical results.


The Kruskal's algorithm based on the Union-Find data structure has a 
theoretical time complexity of $O(m\log n)$, and the hidden 
constant experimentally computed on `UnionFindKruskal` is $c\approx$. 
From the graph [](link to N graph) can be seen that the run times obtained 
define a convex-like function which is almost linear and respects the 
estimated asymptotic growth; moreover from the graph [](MN graph) it can be 
seen how the growth of the run times is logarithmicly proportional to $m*n$, 
confirming the theoretical results of the algorithm.


Prim's algorithm has a theoretical time complexity of $O(m\log n)$, and the 
hidden constant that has been experimentally computed on `Prim` is $c\approx$. 
From the graph [](link to N graph) can be seen that the run times obtained 
define a convex-like function which is almost linear and respects the 
estimated asymptotic growth; moreover from the graph [](MN graph) it can be 
seen how the growth of the run times is logarithmicly proportional to $m*n$, 
confirming the theoretical results of the algorithm.


From [](graph with the run times of all three algs) it can be seen 
how `Prim` outperforms both `NaiveKruskal` (trivially due to an higher time 
complexity) and `UnionFindKruskal` despite having the same time complexity of 
the latter. This result can be justified by a lower hidden constant due to the 
different implementations of the algorithms.

![](graph of all three algs)
