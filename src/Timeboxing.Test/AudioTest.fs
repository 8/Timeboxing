module Timeboxing.Test.AudioTest

open System
open System.IO
open System.Threading
open NLayer.NAudioSupport
open Xunit
open FsUnit.Xunit
open NAudio.Wave

let mp3 = "notification.mp3"

[<Fact>]
let ``mp3 test file is available`` () =
  File.Exists(mp3) |> should be True

[<Fact>]
let ``test default audio output`` () =
  use out = new WasapiOut()
  out |> should not' (be Null)
  ()

[<Fact>]
let ``load mp3 audio file using NLayer`` () =
  let builder = Mp3FileReaderBase.FrameDecompressorBuilder(fun wf -> upcast new Mp3FrameDecompressor(wf))
  use reader = new Mp3FileReaderBase(mp3, builder)
  ()

[<Fact>]
let ``play mp3 audio file`` () =
  use out = new WasapiOut()
  
  let builder = Mp3FileReaderBase.FrameDecompressorBuilder(fun wf -> upcast new Mp3FrameDecompressor(wf))
  use reader = new Mp3FileReaderBase(mp3, builder)

  out.Init(reader)
  out.Play()
  
  while out.PlaybackState = PlaybackState.Playing do
    Thread.Sleep(TimeSpan.FromMilliseconds(100.))
  ()
  