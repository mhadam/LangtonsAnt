// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Ant

[<EntryPoint>]
let main argv = 
    let convertDirs (ds:string) =
        ds.ToCharArray()
        |> Array.map (fun x -> match x with
                               | 'L' -> Some Left
                               | 'R' -> Some Right
                               | _ -> None
                               )

    let convertOrientation (os:string) =
        match os with
        | "N" -> North
        | "S" -> South
        | "W" -> East
        | "E" -> West
        | _ -> East

    let outputName (dirs : string) (iterations : string) =
        sprintf "%s_%s.png" dirs iterations

    if argv.Length = 3 then
        let turns : turn option [] = argv.[0] |> convertDirs
        let iterations : int = argv.[1] |> int
        let orientation : vector = argv.[2] |> convertOrientation
        ignore (executeAnt 1600 900 turns iterations orientation (outputName argv.[0] argv.[1]))
        0
    else
        printfn "%i arguments input, expected 3 arguments" argv.Length
        0 // return an integer exit code
