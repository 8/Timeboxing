module Timeboxing.ControlExtensions

open Avalonia
open Avalonia.Controls
open Avalonia.Data
open Avalonia.Styling

type Panel with
  // override Children property to make it settable during construction
  member this.Children
    with get () = this.Children
    and set children =
      this.Children.Clear()
      this.Children.AddRange(children)

type Control with
  // extend all controls with the grid row / column properties
  member this.Row
    with get () = Grid.GetRow (this)
    and set row = Grid.SetRow (this, row)
  member this.Column
    with get () = Grid.GetColumn (this)
    and set column = Grid.SetColumn (this, column)
  member this.ColumnSpan
    with get () = Grid.GetColumnSpan (this)
    and set columnSpan = Grid.SetColumnSpan (this, columnSpan)
  member this.RowSpan
    with get () = Grid.GetRowSpan (this)
    and set rowSpan = Grid.SetRowSpan (this, rowSpan)
    
  member this.SetupControl
    with set (setup : Control -> unit) =
      setup this
    
  member this.Bind
    with set (prop : AvaloniaProperty, binding : Binding) =
      this.Bind(prop, binding) |> ignore

  member this.Styles
    with get () = this.Styles
    and set values =
      this.Styles.Clear ()
      this.Styles.AddRange values

type Button with
  member this.SetupButton
    with set (setup : Button -> unit) =
      setup this

type ListBox with
  member this.SetupListBox
    with set (action : ListBox -> unit) =
      action this

type Window with
  member this.SetupWindow
    with set (action : Window -> unit) =
      action this

type Style with
  member this.Setters
    with get () = this.Setters
    and set values =
      this.Setters.Clear ()
      for setter in values do
        this.Setters.Add setter
      ()