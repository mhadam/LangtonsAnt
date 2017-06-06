module Ant

open System.Drawing
open System.IO
open System
open ColorMine.ColorSpaces

type turn =
    | Left
    | Right

type vector =
    | North
    | South
    | East
    | West

type ant = {
    dimensions : int*int
    position: int*int
    colors: seq<Color>
    vector: vector
    grid: int[,] }

let makeColorPalette (turns : turn option []) =
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

    let hsvColors =
        seq { yield initialColor; yield! (Seq.unfold (fun acc -> Some(generateColor acc, generateColor acc)) initialColor |> Seq.take turns.Length) }
    
    let getRGBColor (hsv : Hsv) =
        let rgb = hsv.To<Rgb>()
        Color.FromArgb(255, int(rgb.R), int(rgb.G), int(rgb.B))

    Seq.map getRGBColor hsvColors

let moveAnt ant =
    let (x, y) = ant.position
    let (xLength, yLength) = (Array2D.length1 ant.grid, Array2D.length2 ant.grid)
    let firstMove = 
        match ant with
        | { vector = East } when x < xLength-1 -> { ant with position = (x + 1, y) }
        | { vector = North } when y < yLength-1-> { ant with position = (x, y + 1)}
        | { vector = West } when x > 0 -> { ant with position = (x - 1, y)}
        | { vector = South } when y > 0 -> { ant with position = (x, y - 1)}
        | _ -> ant
    
    let (x1, y1) = firstMove.position

    match firstMove.position with
    | x, y when x = xLength && y = yLength -> { firstMove with position = (0, 0) }
    | x, _ when x = xLength -> { firstMove with position = (0, y1) }
    | _, y when y = yLength -> { firstMove with position = (x1, 0) }
    | -1, -1 -> { firstMove with position = (xLength-1, yLength-1) }
    | -1, _ -> { firstMove with position = (xLength-1, y1) }
    | _, -1 -> { firstMove with position = (x1, yLength-1) }
    | _, _ -> firstMove

let updateVector ant (turn : turn option) =
    let initialVector = ant.vector
    let newVector =
        match turn with
        | Some turn ->
            match (turn, initialVector) with
            | (Left, East) -> North
            | (Left, North) -> West
            | (Left, West) -> South
            | (Left, South) -> East
            | (Right, East) -> South
            | (Right, North) -> East
            | (Right, West) -> North
            | (Right, South) -> West
        | None -> initialVector

    { ant with vector = newVector }

let saveAs (name : string) (bitmap : Bitmap) =
    let path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name)
    bitmap.Save(path)

let outputBitmap ant =
    let { dimensions = dimensions } = ant
    let (xSize, ySize) = dimensions
    let outputBitmap =
        new Bitmap(xSize, ySize)

    let getColor index =
        Seq.item index ant.colors

    let pixelFunc x y v =
        outputBitmap.SetPixel(x, y, getColor v)
    
    let outputGrid =
        Array2D.iteri pixelFunc ant.grid
    outputBitmap


let executeAnt xSize ySize (directions : turn option []) iterations orientation outputName =
    let initialAnt = {
        dimensions = (xSize, ySize)
        position = (xSize / 2 - 1, ySize / 2 - 1)
        colors = makeColorPalette directions
        vector = orientation
        grid = Array2D.zeroCreate<int> xSize ySize }

    let workAnt ant =
        let { position = position } = ant
        let (x, y) = position
        let color = Array2D.get ant.grid x y
        let turnedAnt = updateVector ant directions.[color]

        if color >= directions.Length-1 then
            Array2D.set ant.grid x y 0
        else
            Array2D.set ant.grid x y (color+1)

        moveAnt turnedAnt

    let rec callWork (ant : ant) (n : int) : ant =
        match n with
        | x when x > 1 -> callWork (workAnt ant) (n-1)
        | x when x = 1 -> (workAnt ant)
        | _ -> (workAnt ant)

    let final = callWork initialAnt iterations
    saveAs outputName (outputBitmap final)