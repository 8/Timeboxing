module Timeboxing.Audio

open System
open System.IO
open FSharp.Control.Reactive
//open LibVLCSharp.Shared
//
//module Internal =
//  let init () =
//    try
//      Core.Initialize()
//      new LibVLC() |> Some
//    with _ -> None
//
//  let defaultPath () =
//    Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Media", "Windows Notify.wav")
//
//  let mediaFromPath (path : string) (vlc : LibVLC) =
//    new Media(vlc, path, FromType.FromPath)
//    
//  let mediaFromStream vlc stream =
//    new Media(vlc, new StreamMediaInput(stream))
//    
//  let mediaFromAsset vlc asset =
//    mediaFromStream vlc (Assets.streamFrom asset)
//    
//  let mediaNotification vlc =
//    lazy mediaFromAsset vlc Assets.notification
//    
//  let playerFrom (media : Media) =
//    new MediaPlayer (media)
//
//  let play (player : MediaPlayer) =
//    player.Stop()
//    player.Play()
//
//open Internal
//
//let setup (state : State) =
//
//  let vlc = init ()
//  let media = vlc |> Option.map mediaNotification
//  let player = media |> Option.map (fun media -> lazy (media.Value |> playerFrom))
//  
//  let subscribe (player : Lazy<MediaPlayer>) =
//    state.CompletedTimeboxes
//    |> Observable.pairwise
//    |> Observable.filter (fun (t1, t2) -> t2 - t1 = 1)
//    |> Observable.subscribe (fun _ -> play player.Value |> ignore)
//    
//  let subscription = player |> Option.map subscribe
//    
//  let dispose (disposeable : IDisposable) = disposeable.Dispose()
//  let disposeAll () =
//    subscription |> Option.iter dispose
//    player |> Option.iter (fun l -> dispose l.Value)
//    media |> Option.iter (fun l -> dispose l.Value)
//    vlc |> Option.iter dispose
//
//  { new IDisposable with member this.Dispose() = disposeAll() }  

let setup (state : State) =
  { new IDisposable with member this.Dispose() = () }
