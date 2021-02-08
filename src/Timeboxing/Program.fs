module Timeboxing.Program
open Avalonia
open Avalonia.ReactiveUI
open Timeboxing.AppBuilderExtensions

open Timeboxing.Views

let mainWindowFactory () =
  let state = State.init ()
  let window = mainWindow state
  state.IsExitRequested
  |> Observable.filter id
  |> Observable.subscribe (fun _ -> window.Close ())
  |> ignore
  
  window

let createApp () =
  AppBuilder.Configure<Application>()
    .AddStyleFluentDark()
    .UsePlatformDetect()
    .UseMainWindowFactory(mainWindowFactory)
    .UseReactiveUI()

let runApp args appBuilder =
  appBuilder.StartWithClassicDesktopLifetime args
  
[<EntryPoint>]
let main args = createApp () |> runApp args