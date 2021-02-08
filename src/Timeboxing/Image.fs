module Timeboxing.Image

open System
open System.Reflection
open Avalonia
open Avalonia.Media
open Avalonia.Media.Imaging
open Avalonia.Platform

let completeUri (uri : string) =
  if not (uri.StartsWith "avares://") then
    $"avares://{Assembly.GetEntryAssembly().GetName().Name}{uri}"
  else
    uri

let loadImage uri : IImage =
  let assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>()
  let uri = uri |> completeUri |> Uri
  let stream = assetLoader.Open(uri, null)
  upcast new Bitmap (stream)

let loadSvg uri : IImage =
  let assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>()
  let uri = uri |> completeUri |> Uri
  let stream = assetLoader.Open(uri, null)
  let source = new Avalonia.Svg.Skia.SvgSource ()
  source.Load(stream) |> ignore
  let image =  Avalonia.Svg.Skia.SvgImage(Source = source)
  upcast image

