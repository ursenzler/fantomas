﻿module Fantomas.Tests.CommentTests

open NUnit.Framework
open FsUnit

open Fantomas.CodeFormatter
open Fantomas.Tests.TestHelper

[<Test>]
let ``should keep // comments after nowarn directives``() =
    formatSourceString false """#nowarn "51" // address-of operator can occur in the code
    """ config
    |> should equal """#nowarn "51" // address-of operator can occur in the code"""

[<Test>]
let ``should keep // comments before module definition``() =
    formatSourceString false """
// The original idea for this typeprovider is from Ivan Towlson
// some text
module FSharpx.TypeProviders.VectorTypeProvider

let x = 1""" config
    |> should equal """// The original idea for this typeprovider is from Ivan Towlson
// some text
module FSharpx.TypeProviders.VectorTypeProvider

let x = 1"""

[<Test>]
let ``should preserve comments on local let bindings``() =
    formatSourceString false """
let print_30_permut() = 

    /// declare and initialize
    let permutation : int array = Array.init n (fun i -> Console.Write(i+1); i)
    permutation
    """ config
    |> prepend newline
    |> should equal """
let print_30_permut() = 
    /// declare and initialize
    let permutation : int array = 
        Array.init n (fun i -> 
                Console.Write(i + 1)
                i)
    permutation"""

[<Test>]
let ``should keep xml documentation``() =
    formatSourceString false """
/// <summary>
/// Kill Weight Mud
/// </summary>
///<param name="sidpp">description</param>
///<param name="tvd">xdescription</param>
///<param name="omw">ydescription</param>
let kwm sidpp tvd omw =
    (sidpp / 0.052 / tvd) + omw

/// Kill Weight Mud
let kwm sidpp tvd omw = 1.0""" config
    |> prepend newline
    |> should equal """
/// <summary>
/// Kill Weight Mud
/// </summary>
///<param name="sidpp">description</param>
///<param name="tvd">xdescription</param>
///<param name="omw">ydescription</param>
let kwm sidpp tvd omw = (sidpp / 0.052 / tvd) + omw

/// Kill Weight Mud
let kwm sidpp tvd omw = 1.0"""

[<Test>]
let ``should preserve comments on members``() =
    formatSourceString false """
type MyClass2(dataIn) as self =
       let data = dataIn
       do self.PrintMessage()
       // Print a message to console
       member this.PrintMessage() =
           printf "Creating MyClass2 with Data %d" data
       // A static member
       static member Content = ()
    """ config
    |> prepend newline
    |> should equal """
type MyClass2(dataIn) as self = 
    let data = dataIn
    do self.PrintMessage()
    // Print a message to console
    member this.PrintMessage() = printf "Creating MyClass2 with Data %d" data
    // A static member
    static member Content = ()"""

[<Test>]
let ``should accommodate multiple kinds of comment``() =
    formatSourceString false """
module Tests = 
        (* Comments *)
        // This is another comment

        /// This is doc comment
    [<Test>]
    let ``this is a test``() = ()
    """ config
    |> prepend newline
    |> should equal """
module Tests = 
    (* Comments *)
    // This is another comment
    /// This is doc comment
    [<Test>]
    let ``this is a test``() = ()"""

[<Test>]
let ``should keep comments on union cases``() =
    formatSourceString false """
type DU =
    // 1
    | One
    // 2
    | Two
    // 3
    | Three
    """ config
    |> prepend newline
    |> should equal """
type DU = 
    // 1
    | One
    // 2
    | Two
    // 3
    | Three"""
