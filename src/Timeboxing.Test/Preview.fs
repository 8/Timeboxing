module Timeboxing.Test.Preview

open System.IO
open Avalonia
open Avalonia.Controls
open Avalonia.Layout
open Timeboxing.Test.Render

type ControlFactory = unit -> IControl
  
let renderToStream appBuilder (settings : RenderSettings) (stream : Stream) (controlFactory : ControlFactory) =

  let initApp (appBuilder : AppBuilder) =
    appBuilder
      .SetupWithoutStarting()
    
  let ensureAppInitialized appBuilder =
    if Application.Current = null then
      initApp appBuilder |> ignore
      true
    else
      false

  // initialize the app
  ensureAppInitialized appBuilder |> ignore
    
  let control = controlFactory ()

  // create a window
  let window = Window ()

  let wrapper =
    ContentControl (
      Content = (control),
      (* some styles, like the fluent styles, depend on the window background style *)
      Background = window.Background
    )

  // associate the layoutable with the window, which in turn makes the application styles available
  window.Content <- wrapper

  // render the layoutable
  render settings stream (wrapper :> ILayoutable)
  ()
    
let renderToFile appBuilder (settings : RenderSettings) (filePath : string) (controlFactory : ControlFactory) =
  use fs = File.Create filePath
  renderToStream appBuilder settings fs controlFactory

 