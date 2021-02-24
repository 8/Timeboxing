module Timeboxing.Program
open System
open Avalonia
open Avalonia.ReactiveUI
open Timeboxing.AppBuilderExtensions

open Timeboxing.Views

let mainWindowFactory state =
  let window = mainWindow state
  state.IsExitRequested
  |> Observable.filter id
  |> Observable.subscribe (fun _ -> window.Close ())
  |> ignore
  
  window

let createApp () =
  
  let state = State.init ()
  let audio = Audio.setup state
  
  let appBuilder =
    AppBuilder.Configure<Application>()
      .AddStyleFluentDark()
      .UsePlatformDetect()
      .UseMainWindowFactory(fun () -> mainWindowFactory state)
      .UseReactiveUI()
    
  (appBuilder, state, audio)

let runApp args (appBuilder, state : State, audio : IDisposable) =
  let res = appBuilder.StartWithClassicDesktopLifetime args
  audio.Dispose ()
  res

[<EntryPoint>]
let main args = createApp () |> runApp args