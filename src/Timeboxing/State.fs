namespace Timeboxing

open System
open System.Reactive.Subjects
open FSharp.Control.Reactive

type DurationType =
| Work
| Break

type State = {
  IsExitRequested : BehaviorSubject<bool>
  WorkDuration : BehaviorSubject<TimeSpan>
  BreakDuration : BehaviorSubject<TimeSpan>
  Time: BehaviorSubject<TimeSpan>
  FormattedTime: IObservable<string>
  IsTimerRunning: BehaviorSubject<bool>
  DurationType : BehaviorSubject<DurationType>
  CompletedTimeboxes : BehaviorSubject<int>
  Title : BehaviorSubject<string>
}

module State =

  let formatTime (timeSpan : TimeSpan) = timeSpan.ToString("mm\:ss")
    
  let init () =
    let isExitRequested = new BehaviorSubject<bool>(false)
    let workDuration = new BehaviorSubject<TimeSpan>(TimeSpan.FromMinutes 25.)
    let breakDuration = new BehaviorSubject<TimeSpan>(TimeSpan.FromMinutes 5.)

    let time = new BehaviorSubject<TimeSpan>(workDuration.Value)
    let isTimerRunning = new BehaviorSubject<bool>(false)
    let durationType = new BehaviorSubject<DurationType>(Work)

    let completedTimeboxes = new BehaviorSubject<int>(0)
    
    let oneSec = TimeSpan.FromSeconds 1.
    
    Observable.interval oneSec
    |> Observable.observeOn Avalonia.Threading.AvaloniaScheduler.Instance
    |> Observable.combineLatest isTimerRunning
    |> Observable.map fst
    |> Observable.filter id
    |> Observable.withLatestFrom (fun a -> id) time
    |> Observable.subscribe (fun t ->
      if t >= oneSec then
        let newTime = t.Subtract oneSec
        time.OnNext(newTime)
        if newTime = TimeSpan.Zero && durationType.Value = Work then
          completedTimeboxes.OnNext(completedTimeboxes.Value + 1)
      () )
    |> ignore
    
    let formattedTime =
      time
      |> Observable.map formatTime
      
    let getTitle version =
      $"Timeboxing {version}"
      
    let title = new BehaviorSubject<string>(getTitle Version.version)
    
    {
      IsExitRequested = isExitRequested
      WorkDuration = workDuration
      BreakDuration = breakDuration
      Time = time
      FormattedTime = formattedTime
      IsTimerRunning = isTimerRunning
      DurationType = durationType
      CompletedTimeboxes = completedTimeboxes
      Title = title
    }


