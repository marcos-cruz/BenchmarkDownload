# BenchmarkDownload

This application is a benchmark, which demonstrates the difference in performance with use of synchronized, asynchronous and parallel processing.

# How is it Done?

A WPF application downloads the content from 9 sites synchronously, asynchronously and parallel.

# Experiment environment

* Windows WPF Application using .NET Framework 4.7.2
* Processor: Intel(R) Core(TM)2 Duo CPU T6500 @2,10GHz
* Memory (RAM): 4,00 GB
* System: Microsoft Windows 10 Professional 64 bit
* Wireless network(*): Download 10.07 Mb/s, Upload 3.50 Mb/s, Latency 26 ms, Retransmission 0.45%

(*) https://speed.measurementlab.net/#/

## Websites

* https://www.yahoo.com,
* https://www.google.com,
* https://www.microsoft.com,
* https://www.cnn.com,
* https://www.amazon.com,
* https://www.facebook.com,
* https://www.twitter.com,
* https://www.codeproject.com,
* https://www.stackoverflow.com

# Case 1 - Download Synchronously

The download is done synchronously, that is, it downloads one content, then the other, the other and so on. The comparative table shows the processing behavior:

| # | Download in milliseconds  | Bytes   | Average time (ms) |
|---|--------------------------:|--------:|-------------------|
| 1 | 8847                      | 1993391 | Not considered because the first time it runs, it's always slow |
| 2 | 5489                      | 1988665 | 5489 |
| 3 | 6136                      | 1988483 | 5812 |
| 4 | 5747                      | 1988630 | 5790 |
| 5 | 5658                      | 1986383 | 5757 |
| 6 | 6335                      | 1993106 | 5873 |
| 7 | 5713                      | 1989936 | 5846 |
|   | A V E R A G E   T O T A L | 1989200 | 5761 |

# Case 2 - Download Synchronously Parallel

The download is done synchronously and in parallel, that is, each download is a task that is triggered almost simultaneously. The comparative table shows the processing behavior:

| # | Download in milliseconds | Bytes | Average time (ms) |
|---|--------------------------:|--------:|-------------------|
| 1 | 3668 | 1988630 | Not considered because the first time it runs, it's always slow |
| 2 | 2351 | 1993473 | 2351 |
| 3 | 2393 | 1990277 | 2372 |
| 4 | 2300 | 1995308 | 2348 |
| 5 | 2215 | 1987888 | 2314 |
| 6 | 1611 | 1961855 | 2174 |
| 7 | 2495 | 1989507 | 2227 |
|  |  | 1986384,667 | 2820 |

# Case 3 - Download Asynchronously

The download is done asynchronously, that is, the contents are downloaded almost simultaneously. The comparative table shows the processing behavior:

| # | Download in milliseconds | Bytes | Average time (ms) |
|---|--------------------------:|--------:|-------------------|
| 1 | 8353 | 1965105 | Not considered because the first time it runs, it's always slow |
| 2 | 6258 | 1965301 | 6258 |
| 3 | 5811 | 1963942 | 6034 |
| 4 | 6161 | 1962471 | 6076 |
| 5 | 5871 | 1964627 | 6025 |
| 6 | 5785 | 1968559 | 5977 |
| 7 | 5873 | 1962744 | 5959 |
| |  | 1964607,333 | 6055 |

# Case 4 - - Download Asynchronously Parallel

The download is done in an asynchronous and parallel way, that is, the contents are downloaded almost simultaneously. The comparative table shows the processing behavior:

| # | Download in milliseconds | Bytes | Average time (ms) |
|---|--------------------------:|--------:|-------------------|
| 1 | 2774 | 1991397 | Not considered because the first time it runs, it's always slow |
| 2 | 2119 | 1990654 | 2119 |
| 3 | 2145 | 1990530 | 2132 |
| 4 | 2804 | 1990158 | 2356 |
| 5 | 1549 | 1994188 | 2154 |
| 6 | 2325 | 1996087 | 2188 |
| 7 | 2447 | 1989113 | 2231 |
|  |  | 1991788,333 | 2196 |
