\newpage

# Comments on the results

- How do the algorithms behave with respect to the various instances? 
- Is there an algorithm that always manages to do better than the others with respect to the approximation error? 
- Which of the three algorithms you have implemented is more efficient?

Based on our implementations the three algorithms have different complexities, 
and each one of them behaves according to its own; in particular considering 
$n=|V|$:

The Nearest Neighbour heuristic has time complexity of $O(n^2)$ thanks to the 
optimizations discussed in [Optimizations] and as confirmed by the 
graph \ref{fig:ONearestNeighbour}. Moreover, despite being a 
$\log(n)-$approximation algorithm the errors obtained with the given graphs 
are reasonably low and in some cases better than other algorithms with lower 
approximation factor as can be seen in \ref{fig:error-comparison}.

The Metric TSP algorithm instead has time complexity of $O(m\log n)$ as 
discussed in [Metric TSP][2-approximation Metric Tsp] due to the complexity of 
the `prim` algorithm for computing the MST of the graphs, and the errors 
computed on the solutions given by the algorithm respect its $2-$approximation 
factor.

The Closest Insertion heuristic has time complexity of $O(n^3)$.

From \ref{fig:error-comparison} it can be seen that the Closest Insertion 
heuristic in each instance has the lowest approximation error between the 
algorithms analysed. It's interesting to notice that in average the Metric TSP 
algorithm seems to have the worst approximation error with the given dataset 
despite being a $2-$approximation, even when compared with 
the Nearest Neighbour heuristic which is a $\log(n)-$approximation algorithm.


Instead, regarding time efficiency, from \ref{fig:time-comparison-1} and 
\ref{fig:time-comparison-2}, it can be seen that the most efficient of the 
three algorithms implemented is the Nearest Neighbour heuristic which we were 
able to implement with a time complexity of $O(n^2)$. Followed in increasing 
order by the Metric TSP algorithm with time complexity of $O(m\log n)$ and at 
last there's the Closest Insertion heuristic, which has the slowest 
implementation with a time complexity of $O(n^3)$.
