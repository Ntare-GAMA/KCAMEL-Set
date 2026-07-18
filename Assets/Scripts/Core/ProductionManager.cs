// ProductionManager.cs
// Location in Unity project: Assets/Scripts/Core/ProductionManager.cs
//
// MonoBehaviour — the central orchestrator. Owns the ProductionState
// machine, holds the SceneEventPublisher, and calls into the algorithms
// and strategies. Guards all state transitions so invalid ones
// (e.g. wrapping before any scene is attempted) can't happen silently.
//
// DESIGN CHANGE: scenes now allow only ONE shot attempt each (see
// FilmScene.TryCompleteShot). Scene quality targets are set up so that
// weaker/cheaper strategies genuinely aren't enough for harder scenes —
// meaning the player must plan which strategy goes where, within a light
// budget too tight to use the most expensive strategy on every scene.

using System.Collections.Generic;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    [SerializeField] private DPController dpController;
    [SerializeField] private LightBudgetManager lightManager;
    [SerializeField] private DirectorHUD directorHUD;
    [SerializeField] private LightMeterUI lightMeterUI;
    [SerializeField] private CrewStatusUI crewStatusUI;
    [SerializeField] private EndScreenUI endScreenUI;
    [SerializeField] private ProgressUI progressUI;

    private List<FilmScene> scenes = new List<FilmScene>();
    private FilmScene activeScene;
    private ProductionState currentState = ProductionState.PreProduction;

    private readonly SceneEventPublisher publisher = new SceneEventPublisher();

    private void Start()
    {
        // Wire Observer subscribers once at startup
        directorHUD.Subscribe(publisher);
        lightMeterUI.Subscribe(lightManager);
        crewStatusUI.Subscribe(publisher);
        endScreenUI.Subscribe(publisher);
        progressUI.Subscribe(publisher);

        lightManager.OnLightDepleted += HandleLightDepleted;

        SetupScenes();
        publisher.RaiseProgressUpdated(0, scenes.Count); // show "0/3" immediately at start
        TransitionTo(ProductionState.Shooting);
    }

    private void SetupScenes()
    {
        // Quality targets are deliberately tiered against the 3 strategies'
        // base quality output (Wide=6, CloseUp=8.5, Tracking=10):
        //   Easy   (target 5)   -> Wide is already enough, no need to spend more
        //   Medium (target 7)   -> Wide (6) FAILS here, needs at least CloseUp
        //   Hard   (target 9.5) -> Wide and CloseUp both FAIL, only Tracking works
        scenes.Add(new FilmScene("Studio Interview (Easy)", new Vector2(0f, -5f), 5f));
        scenes.Add(new FilmScene("Rooftop Dialogue (Medium)", new Vector2(3f, 4f), 7f));
        scenes.Add(new FilmScene("Street Chase (Hard)", new Vector2(-6f, 2f), 9.5f));
    }

    /// <summary>
    /// State pattern: the only place transitions happen, with guards
    /// preventing invalid moves (e.g. wrapping before any scene is resolved).
    /// </summary>
    private void TransitionTo(ProductionState newState)
    {
        if (newState == ProductionState.Wrapped && scenes.Exists(s => !s.IsResolved) && lightManager.CurrentLight > 0f)
        {
            Debug.LogWarning("[ProductionManager] Cannot wrap — scenes remain and light is still available.");
            return;
        }

        currentState = newState;
        Debug.Log($"[ProductionManager] State -> {currentState}");

        if (newState == ProductionState.Wrapped)
        {
            // A true "win" requires every scene to have succeeded — not just
            // been attempted. A scene that got resolved as Failed still
            // counts against you, even if the day technically wrapped.
            bool allScenesSucceeded = scenes.TrueForAll(s => s.IsComplete);
            publisher.RaiseDayWrapped(allScenesSucceeded);
        }
    }

    /// <summary>
    /// Called when the player picks a shot strategy for the current
    /// nearest unresolved scene.
    /// </summary>
    public void ExecuteShot(IShotStrategy strategy)
    {
        if (currentState != ProductionState.Shooting)
        {
            Debug.LogWarning("[ProductionManager] Not currently shooting — ignoring shot request.");
            return;
        }

        Vector2 dpPosition = dpController.GetCurrentPosition();
        activeScene = NearestSceneFinder.FindNearestIncomplete(dpPosition, scenes);

        if (activeScene == null)
        {
            Debug.Log("[ProductionManager] All scenes resolved.");
            TransitionTo(ProductionState.Wrapped);
            return;
        }

        // Resource-budget guard: can't attempt a shot you can't afford.
        // This is what forces genuine planning instead of always picking
        // the "best" strategy — Tracking might simply not be affordable
        // by the time you reach the last scene if it was overspent earlier.
        if (strategy.LightCost > lightManager.CurrentLight)
        {
            Debug.LogWarning($"[ProductionManager] Not enough light for {strategy.ShotName} " +
                              $"({strategy.LightCost} needed, {lightManager.CurrentLight} remaining). Pick a cheaper shot.");
            return;
        }

        publisher.RaiseSceneStarted(activeScene);

        float quality = strategy.Execute();
        lightManager.ConsumeLight(strategy.LightCost);
        activeScene.TryCompleteShot(quality); // one attempt only — success or fail, no retries

        publisher.RaiseShotExecuted(strategy.ShotName);

        if (activeScene.IsComplete)
        {
            publisher.RaiseSceneCompleted(activeScene);
        }

        int resolvedCount = scenes.FindAll(s => s.IsResolved).Count;
        publisher.RaiseProgressUpdated(resolvedCount, scenes.Count);

        if (scenes.TrueForAll(s => s.IsResolved))
        {
            TransitionTo(ProductionState.Wrapped);
        }
    }

    private void HandleLightDepleted()
    {
        Debug.Log("[ProductionManager] Light depleted — forcing wrap.");
        currentState = ProductionState.Wrapped; // bypass guard: day is over regardless of remaining scenes

        bool allScenesSucceeded = scenes.TrueForAll(s => s.IsComplete);
        publisher.RaiseDayWrapped(allScenesSucceeded);
    }

    private void OnDestroy()
    {
        directorHUD.Unsubscribe(publisher);
        lightMeterUI.Unsubscribe(lightManager);
        crewStatusUI.Unsubscribe(publisher);
        endScreenUI.Unsubscribe(publisher);
        progressUI.Unsubscribe(publisher);
        lightManager.OnLightDepleted -= HandleLightDepleted;
    }
}
