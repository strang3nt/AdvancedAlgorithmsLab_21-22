module lab1.Utils

let saveToCSV filename (N: int array) (M: int array) (rTs: int array) (C: float array) =
    let writer = new System.IO.StreamWriter (filename + ".csv")
    writer.WriteLine "n, m, runtime, c"
    for i=0 to N.Length-1 do
        writer.WriteLine $"%9i{N[i]}, %9i{M[i]}, %9i{rTs[i]}, %9.3f{C[i]}"
    writer.Close()