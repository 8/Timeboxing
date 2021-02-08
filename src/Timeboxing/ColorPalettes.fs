module Timeboxing.ColorPalettes

open System
open Avalonia.Media

module Color =
  let fromRgb (r, g, b) = Color.FromRgb (byte r, byte g, byte b)

type Palette = {
  AllColors : Color array
  Color050 : Color
  Color100 : Color
  Color200 : Color
  Color300 : Color
  Color400 : Color
  Color500 : Color
  Color600 : Color
  Color700 : Color
  Color800 : Color
  Color900 : Color
}

module Palette =
  let init (colors : Color list) : Palette =

    if colors.Length <> 10 then
      raise <| ArgumentException ("Needs exactly 10 Colors for a palette", nameof colors)

    {
     AllColors = colors |> List.toArray
     Color050 = colors.[0]
     Color100 = colors.[1]
     Color200 = colors.[2]
     Color300 = colors.[3]
     Color400 = colors.[4]
     Color500 = colors.[5]
     Color600 = colors.[6]
     Color700 = colors.[7]
     Color800 = colors.[8]
     Color900 = colors.[9]
    }

let trueGray =
  [
  (250, 250, 250)
  (245, 245, 245)
  (229, 229, 229)
  (212, 212, 212)
  (163, 163, 163)
  (115, 115, 115)
  ( 82,  82,  82)
  ( 64,  64,  64)
  ( 38,  38,  38)
  ( 23,  23,  23)
  ]
  |> List.map Color.fromRgb
  |> Palette.init

let gray =
  [
  (250, 250, 250)
  (244, 244, 245)
  (228, 228, 231)
  (212, 212, 216)
  (161, 161, 170)
  (113, 113, 122)
  ( 82,  82,  91)
  ( 63,  63,  70)
  ( 39,  39,  42)
  ( 24,  24,  27)
  ]
  |> List.map Color.fromRgb
  |> Palette.init
  
let coolGray =
  [
  (249, 250, 251)
  (243, 244, 246 )
  (229, 231, 235 )
  (209, 213, 219 )
  (156, 163, 175 )
  (107, 114, 128 )
  ( 75,  85,  99 )
  ( 55,  65,  81 )
  ( 31,  41,  55 )
  ( 17,  24,  39 )
  ]

let blueGray =
  [
    (248, 250, 252)
    (241, 245, 249)
    (226, 232, 240)
    (203, 213, 225)
    (148, 163, 184)
    (100, 116, 139)
    ( 71,  85, 105)
    ( 51,  65,  85)
    ( 30,  41,  59)
    ( 15,  23,  42)
  ]
  |> List.map Color.fromRgb
  |> Palette.init

let red =
  [
   (254, 242, 242)
   (254, 226, 226)
   (254, 202, 202)
   (252, 165, 165)
   (248, 113, 113)
   (239,  68,  68)
   (220,  38,  38)
   (185,  28,  28)
   (153,  27,  27)
   (127,  29,  29)
  ]
  |> List.map Color.fromRgb
  |> Palette.init
  
let white = (255, 255, 255) |> Color.fromRgb