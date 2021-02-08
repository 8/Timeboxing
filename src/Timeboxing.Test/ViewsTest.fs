module Timeboxing.Test.ViewsTest

open System.IO
open System
open System.Reflection

open Xunit

open Avalonia.Controls
open Timeboxing
open Timeboxing.Test.Preview
open Timeboxing.Test.Render
open Timeboxing.Program
open Timeboxing.Views

let render settings name (controlFactory  : unit -> IControl)  =
  let appBuilder = createApp ()
  let filePath =
    Path.Combine [|
      Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
      "screenshots"
      Assembly.GetExecutingAssembly().GetName().Name
      $"{name}.png"
    |]

  renderToFile appBuilder settings filePath controlFactory

let renderSizeAuto name controlFactory  =
  render RenderSettings.def name controlFactory

let renderSizeExact (width, height) name controlFactory =
  render
    { RenderSettings.def with Width = width; Height = height; SizeType = SizeType.Exact; }
    name
    controlFactory

[<Fact>]
let titleViewTest () =
  renderSizeExact (400, 20) "titleView" (State.init >> titleView)

[<Fact>]
let titleViewFullTest () =
  let state = 
    let state = State.init ()
    state.CompletedTimeboxes.OnNext(8)
    state

  renderSizeAuto "titleView-full" (fun () -> titleView state)

[<Fact>]
let buttonViewTest () =
  renderSizeAuto "buttonsView" (State.init >> buttonsView)
  
[<Fact>]
let timerViewTest() =
  renderSizeAuto "timerView" (State.init >> timerView)

[<Fact>]
let mainViewTest () =
  renderSizeAuto "mainView" (State.init >> mainView)
  
[<Fact>]
let settingsViewTest () =
  renderSizeAuto "settingsView" (State.init >> settingsView)