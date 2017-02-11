module Ant

open System.Drawing
open System.IO
open System
open ColorMine.ColorSpaces

let goldenRatio = (1.0 + sqrt(5.0)) / 2.0

let random = Random()
let initialColor =
    let h = float(random.Next(100))*360.0/100.0
    let s = 0.5
    let v = 0.95
    Hsv(H=h, S=s, V=v)

let generateColor (c1 : Hsv) =
    let next = (c1.H / 360.0 + 1.0 / goldenRatio) % 1.0
    let s = 0.5
    let v = 0.95
    Hsv(H = next * 360.0, S = s, V = v)

let convertDirs (ds:string) =
    ds.ToCharArray()
    |> Array.map (fun x -> match x with
                            | 'L' -> 90.0
                            | 'R' -> -90.0
                            | _ -> 0.0)

let iterations = 10000000
let dirs = "LRLRLRRR" |> convertDirs
let colors = seq { yield initialColor; yield! (Seq.unfold (fun acc -> Some(generateColor acc, generateColor acc)) initialColor |> Seq.take dirs.Length) }

type ant = {
    dimension : int*int
    position: int*int
    color: Color
    vector: float
    grid: int[,] }

let dimension = (1000, 1000)
let (x, y) = dimension
let initialAnt = {
    dimension = dimension
    position = ((fst dimension) / 2 - 1, (snd dimension) / 2 - 1)
    color = Color.Black
    vector = 0.0
    grid = Array2D.zeroCreate<int> x y }

let moveAnt ant =
    //printfn "%A" (ant.vector, ant.position)
    let (x, y) = ant.position
    match ant with
    | { vector = 0.0 } when x < bitmap.Width-1 -> { ant with position = (x + 1, y) }
    | { vector = 90.0 } when y < bitmap.Width-1-> { ant with position = (x, y + 1)}
    | { vector = 180.0 } when x > 0 -> { ant with position = (x - 1, y)}
    | { vector = 270.0 } when y > 0 -> { ant with position = (x, y - 1)}
    | _ -> ant

let addAngle ant angle =
    let curAngle = ant.vector
    let newVector = 
        match (curAngle + angle) % 360.0 with
        | x when x < 0.0 -> (curAngle + angle) % 360.0 + 360.0
        | _ -> (curAngle + angle) % 360.0
    
    { ant with vector = newVector }

let workAnt ant =
    let { position = position } = ant
    let (x, y) = position
    let color = Array2D.get ant.grid x y
    let turnedAnt = addAngle ant dirs.[color]

    if color >= dirs.Length-1 then
        Array2D.set ant.grid x y 0
    else
        Array2D.set ant.grid x y (color+1)

    moveAnt turnedAnt

let saveAs (name : string) (bitmap : Bitmap) =
    let path = Path.Combine(__SOURCE_DIRECTORY__, name)
    bitmap.Save(path)

let rec callWork (ant : ant) (n : int) : ant =
    match n with
    | x when x > 1 -> callWork (workAnt ant) (n-1)
    | x when x = 1 -> (workAnt ant)
    | _ -> (workAnt ant)

let final = callWork initialAnt iterations

let getRGBColor (value : int) (ant : ant) =
    let rgb = (Seq.item value colors).To<Rgb>()
    let rgbColor = Color.FromArgb(255, int(rgb.R), int(rgb.G), int(rgb.B))
    rgbColor

let outputBitmap ant =
    let outputBitmap =
        new Bitmap(1000, 1000)

    let pixelFunc x y v =
        if v <> 0 then
            printfn "%A" v
        outputBitmap.SetPixel(x, y, (getRGBColor v ant))
    let outputGrid =
        Array2D.iteri pixelFunc ant.grid
    outputBitmap

saveAs "langton.png" (outputBitmap final)