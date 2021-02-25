module Timeboxing.Assets

open System
open System.Reflection
open Avalonia
open Avalonia.Platform

type Asset =
  | SvgAsset of string
  | Mp3Asset of string
  
let coffee = SvgAsset "coffee"
let playPause = SvgAsset "playpause"
let settings = SvgAsset "settings"
let work = SvgAsset "work"
let clock = SvgAsset "clock"
let notification = Mp3Asset "notification"

let uri (asset : Asset) =
  match asset with
  | SvgAsset svg -> $"/assets/{svg}.svg"
  | Mp3Asset mp3 -> $"/assets/{mp3}.mp3"
  
let completeUri (uri : string) =
  if not (uri.StartsWith "avares://") then
    $"avares://{Assembly.GetExecutingAssembly().GetName().Name}{uri}"
  else
    uri

let streamFromUri (assetLoader: IAssetLoader) uri =
  let uri = uri |> completeUri |> Uri
  assetLoader.Open(uri, null)
  
let streamFromAssetLoader assetLoader asset =
  asset |> uri |> (streamFromUri assetLoader)

let assetLoader () =
  match AvaloniaLocator.Current.GetService<IAssetLoader>() with
  | null -> raise <| failwith $"Could not get asset loader. Is the Avalonia Application already initialized?"
  | loader -> loader

let streamFrom = streamFromAssetLoader (assetLoader ())
