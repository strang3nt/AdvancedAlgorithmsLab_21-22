# Originalities

## F# as a imperative language

`F#` is a functional and declarative language, but supports object oriented and imperative paradigms.
We initially chose said language because we wanted to implement all three algorithms using a functional
paradigm: while some functional characteristics were actually useful (for example pattern matching and type
declarations) others actually increment complexity.

Functional and immutable data structures: these type of data structures offer some great guarantees but at the same
time they inherently create a lot of garbage and increased time complexity.

## Prim's implementation



## .NET Garbage collection

```fsharp
let measureRunTime f input numCalls =
    let defaultLatency = Runtime.GCSettings.LatencyMode
    Runtime.GCSettings.LatencyMode <- Runtime.GCLatencyMode.SustainedLowLatency
    // ...
    Runtime.GCSettings.LatencyMode <- defaultLatency
    // ...
```

The `measureRunTime` function is used to get the execution times of the algorithms. It is not possible to shut down garbage collection in `.NET` environment, unless the amount of memory that will be used is known in advance. On the other hand one can tune how much the garbage collector intervenes: garbage collection set as `SustainedLowLatency` means that it intervenes as less as possible.
