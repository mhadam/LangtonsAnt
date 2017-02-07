module Ant

open System.Drawing
open System.IO
open System
open ColorMine.ColorSpaces

let goldenRatio = (1.0 + sqrt(5.0)) / 2.0

let random = Random()
let initialColor =
    let h = float(random.Next(100))*255.0/100.0
    let s = 0.5 * 255.0
    let v = 0.95 * 255.0
    Hsv(H=h, S=s, V=v)

let generateColor (c1 : Hsv) =
    let next = (c1.H / 255.0 + 1.0 / goldenRatio) % 1.0
    let s = 0.5 * 255.0
    let v = 0.95 * 255.0
    Hsv(H = next * 255.0, S = s, V = v)

let convertDirs (ds:string) =
    ds.ToCharArray()
    |> Array.map (fun x -> match x with
                            | 'L' -> 90.0
                            | 'R' -> -90.0
                            | _ -> 0.0)

let iterations = 100000
let dirs = "LRLRLRRR" |> convertDirs
let colors = seq { yield initialColor; yield! (Seq.unfold (fun acc -> Some(generateColor acc, generateColor acc)) initialColor |> Seq.take dirs.Length) }

type ant = {
    dimension : int*int
    position: int*int
    color: Color
    vector: float
    grid: int[,]}

let bitmap = new Bitmap(1000, 1000)

for x in 0..bitmap.Width-1 do
    for y in 0..bitmap.Height-1 do
        bitmap.SetPixel(x, y, Color.White)

let dimension = (1000, 1000)
let (x, y) = dimension
let initialAnt = {
    dimension = dimension
    position = ((fst dimension) / 2 - 1, (snd dimension) / 2 - 1)
    color = Color.Black
    vector = 0.0
    grid = Array2D.zeroCreate<int> x y }

let moveAnt ant =
    let { position = position } = ant
    let (x, y) = position
    match ant with
    | { vector = 0.0 } when x < bitmap.Width-1 -> { ant with position = (x + 1, y) }
    | { vector = 90.0 } when y < bitmap.Width-1-> { ant with position = (x, y + 1)}
    | { vector = 180.0 } when x > 0 -> { ant with position = (x - 1, y)}
    | { vector = 270.0 } when y > 0 -> { ant with position = (x, y - 1)}
    | _ -> ant

let addAngle ant angle =
    let curAngle = ant.vector
    { ant with vector = (curAngle + angle) % 360.0 }

let workAnt ant =
    let {position = position } = ant
    let (x, y) = position
    let color = Array2D.get ant.grid x y
    let newVector = ant.vector + dirs.[color]

    if color >= dirs.Length then
        Array2D.set ant.grid x y 0
    else
        Array2D.set ant.grid x y (color+1)

    moveAnt { ant with vector = newVector}