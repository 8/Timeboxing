module Timeboxing.Views

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Shapes
open Avalonia.Input
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Styling
open Avalonia.VisualTree
open FSharp.Control.Reactive
open Timeboxing.ControlExtensions
open Timeboxing.Svg

let colorPalette = ColorPalettes.trueGray

let completedView (state : State) : IControl =
  let transparent = SolidColorBrush Colors.Transparent
  let filled = SolidColorBrush colorPalette.Color700
  let getFill count i = if count > i then filled else transparent
  let size = 10.
  let rectangles =
    [| 0..7 |]
    |> Array.map (fun _ ->
      Rectangle (
        Width = size,
        Height = size,
        Fill = filled,
        Margin = Thickness (2., 4.)
      ))

  state.CompletedTimeboxes
  |> Observable.subscribe (fun count ->
    for i in 0..rectangles.Length - 1 do
      rectangles.[i].Fill <- getFill count i 
    ()
  )
  |> ignore

  upcast Border (
    BorderBrush = SolidColorBrush Colors.Transparent,
    Margin = Thickness (4., 0., 0., 0.),
    Child =
      StackPanel (
        Orientation = Orientation.Horizontal,
        Children = (rectangles |> Array.map (fun r -> upcast r))
      )
    )
  
let closeButtonStyle (button : Button) =
  button.Content <- "⨉"
  button.Foreground <- SolidColorBrush colorPalette.Color700
  button.Background <- SolidColorBrush colorPalette.Color200
  button.Margin <- Thickness 0.
  button.Padding <- Thickness (4., 0.)
  button.Styles <- [
    Style (
      Selector = Selectors.OfType<Button>(null).Class(":pointerover").Template(),
      Setters = [
        Setter(Property = Button.BackgroundProperty, Value = SolidColorBrush ColorPalettes.red.Color600)
        Setter(Property = Button.ForegroundProperty, Value = SolidColorBrush ColorPalettes.white)
      ]
    )
  ]
  button

let titleView (state : State) : IControl =

  upcast Border (
    Background = SolidColorBrush colorPalette.Color200,
    Child =
      Grid (
        ColumnDefinitions = ColumnDefinitions "*, Auto",
        Children = [
          completedView state
          Button (
            Column = 1,
            SetupButton = (fun (button : Button) ->
              button.Click
              |> Observable.subscribe (fun _ -> state.IsExitRequested.OnNext true)
              |> ignore
            )
          ) |> closeButtonStyle
        ]
    ),
    SetupControl = fun border ->
      border.PointerPressed
      |> Observable.startWith []
      |> Observable.subscribe (fun e ->
        let root = border.GetVisualRoot ()
        match root with
        | :? Window as window -> window.BeginMoveDrag(e)
        | _ -> ()
        ()
        )
      |> ignore
  )

let timerView (state : State) : IControl =

  let fontFamily =
    FontFamily "Bahnschrift"
    //FontFamily (Uri "avares://Timeboxing", "assets/Segment7.otf#Segment7 Standard")
  
  let styler (control : IControl) =
    match control with
    | :? TextBlock as textBlock ->
      textBlock.FontSize <- 54.
      textBlock.FontFamily <- fontFamily
      textBlock.Padding <- Thickness (0., 0., 0., -16.)
      textBlock.Width <- 30.
      textBlock.HorizontalAlignment <- HorizontalAlignment.Center
      textBlock.TextAlignment <- TextAlignment.Center
    | _ -> ()
    ()
  
  (* use multiple textblocks instead of a single one, to compensate for not monospace font *)
  let subscribe (control : IControl) letterIndex =
    
    let subscribeFormattedTimeLetter (textBlock : TextBlock) (letterIndex : int) =
      state.FormattedTime.Subscribe(fun formattedTime ->
        textBlock.Text <- formattedTime.[letterIndex..letterIndex]
        ) |> ignore
      ()
      
    match control with
    | :? TextBlock as textBlock -> subscribeFormattedTimeLetter textBlock letterIndex
    | _ -> ()
    
  upcast Border (
    Background = SolidColorBrush colorPalette.Color700,
    Child =
      Grid (
        Margin = Thickness 4.,
        ColumnDefinitions = ColumnDefinitions ("*,*,*,*,*"),
        Children = [
          TextBlock (
            Column = 0,
            SetupControl = (fun control ->
              styler control
              subscribe control 0
            )
          )
          TextBlock (
            Column = 1,
            SetupControl = (fun control ->
              styler control
              subscribe control 1
            )
          )
          TextBlock (
            Column = 2,
            Text = ":",
            Margin = Thickness 0.,
            FontSize = 54.,
            SetupControl = styler,
            Width = 10.
          )
          TextBlock (
            Column = 3,
            SetupControl = (fun control ->
              styler control
              subscribe control 3
            )
          )
          TextBlock (
            Column = 4,
            SetupControl = (fun control ->
              styler control
              subscribe control 4
            )
          )
        ]
      )
  )
  
