module Timeboxing.Test.AudioTest

open Xunit
open LibVLCSharp.Shared

[<Fact>]
let ``initialize native libraries`` () = Core.Initialize ()
