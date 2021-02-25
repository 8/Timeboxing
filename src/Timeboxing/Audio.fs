module Timeboxing.Audio

open System
open System.IO
open FSharp.Control.Reactive
open NAudio.Wave
open NLayer.NAudioSupport

let init (state : State) =
  
  let stream = Assets.streamFrom Assets.notification
  let builder = Mp3FileReaderBase.FrameDecompressorBuilder(fun wf -> upcast new Mp3FrameDecompressor(wf))
  let reader = new Mp3FileReaderBase(stream, builder)
  let device =
    let device = new WasapiOut()
    device.Init(reader)
    device
  
  let play () =
    device.Stop ()
    reader.Seek(0L, SeekOrigin.Begin) |> ignore
    device.Play ()
    ()
    
  let subscription =
    state.Time
    |> Observable.pairwise
    |> Observable.filter (fun (last, now) -> last = TimeSpan.FromSeconds(1.) && now = TimeSpan.Zero) 
    |> Observable.subscribe (fun _ -> play ())
    
  let disposeAll () =
    device.Dispose ()
    reader.Dispose ()
    stream.Dispose ()
    subscription.Dispose ()
    ()
  
  { new IDisposable with member this.Dispose() = disposeAll () }