let buttonsView (state : State) : IControl =
  let styler (control : IControl) =
    match control with
    | :? Button as b ->
      b.HorizontalAlignment <- HorizontalAlignment.Center
      b.Margin <- Thickness 0.
      b.Padding <- Thickness 4.
      b.Foreground <- SolidColorBrush colorPalette.Color050
      b.Background <- SolidColorBrush colorPalette.Color500
      b.Cursor <- Cursor StandardCursorType.Hand
      b.Styles.Clear ()
      b.Styles <- [
        Style (
          Selector = Selectors.OfType<Button>(null).Class(":pointerover").Template(),
          Setters = [
            Setter (Property = Button.BackgroundProperty, Value = SolidColorBrush colorPalette.Color600)
          ]
        )
        Style (
          Selector = Selectors.OfType<Button>(null).Class(":pressed").Template(),
          Setters = [
            Setter ( Property = Button.BackgroundProperty, Value = SolidColorBrush colorPalette.Color700)
          ]
        )
      ]
      ()
    | _ -> ()
    
  upcast Border (
    Background = SolidColorBrush colorPalette.Color500,
    Child =
      Grid (
        Height = 30.,
        Margin = Thickness (4., 0.),
        //ColumnDefinitions = ColumnDefinitions ("*, *, *, *"),
        ColumnDefinitions = ColumnDefinitions ("*, *, *"),
        Children = [
          Button (
            Column = 0,
            Content = Image (Source = fromAsset Assets.clock),
            SetupControl = (fun control ->
              styler control
              match control with
              | :? Button as button ->
                button.Click
                |> Observable.subscribe (fun _ ->
                  state.IsTimerRunning.OnNext(true)
                  state.DurationType.OnNext(Work)
                  state.Time.OnNext(state.WorkDuration.Value)
                  ())
                |> ignore
                ()
              | _ -> ()
              ()
            )
          )
          Button (
            Column = 1,
            Content = Image (Source = fromAsset Assets.coffee),
            SetupControl = (fun control ->
              styler control
              match control with
              | :? Button as button ->
                button.Click
                |> Observable.subscribe (fun _ ->
                  state.IsTimerRunning.OnNext(true)
                  state.DurationType.OnNext(Break)
                  state.Time.OnNext(state.BreakDuration.Value)
                  ()
                )
                |> ignore
              | _ -> ()
            )
          )
          Button (
            Column = 2,
            Content = Image (Source = fromAsset Assets.playPause),
            SetupControl = styler,
            SetupButton = (fun (button : Button) ->
              button.Click
              |> Observable.subscribe (fun _ ->
                state.IsTimerRunning.OnNext(not state.IsTimerRunning.Value)
                ()
                )
              |> ignore
              ()) 
          )
          //Button (
          //  Column = 3,
          //  Content = Image (Source = loadSvgFromAssets Assets.settings),
          //  Setup = styler
          //)
        ]
      )
  )

