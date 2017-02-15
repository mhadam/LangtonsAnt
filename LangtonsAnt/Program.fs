// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Ant

[<EntryPoint>]
let main argv = 
    let convertDirs (ds:string) =
        ds.ToCharArray()
        |> Array.map (fun x -> match x with
                               | 'L' -> 90.0
                               | 'R' -> -90.0
                               | _ -> 0.0)

    let convertOrientation (os:string) =
        match os with
        | "N" -> 0.0
        | "S" -> 180.0
        | "W" -> 90.0
        | "E" -> 270.0
        | _ -> 0.0

    let outputName (dirs : string) (iterations : string) =
        sprintf "%s_%s.png" dirs iterations

    if argv.Length = 3 then
        let dirs : float [] = argv.[0] |> convertDirs
        let iterations : int = argv.[1] |> int
        let orientation : float = argv.[2] |> convertOrientation
        ignore (executeAnt 1600 900 dirs iterations orientation (outputName argv.[0] argv.[1]))
        0
    else
        0 // return an integer exit code
