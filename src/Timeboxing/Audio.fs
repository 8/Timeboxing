module Timeboxing.Audio

open System
open System.IO
open FSharp.Control.Reactive
open LibVLCSharp.Shared

module Internal =
  let init () =
    try
      Core.Initialize()
      new LibVLC() |> Some
    with _ -> None

  let defaultPath () =
    Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Media", "Windows Notify.wav")

  let mediaFrom (path : string) (vlc : LibVLC) =
    new Media(vlc, path, FromType.FromPath)
    
  let playerFrom (media : Media) =
    new MediaPlayer (media)

  let play (player : MediaPlayer) =
    player.Stop()
    player.Play()

open Internal

let setup (state : State) =
  
  let vlc = init ()
  let media = vlc |> Option.map (defaultPath() |> mediaFrom)
  let player = media |> Option.map playerFrom
  
  let subscribe (player : MediaPlayer) =
    state.CompletedTimeboxes
    |> Observable.pairwise
    |> Observable.filter (fun (t1, t2) -> t2 - t1 = 1)
    |> Observable.subscribe (fun _ -> play player |> ignore)
    
  let subscription = player |> Option.map subscribe
    
  let dispose (disposeable : IDisposable) = disposeable.Dispose()
  let disposeAll () =
    subscription |> Option.iter dispose
    player |> Option.iter dispose
    media |> Option.iter dispose
    vlc |> Option.iter dispose

  { new IDisposable with member this.Dispose() = disposeAll() }  


  