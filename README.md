# KCAMEL Set

A Unity 2D game built to demonstrate core OOP principles, three design patterns (Strategy, Observer, State), and two algorithms (linear nearest-neighbor search, bounded resource tracking).

You play a Director of Photography (DP) on a one-day shoot. Move between three film scenes, pick a shot strategy for each (Wide / Close-Up / Tracking), and manage a shared, depleting light budget — every scene gets exactly one shot attempt, so the strategy you spend on an easy scene is light you can never get back for a harder one later.

## Setup Instructions

**Requirements**
- Unity Hub
- Unity Editor **2022.3 LTS** or later (2D URP template project)
- Git

**Clone and open**
1. Clone the repository:
   ```
   git clone <this-repo-url>
   ```
2. Open **Unity Hub** → **Add** → select the cloned project folder.
3. Open the project — Unity will import assets and packages automatically on first load (this can take a few minutes).
4. In the **Project** window, go to `Assets/Scenes/` and open the main scene (double-click it).
5. Press **Play** in the Editor toolbar to run the game.

**Controls**
- Move the DP: **arrow keys** or **WASD**
- Attempt a shot: walk near a scene, then click one of the three on-screen shot buttons (**Wide**, **Close-Up**, **Tracking**)

**Build**
- `File → Build Settings` → select your target platform → `Build` (or `Build and Run`). The main scene must be added under **Scenes In Build** if it isn't already.

**Troubleshooting**
- If scripts show compile errors on first open, wait for Unity to finish importing (bottom-right progress spinner) before checking the Console again.
- If UI elements appear blank, confirm `TextMeshPro` essentials have been imported (Unity will prompt for this automatically the first time a TMP object is selected).

## How to play

1. Move the DP toward one of the three scene markers.
2. Click a shot-selection button to attempt that scene: **Wide** (cheap, safe, lower quality), **Close-Up** (moderate cost/quality, small failure risk), or **Tracking** (expensive, highest quality, highest failure risk).
3. Watch the light meter in the HUD — it depletes by the light cost of whichever shot you choose and never refills.
4. Each scene only gets one attempt: meeting its quality target completes it, falling short fails it permanently — no retries.
5. The day wraps once all three scenes are resolved, or the moment the light budget hits zero, whichever comes first. The end screen reports a win (all scenes succeeded) or a loss.

## Project structure

```
Assets/Scripts/
├── Algorithms/
│   ├── NearestSceneFinder.cs     Algorithm 1 — nearest incomplete scene (linear search)
│   └── LightBudgetManager.cs     Algorithm 2 — bounded resource decrement
├── Core/
│   ├── ProductionManager.cs      Central orchestrator; owns the State machine
│   └── ProductionState.cs        State pattern — PreProduction / Shooting / Wrapped
├── Crew/
│   ├── CrewMember.cs             Abstract base (Abstraction, Inheritance)
│   ├── CameraOperator.cs
│   ├── Gaffer.cs
│   └── SoundTech.cs
├── Events/
│   └── SceneEventPublisher.cs    Observer pattern subject
├── Player/
│   └── DPController.cs           DP movement + encapsulated position access
├── Scenes_Data/
│   └── FilmScene.cs              Scene data with encapsulated resolution state
├── Strategies/
│   ├── IShotStrategy.cs          Strategy pattern interface
│   ├── WideShotStrategy.cs
│   ├── CloseUpStrategy.cs
│   └── TrackingShotStrategy.cs
└── UI/
    ├── DirectorHUD.cs            Observer subscriber
    ├── LightMeterUI.cs           Observer subscriber (subscribes to LightBudgetManager)
    ├── CrewStatusUI.cs           Observer subscriber
    ├── ProgressUI.cs             Observer subscriber
    ├── EndScreenUI.cs            Observer subscriber
    ├── ShotSelectButton.cs       Builds a concrete IShotStrategy on click
    ├── RecIndicatorUI.cs         Cosmetic — blinking REC dot
    └── TimecodeUI.cs             Cosmetic — running timecode display
```

## OOP principles

| Principle | Where it shows up |
|---|---|
| Encapsulation | `FilmScene` only changes `IsComplete`/`IsFailed`/`AccumulatedQuality` through `TryCompleteShot()`; `DPController` exposes `GetCurrentPosition()` instead of its `Rigidbody2D`; `LightBudgetManager.CurrentLight` only changes through `ConsumeLight()` |
| Abstraction | `IShotStrategy` defines *what* a shot provides without saying *how*; `CrewMember` declares `PerformTask()` as abstract |
| Inheritance | `CameraOperator`, `Gaffer`, `SoundTech` all inherit from `CrewMember` |
| Polymorphism | `ProductionManager.ExecuteShot(IShotStrategy strategy)` calls `strategy.Execute()` identically regardless of which concrete strategy was passed in |

Full writeup with code excerpts and reflections: `Part1-2_OOP_and_Design_Patterns.docx`.

## Design patterns

- **Strategy** — `IShotStrategy` + `WideShotStrategy` / `CloseUpStrategy` / `TrackingShotStrategy`. Each shot type is a fully self-contained, interchangeable object; `ProductionManager` depends only on the interface.
- **Observer** — `SceneEventPublisher` (events: `OnSceneStarted`, `OnSceneCompleted`, `OnShotExecuted`, `OnDayWrapped`, `OnProgressUpdated`) decouples `ProductionManager` from the five UI panels that react to it. `LightBudgetManager` independently publishes `OnLightChanged` / `OnLightDepleted`, observed by `LightMeterUI`.
- **State** — `ProductionState` enum (`PreProduction`, `Shooting`, `Wrapped`), with every transition funneled through `ProductionManager.TransitionTo()`, which guards against invalid transitions such as wrapping while scenes and light both remain.

Full writeup: `Part1-2_OOP_and_Design_Patterns.docx`.

## Algorithms

| Algorithm | File | Complexity |
|---|---|---|
| Nearest incomplete scene (linear search / greedy nearest-neighbor) | `NearestSceneFinder.cs` | O(n) time, O(1) space |
| Light budget tracking (bounded resource decrement) | `LightBudgetManager.cs` | O(1) per call |
