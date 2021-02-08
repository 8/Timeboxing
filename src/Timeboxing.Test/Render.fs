module Timeboxing.Test.Render

open System.IO
open Avalonia
open Avalonia.Layout
open Avalonia.Media.Imaging
open Avalonia.Skia

type SizeType =
| Desired
| Exact

type RenderSettings = {
  Width : int
  Height : int
  Dpi : float
  SizeType : SizeType
}

module RenderSettings =
  let def = { Width = 800; Height = 800; Dpi = 96.; SizeType = SizeType.Desired }

let render (settings : RenderSettings) (stream : Stream) (control : ILayoutable) =
  SkiaPlatform.Initialize ()

  let dpiVector = Vector (settings.Dpi, settings.Dpi)
  let size = Size (float settings.Width, float settings.Height)

  control.Measure(size)

  let rect =
    match settings.SizeType with
    | Exact -> Rect size
    | Desired -> Rect control.DesiredSize

  control.Arrange(rect)

  let pixelSize =
    match settings.SizeType with
    | Exact -> PixelSize (settings.Width, settings.Height)
    | Desired -> PixelSize (int rect.Width, int rect.Height)

  use bitmap = new RenderTargetBitmap (pixelSize, dpiVector)

  bitmap.Render(control)
  bitmap.Save(stream)