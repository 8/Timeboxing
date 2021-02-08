module Timeboxing.Assets

type Asset =
  | SvgAsset of string
  
let coffee = SvgAsset "coffee"
let playPause = SvgAsset "playpause"
let settings = SvgAsset "settings"
let work = SvgAsset "work"
let clock = SvgAsset "clock"

let uri (asset : Asset) =
  match asset with
  | SvgAsset svg -> $"/assets/{svg}.svg"
  