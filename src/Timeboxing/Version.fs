module Timeboxing.Version

open System.Reflection

let version =
  let v = Assembly.GetExecutingAssembly().GetName().Version
  $"{v.Major}.{v.Minor}.{v.Build}"

()