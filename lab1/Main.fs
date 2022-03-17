

// For more information see https://aka.ms/fsharp-console-apps
open Plotly.NET
open Plotly.NET.ImageExport

let xData = [0. .. 10.]
let yData = [0. .. 10.]

Chart.Point(xData,yData)
|> Chart.savePNG(
    "./out/example1",
    Width=300,
    Height=300
)
