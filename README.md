# KCAMEL Set - Production Day

A top-down 2D Unity game where you play a Director of Photography (DP) managing a single film shoot day: move between scene locations, choose a shot strategy for each one, and manage a finite lighting budget before the day wraps.

Built for the ALU Advanced C# / Unity Game Development course — Mini Game: Principles of OOP and Design Patterns assignment.

---

## Table of Contents
- [Gameplay Overview](#gameplay-overview)
- [Controls](#controls)
- [Setup Instructions](#setup-instructions)
- [Project Structure](#project-structure)
- [Design Patterns](#design-patterns)
- [Algorithms](#algorithms)
- [OOP Principles](#oop-principles)
- [Known Limitations](#known-limitations)
- [Credits](#credits)

---

## Gameplay Overview

You control a DP on a film set with three scenes to shoot: an **Easy**, a **Medium**, and a **Hard** scene, each requiring a different quality target. You have three shot strategies available — **Wide Shot**, **Close-Up**, and **Tracking Shot** — each with a different light cost, time cost, and chance of a botched take.

Each scene only gets **one shot attempt**. If your chosen strategy doesn't meet that scene's quality target, the scene is marked **failed permanently** — there are no retries. You also have a **shared, limited light budget** across the whole day, so spending an expensive strategy on an easy scene can cost you the light needed to complete a harder scene later.

The day ends (wraps) once all three scenes are resolved, or once your light budget runs out — whichever comes first. An end screen reports whether you successfully wrapped all three scenes or ran out of light with scenes still unresolved.

## Controls

| Action | Input |
|---|---|
| Move DP | Arrow keys / WASD |
| Select shot strategy | Click **Wide Shot**, **Close-Up**, or **Tracking Shot** button |

## Setup Instructions

1. Clone or download this repository.
2. Open the project in **Unity 6 (6000.4.5f1)** or later via Unity Hub.
3. Open the `SampleScene` scene under `Assets/Scenes/`.
4. Press **Play** in the Unity Editor.
5. To build a standalone version: `File > Build Settings > Build`.

No external packages or paid assets are required beyond what's included in the repository.

## Project Structure

```
Assets/Scripts/
├── Core/               ProductionManager (orchestrator), ProductionState (enum)
├── Strategies/          IShotStrategy interface + 3 concrete strategies
├── Crew/                CrewMember abstract class + 3 concrete crew roles
├── Scenes_Data/         FilmScene (scene data + resolution logic)
├── Events/              SceneEventPublisher (Observer pattern subject)
├── Algorithms/          NearestSceneFinder, LightBudgetManager
├── Player/              DPController (top-down movement)
└── UI/                  DirectorHUD, LightMeterUI, CrewStatusUI, EndScreenUI,
                         ProgressUI, RecIndicatorUI, TimecodeUI, ShotSelectButton
```

## Design Patterns

### Strategy — `IShotStrategy`
`WideShotStrategy`, `CloseUpStrategy`, and `TrackingShotStrategy` each implement `IShotStrategy`, letting `ProductionManager` execute any shot type polymorphically through a single interface call (`strategy.Execute()`), without knowing which concrete strategy it's holding.

### Observer — `SceneEventPublisher`
Five UI scripts (`DirectorHUD`, `LightMeterUI`, `CrewStatusUI`, `EndScreenUI`, `ProgressUI`) each subscribe independently to events raised by `SceneEventPublisher`, decoupling game logic from the UI layer entirely. `ProductionManager` never references any UI class directly.

### State — `ProductionState`
An enum (`PreProduction`, `Shooting`, `Wrapped`) with all transitions guarded inside `ProductionManager.TransitionTo()`, preventing invalid states such as wrapping the day while scenes remain and light is still available.

## Algorithms

### 1. Nearest-Scene Search — `NearestSceneFinder`
A linear O(n) scan that finds the closest unresolved scene to the DP's current position whenever a shot is executed, using squared-distance comparisons to avoid unnecessary square-root calls.

### 2. Light Budget (Resource Optimization) — `LightBudgetManager`
Tracks a finite, shared light resource that depletes based on the chosen strategy's cost, blocks unaffordable shots outright, and fires events (`OnLightChanged`, `OnLightDepleted`) that the UI and `ProductionManager` both react to independently.

## OOP Principles

| Principle | Where it's demonstrated |
|---|---|
| Encapsulation | `FilmScene`'s completion state can only change through `TryCompleteShot()` |
| Abstraction | `IShotStrategy` exposes only what a shot needs to report, hiding internal execution logic |
| Inheritance | `CrewMember` (abstract) → `CameraOperator`, `Gaffer`, `SoundTech` |
| Polymorphism | `PerformTask()` and `Execute()` calls resolve to different behavior depending on the concrete runtime type |

## Known Limitations

- Shot strategy failure is randomized (`UnityEngine.Random`), so occasional unlucky runs on the Hard scene are expected by design — this is intentional risk, not a bug.
- Crew role icons are decorative HUD elements; crew members do not have independent in-world behavior beyond their `PerformTask()` method, which is not currently called from gameplay (reserved for the OOP/inheritance demonstration).
- The camera-viewfinder HUD styling (corner brackets, REC indicator, timecode) is purely cosmetic framing and does not affect gameplay logic.

## Credits

Developed by NTARE GAMA Allan ( KCAMEL Productions) for ALU's Advanced C# / Unity Game Development course.
