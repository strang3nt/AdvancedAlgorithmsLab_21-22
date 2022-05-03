module Lab2.TspGraph

open Lab1.Graphs

type Name = string
type Comment = string
type Dimension = int
type EdgeWeightType =
    | Geo
    | Eucl2d

[<Struct>]
type TspGraph = TspGraph of (Name * Comment * Dimension * Graph)

//For converting coordinate input to longitude and latitude in radian:
//
//  PI = 3.141592;
//
//  deg = (int) x[i];
//  min = x[i]- deg;
//  rad = PI * (deg + 5.0 * min/ 3.0) / 180.0;
let private radian (coord: float) =
    let PI = 3.141592
    let deg = floor coord
    let min = coord - deg
    PI * (deg + 5.0 * min / 3.0) / 180.0

//For computing the geographical distance:
//
// RRR = 6378.388;
//
// q1 = cos( longitude[i] - longitude[j] );
// q2 = cos( latitude[i] - latitude[j] );
// q3 = cos( latitude[i] + latitude[j] );
// dij = (int) ( RRR * acos( 0.5*((1.0+q1)*q2 - (1.0-q1)*q3) ) + 1.0);
let w (x1: float) (y1: float) (x2: float) (y2: float) weightType : int =
    match weightType with
    | Geo ->
        let x1 = radian x1
        let y1 = radian y1
        let x2 = radian x2
        let y2 = radian y2
        let RRR = 6378.388
        let q1 = cos (y1 - y2)
        let q2 = cos (x1 - x2)
        let q3 = cos (x1 + x2)
        int (RRR * acos(0.5 * ((1.0 + q1) * q2 - (1.0 - q1) * q3)) + 1.0)
    | Eucl2d ->
        let distance = System.Math.Sqrt ( ((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)) )
        int (System.Math.Round distance)