let contextMenuView (state : State)  =
    
  ContextMenu (
    Items = [
      MenuItem (
        Header = "Reset Completed Timeboxes",
        SetupMenuItem = fun menuItem ->
          menuItem.Click
          |> Observable.subscribe (fun _ -> state.CompletedTimeboxes.OnNext(0))
          |> ignore
      ) :> Control
      CheckBox (
        Content = "Show Always on Top",
        Margin = Thickness 0.,
        SetupCheckBox = fun checkBox ->
          
          state.IsAlwaysOnTop.Subscribe(fun isAlwaysOnTop -> checkBox.IsChecked <- isAlwaysOnTop)
          |> ignore
          
          let updateIsAlwaysOnTop _ =
            checkBox.IsChecked
            |> Option.ofNullable
            |> Option.iter state.IsAlwaysOnTop.OnNext

          checkBox.Checked |> Observable.subscribe updateIsAlwaysOnTop |> ignore
          checkBox.Unchecked |> Observable.subscribe updateIsAlwaysOnTop |> ignore
      ) :> Control

    ]
  )

let mainView state : IControl =
  upcast StackPanel (
    ContextMenu = (contextMenuView state),
    Orientation = Orientation.Vertical,
    Children = [
      titleView state
      timerView state
      buttonsView state
    ]
  )

let mainWindow state =
  
  Window (
    Content = mainView state,
    SystemDecorations = SystemDecorations.None,
    SizeToContent = SizeToContent.WidthAndHeight,
    CanResize = false,
    SetupWindow = (fun window ->
      state.Title
      |> Observable.subscribe (fun s -> window.Title <- s)
      |> ignore
      
      state.IsAlwaysOnTop
      |> Observable.subscribe (fun isAlwaysOnTop -> window.Topmost <- isAlwaysOnTop)
      |> ignore
    )
  )

let settingsView state : IControl =
  let styler (control : Control) =
    match control with
    | :? CheckBox as checkBox ->
      
      ()
    | :? Button as button ->
      button.Width <- 80.
      button.HorizontalAlignment <- HorizontalAlignment.Center
      button.HorizontalContentAlignment <- HorizontalAlignment.Center
      button.Padding <- Thickness (4.)
    | :? TextBlock as textBlock ->
      textBlock.VerticalAlignment <- VerticalAlignment.Center
      textBlock.HorizontalAlignment <- HorizontalAlignment.Right
      textBlock.Margin <- Thickness 4.
    | :? TextBox as textBox ->
      textBox.Margin <- Thickness (8.,4.,4.,4.)
      textBox.Padding <- Thickness 4.
      textBox.HorizontalContentAlignment <- HorizontalAlignment.Right
      textBox.FontFamily <- FontFamily "Consolas"
      textBox.FontSize <- 18.
      textBox.BorderThickness <- Thickness 2.
      ()
    | _ -> ()
    
  let title =
     Border (
        Background = SolidColorBrush colorPalette.Color200,
        Child =
          Grid (
            ColumnDefinitions = ColumnDefinitions ("*,Auto"),
            Children = [
              Button (Column = 1) |> closeButtonStyle
            ]
          )
      )
     
  let buttons =
    Border (
      Margin = Thickness 4.,
      Child =
        Grid (
          ColumnDefinitions = ColumnDefinitions ("*,*"),
          RowDefinitions = RowDefinitions("Auto"),
          Children = [
            Button (Content = "Cancel", Column = 0, SetupControl = styler)
            Button (Content = "Apply", Column = 1, SetupControl = styler)
          ]
        )
    )
    
  let controls =
    Border (
      Padding = Thickness 4.,
      Child =
        Grid (
          ColumnDefinitions = ColumnDefinitions ("Auto, Auto"),
          RowDefinitions = RowDefinitions ("Auto, Auto, Auto"),
          Children = [
            TextBlock (Text = "Work Duration", Row = 0, Column = 0, SetupControl = styler )
            
            TextBox (Text = "25:00", Row = 0, Column = 1, SetupControl = styler)
            
            TextBlock (Text = "Break Duration", Row = 1, Column = 0, SetupControl = styler)
            
            TextBox (Text = "5:00", Row = 1, Column = 1, SetupControl = styler)
            
            CheckBox (Content = "Bring to Foreground", Row = 2, ColumnSpan = 2, SetupControl = styler)
          ]
        )
    )
     
  upcast Border (
    Background = SolidColorBrush colorPalette.Color700,
    Child = StackPanel (
      Children = [
        title
        controls
        buttons
      ]
    )
  )

