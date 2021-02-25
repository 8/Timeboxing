module Timeboxing.Program

open System
open Avalonia
open Avalonia.ReactiveUI
open Timeboxing.AppBuilderExtensions
open Timeboxing.Views

type AppState = {
  MainWindowFactory : unit -> Avalonia.Controls.Window
  State : State
  Audio : IDisposable
}

let init () =
  let mainWindowFrom state =
    let window = mainWindow state
    state.IsExitRequested
    |> Observable.filter id
    |> Observable.subscribe (fun _ -> window.Close ())
    |> ignore
    window
    
  let state = State.init ()
  let audio = Audio.init(state)
    
  {
    MainWindowFactory = (fun () -> mainWindowFrom state)
    State = state
    Audio = audio
  }

let createApp () =
    
  let appState = lazy init ()

  let appBuilder =
    AppBuilder.Configure<Application>()
      .AddStyleFluentDark()
      .UsePlatformDetect()
      .UseMainWindowFactory(fun () -> appState.Value.MainWindowFactory ())
      .UseReactiveUI()
      
  (appBuilder, appState)

let runApp args (appBuilder, appState) =
  appBuilder.StartWithClassicDesktopLifetime args

[<EntryPoint>]
let main args = createApp () |> runApp args