open System.Drawing
open System.IO
open System

type ant = {
    position: int*int
    color: Color
    vector: float }

let convertDirs (ds:string) =
    ds.ToCharArray()
    |> Array.map (fun x -> match x with
                           | 'L' -> 90.0
                           | 'R' -> -90.0
                           | _ -> 0.0)

let dirs = "LRLRLRRR" |> convertDirs
let initialAnt = {
    position = (width / 2 - 1, height / 2 - 1)
    color = Color.Black
    vector = 0.0 }

