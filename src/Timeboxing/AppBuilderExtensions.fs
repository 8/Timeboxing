module Timeboxing.AppBuilderExtensions

open System
open System.Runtime.CompilerServices

open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Markup.Xaml
open Avalonia.Styling

[<Extension>]
type AppBuilderExtensions =

  static member inline private loadStyle (source : string) =
    let uri = Uri source
    AvaloniaXamlLoader.Load (uri) :?> IStyle
    //StyleInclude (uri, Source = uri)

  [<Extension>]
  static member inline AddStyle<'T when 'T :> AppBuilderBase<'T>> (appBuilder : 'T, url) : 'T =
    appBuilder.AfterSetup(
      fun appBuilder ->
        AppBuilderExtensions.loadStyle url
        |> appBuilder.Instance.Styles.Add
    )

  [<Extension>]
  static member inline AddStyleDefaultLight<'T when 'T :> AppBuilderBase<'T>> (appBuilder : 'T): 'T =
    appBuilder
      .AddStyle("resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default")
      .AddStyle ("resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default")

  [<Extension>]
  static member inline AddStyleDefaultDark<'T when 'T :> AppBuilderBase<'T>> (appBuilder : 'T): 'T =
    appBuilder
      .AddStyle("resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default")
      .AddStyle ("resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default")

  [<Extension>]
  static member inline AddStyleFluentLight<'T when 'T :> AppBuilderBase<'T>> (appBuilder : 'T): 'T =
    appBuilder.AddStyle ("avares://Avalonia.Themes.Fluent/FluentLight.xaml")

  [<Extension>]
  static member inline AddStyleFluentDark<'T when 'T :> AppBuilderBase<'T>> (appBuilder : 'T) : 'T =
    appBuilder.AddStyle ("avares://Avalonia.Themes.Fluent/FluentDark.xaml")
 
  [<Extension>]
  static member inline UseMainWindow<'T, 'TWindow when 'T :> AppBuilderBase<'T> and 'TWindow :> Window and 'TWindow : (new : unit -> 'TWindow)> (appBuilder : 'T) : 'T =
    appBuilder.AfterSetup (
      fun appBuilder ->
        match appBuilder.Instance.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as lifetime -> lifetime.MainWindow <- new 'TWindow ()
        | _ -> ()
    )

  [<Extension>]
  static member inline UseMainWindowFactory<'T, 'TWindow when 'T :> AppBuilderBase<'T> and 'TWindow :> Window> (appBuilder : 'T, windowFactory : unit -> 'TWindow) : 'T =
    appBuilder.AfterSetup (
      fun appBuilder ->
        match appBuilder.Instance.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as lifetime -> lifetime.MainWindow <- windowFactory ()
        | _ -> ()
    )
