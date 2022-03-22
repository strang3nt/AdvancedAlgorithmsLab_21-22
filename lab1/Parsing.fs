module lab1.Parsing

open System
open System.IO

let file filename =
    let path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "/graphs/" + filename
    File.ReadAllLines(path)