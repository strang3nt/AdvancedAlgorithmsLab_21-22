\newpage

# Discovery time of Karger and Stein algorithm

    N     M   Instant(seconds)
----- ----- ------------------
10	    14	    0,00786
10	    10	    0,00008
10	    12	    0,00005
10	    11	    0,00018
20	    24	    0,00033
20	    24	    0,00029
20	    27	    0,00017
20	    25	    0,00018
40	    52	    0,00847
40	    54	    0,00047
40	    51	    0,00049
40	    50	    0,00073
60	    82	    0,00096
60	    72	    0,00084
60	    83	    0,00086
60	    79	    0,00091
80  	 101	    0,00145
80	   105	    0,00149
80  	 108	    0,00139
80	   108	    0,02427
100	   128	    0,00214
100	   120	    0,02956
100	   125	    0,00223
100	   133	    0,00213
150	   197	    0,01768
150	   206	    0,00443
150	   195	    0,00517
150	   198	    0,01052
200	   276	    0,00741
200	   260	    0,01302
200	   269	    0,00806
200	   274	    0,00910
250	   317	    0,01127
250	   322	    2,81514
250	   338	    0,01065
250	   326	    0,01217
300	   403	    0,01543
300	   393	    0,01582
300	   408	    0,01571
300	   411	    1,20873
350	   468	    0,02860
350	   475	    0,02361
350	   462	    0,02253
350	   474	    0,02263
400	   543	    0,03173
400	   527      0,02875
400	   526	    0,02897
400	   525	    7,40521
450	   595	    0,04261
450	   602	    14,32769
450	   593	    0,03361
450	   594	    28,28423
500	   670	    0,04576
500	   671	    0,05317
500	   670	    30,98502
500	   666	    0,05077

: Karger's min cut found instant

![Graph size with respect to instant](img/karger_instant.png)

From the table and the graph it is noticeable that the bigger is the
size of the input the slowest the mincut is found, unsurprisingly.
Except for some outliers the mincut is found during the very first iterations
of the algorithm. Even the mincut outliers, with respect to the size of the input,
are found during the very first steps of Karger's execution.
