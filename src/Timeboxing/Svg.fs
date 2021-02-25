module Timeboxing.Svg

open System.IO
open Avalonia.Media

let fromStream (stream : Stream) : IImage =
  let source = new Avalonia.Svg.Skia.SvgSource ()
  source.Load(stream) |> ignore
  let image =  Avalonia.Svg.Skia.SvgImage(Source = source)
  upcast image

let fromAsset asset =
  Assets.streamFrom asset |> fromStream
