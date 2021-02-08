module Timeboxing.Test.ColorPalettesTest

open Xunit
open Timeboxing

[<Fact>]
let trueGrayTest () =
  let colorPalette = ColorPalettes.trueGray
  ()