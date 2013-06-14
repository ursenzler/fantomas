﻿#r "../../lib/FSharp.Compiler.dll"

#load "SourceFilter.fs"
#load "FormatConfig.fs"
#load "SourceParser.fs"
#load "SourceTransformer.fs"
#load "CodePrinter.fs"
#load "CodeFormatter.fs"

open Fantomas.SourceFilter
open Fantomas.FormatConfig
open Fantomas.CodeFormatter

let config = FormatConfig.Default

let t01 = """
type DU =
    // 1
    | One
    // 2
    | Two
    // 3
    | Three
"""

let t02 = """
try 
    fst(find (fun (s, (s', ty)) -> 
                s' = s0 && can (type_match ty ty0) []) (!the_interface))
with
| Failure _ -> s0
"""
;;

let ys = (tokenize t01) 
let xs = filterComments (tokenize t01) 
         |> Seq.iter (fun (KeyValue(pos, s)) -> printfn "l:%O, c:%O, %A" pos.Line pos.Column s);;

printfn "Result:\n%s" <| formatSourceString false t01 config;;

printfn "Result:\n%s" <| formatSelectionFromString false (makeRange 6 5 6 51) t02 config;;

printfn "Tree:\n%A" <| parse false t01;;