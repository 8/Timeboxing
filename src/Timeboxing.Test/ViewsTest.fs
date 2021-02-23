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
  let state count = 
    let state = State.init ()
    state.CompletedTimeboxes.OnNext(count)
    state
    
  renderSizeAuto "titleView-empty" (fun () -> titleView (state 0))
  renderSizeAuto "titleView-half" (fun () -> titleView (state 4))
  renderSizeAuto "titleView-full" (fun () -> titleView (state 8))

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
  
  
type ViewFactory = State -> IControl
type StateFactory = unit -> State
type ViewWithStateFactory = unit -> IControl
  
[<Fact>]
let screenshots () =
  
  let controlWithState (controlFactory : ViewFactory) (stateFactory: StateFactory) : ViewWithStateFactory =
    fun () -> stateFactory () |> controlFactory
      
  let stateFactory mutate : StateFactory =
    fun () ->
      let s = State.init ()
      mutate s
      s
  
  let startWork = stateFactory (fun s -> ())
  let startBreak = stateFactory (fun s -> s.Time.OnNext(TimeSpan.FromMinutes(5.)))
  let half = stateFactory (fun s -> s.CompletedTimeboxes.OnNext 4)
  let full = stateFactory (fun s -> s.CompletedTimeboxes.OnNext 8)
  let action = stateFactory (fun s ->
    s.Time.OnNext(TimeSpan (0, 2, 58))
    s.CompletedTimeboxes.OnNext 2)

  let mainViewFactory = controlWithState mainView
  let render name state = renderSizeAuto $"screenshot-{name}" (mainViewFactory state)
  
  render "startwork" startWork
  render "startbreak" startBreak
  render "half" half
  render "full" full
  render "action" action
  
  ()
