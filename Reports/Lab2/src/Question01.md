\newpage

# Results table

<!-- | Instance     | Closest Insertion                ||| Nearest Neighbour             ||| Metric TSP                     |||
| :----------: | :------- | :---------- | :-------- | :------- | :------- | :-------- | :------- | :-------  | :-------- |
|              | **Sol**  | **Time**    | **Error** | **Sol**  | **Time** | **Error** | **Sol**  | **Time**  | **Error** |
| burma14.tsp  | 3568     | 853570      | 0.073729  | 4048     | 159389   | 0.218176  | 4062     | 204822    | 0.222389  |
| ulysses16.tsp| 7602     | 108640      | 0.108325  | 9874     | 9705     | 0.439568  | 7685     | 86550     | 0.120426  |
| ulysses22.tsp| 7706     | 239430      | 0.098816  | 8628     | 10940    | 0.230287  | 8022     | 148621    | 0.143876  |
| eil51.tsp    | 492      | 2164280     | 0.154930  | 534      | 26034    | 0.253521  | 594      | 988585    | 0.394366  |
| berlin52.tsp | 9004     | 2050130     | 0.193848  | 8980     | 23112    | 0.190666  | 10303    | 751721    | 0.366083  |
| kroD100.tsp  | 25787    | 14738240    | 0.211681  | 26854    | 51804    | 0.261817  | 28809    | 2368161   | 0.353679  |
| kroA100.tsp  | 25219    | 14191410    | 0.184324  | 26947    | 53444    | 0.265474  | 27340    | 2555031   | 0.283930  |
| ch150.tsp    | 7964     | 47014010    | 0.219975  | 8197     | 86503    | 0.255668  | 9077     | 6049426   | 0.390472  |
| gr202.tsp    | 46706    | 118014130   | 0.162998  | 49450    | 133987   | 0.231325  | 49475    | 11120822  | 0.231947  |
| gr229.tsp    | 156895   | 184379280   | 0.165622  | 166150   | 148615   | 0.234380  | 170751   | 14244665  | 0.268562  |
| pcb442.tsp   | 61158    | 1771608140  | 0.204419  | 61609    | 487360   | 0.213301  | 74368    | 69636704  | 0.464571  |
| d493.tsp     | 41163    | 2089836830  | 0.176019  | 43874    | 561792   | 0.253471  | 45666    | 82189901  | 0.304668  |
| dsj1000.tsp  | 22723503 | 40525539930 | 0.217786  | 24630960 | 2332551  | 0.320009  | 25729081 | 445921915 | 0.378859  |

: Contains execution time, results and error of the three algorithms -->

| Instance      | **Optimal sol** | Closest Insertion | Nearest Neighbour | Metric TSP |
| :------------ | :------- | :------- | :------- | :------- |
| burma14.tsp   | 3323     | 3568     | 4048     | 4062     |
| ulysses16.tsp | 6859     | 7602     | 9874     | 7685     |
| ulysses22.tsp | 7013     | 7706     | 8628     | 8022     |
| eil51.tsp     | 426      | 492      | 534      | 594      |
| berlin52.tsp  | 7542     | 9004     | 8980     | 10303    |
| kroD100.tsp   | 21282    | 25787    | 26854    | 28809    |
| kroA100.tsp   | 21294    | 25219    | 26947    | 27340    |
| ch150.tsp     | 6528     | 7964     | 8197     | 9077     |
| gr202.tsp     | 40160    | 46706    | 49450    | 49475    |
| gr229.tsp     | 134602   | 156895   | 166150   | 170751   |
| pcb442.tsp    | 50778    | 61158    | 61609    | 74368    |
| d493.tsp      | 35002    | 41163    | 43874    | 45666    |
| dsj1000.tsp   | 18659688 | 22723503 | 24630960 | 25729081 |

: Solutions of all 3 algorithms compared

| Instance      | Closest Insertion | Nearest Neighbour | Metric TSP |
| :------------ | :---------------- | :---------- | :-------------- |
|               | **Execution times(ns)** | | |
| burma14.tsp   | 853570      | 159389  | 204822    |
| ulysses16.tsp | 108640      | 9705    | 86550     |
| ulysses22.tsp | 239430      | 10940   | 148621    |
| eil51.tsp     | 2164280     | 26034   | 988585    |
| berlin52.tsp  | 2050130     | 23112   | 751721    |
| kroD100.tsp   | 14738240    | 51804   | 2368161   |
| kroA100.tsp   | 14191410    | 53444   | 2555031   |
| ch150.tsp     | 47014010    | 86503   | 6049426   |
| gr202.tsp     | 118014130   | 133987  | 11120822  |
| gr229.tsp     | 184379280   | 148615  | 14244665  |
| pcb442.tsp    | 1771608140  | 487360  | 69636704  |
| d493.tsp      | 2089836830  | 561792  | 82189901  |
| dsj1000.tsp   | 40525539930 | 2332551 | 445921915 |

: Time execution of all 3 algorithms compared

![Comparison between the execution time of the three algorithms \label{fig:time-comparison-1}](img/TimeComparison1.png)

![Comparison between the execution time of Nearest Neighbour and Metric TSP \label{fig:time-comparison-2}](img/TimeComparison2.png)

| Instance      | Closest Insertion | Nearest Neighbour | Metric TSP |
| :------------ | :---------------- | :---------- | :-------------- |
|               | **Errors** | | |
| burma14.tsp   | 7.37%  | 21.81% | 22.23% |
| ulysses16.tsp | 10.83% | 43.95% | 12.04% |
| ulysses22.tsp | 09.88% | 23.02% | 14.38% |
| eil51.tsp     | 15.49% | 25.35% | 39.43% |
| berlin52.tsp  | 19.38% | 19.06% | 36.60% |
| kroD100.tsp   | 21.16% | 26.18% | 35.36% |
| kroA100.tsp   | 18.43% | 26.54% | 28.39% |
| ch150.tsp     | 21.99% | 25.56% | 39.04% |
| gr202.tsp     | 16.29% | 23.13% | 23.19% |
| gr229.tsp     | 16.56% | 23.43% | 26.85% |
| pcb442.tsp    | 20.44% | 21.33% | 46.45% |
| d493.tsp      | 17.60% | 25.34% | 30.46% |
| dsj1000.tsp   | 21.77% | 32.00% | 37.88% |

: Error of all 3 algorithms compared

![Comparison between the error of the three algorithms\label{fig:error-comparison}](img/ErrorComparison.png)
