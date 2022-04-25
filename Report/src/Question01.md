# Results and explanatory graphs

For each algorithm there are two graphs representing the run times, 
respectively one with $n=|V|$ the other one with $m*n$ ($m=|E|,\;n=|V|$) in 
the abscissa axis in order to have a comparison of the relation of both the 
variables with the effective run time.

Then there's a graph comparing the run times of the three algorithms in 
order to analyze and compare the behaviour of the three implementations with 
equal instances and determine the most efficient one.

In the end there's a table reporting the weight of the MST for each given 
graph, calculated by applying one of the three algorithms.

![](N graph Naive Kruskal)
![](MN graph Naive Kruskal)

![](N graph Union-Find Kruskal)
![](MN graph Union-Find Kruskal)

![](N graph Prim)
![](MN graph Prim)

![](graph of all three algs)

        Nodes           Edges           MST weight
------------- --------------- --------------------
       10               9           29316
       10              11           16940
       10              13          -44448
       10              10           25217
       20              24          -32021
       20              24           25130
       20              28          -41693
       20              26          -37205
       40              56         -114203
       40              50          -31929
       40              50          -79570
       40              52          -79741
       80             108         -139926
       80              99         -198094
       80             104         -110571
       80             114         -233320
      100             136         -141960
      100             129         -271743
      100             137         -288906
      100             132         -229506
      200             267         -510185
      200             269         -515136
      200             269         -444357
      200             267         -393278
      400             540        -1119906
      400             518         -788168
      400             538         -895704
      400             526         -733645
      800            1063        -1541291
      800            1058        -1578294
      800            1076        -1664316
      800            1049        -1652119
     1000            1300        -2089013
     1000            1313        -1934208
     1000            1328        -2229428
     1000            1344        -2356163
     2000            2699        -4811598
     2000            2654        -4739387
     2000            2652        -4717250
     2000            2677        -4537267
     4000            5360        -8722212
     4000            5315        -9314968
     4000            5340        -9845767
     4000            5368        -8681447
     8000           10705       -17844628
     8000           10670       -18798446
     8000           10662       -18741474
     8000           10757       -18178610
    10000           13301       -22079522
    10000           13340       -22338561
    10000           13287       -22581384
    10000           13311       -22606313
    20000           26667       -45962292
    20000           26826       -45195405
    20000           26673       -47854708
    20000           26670       -46418161
    40000           53415       -92003321
    40000           53446       -94397064
    40000           53242       -88771991
    40000           53319       -93017025
    80000          106914       -186834082
    80000          106633       -185997521
    80000          106586       -182065015
    80000          106554       -180793224
   100000          133395       -230698391
   100000          133214       -230168572
   100000          133524       -231393935
   100000          133463       -231011693

Table: Weight of all MSTs
