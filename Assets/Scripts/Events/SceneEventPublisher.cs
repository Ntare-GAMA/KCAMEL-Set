using System;

/// <summary>
/// Observer pattern subject. ProductionManager raises these events;
/// DirectorHUD, LightMeterUI, and CrewStatusUI subscribe independently.
/// Neither side needs a direct reference to the other's concrete type —
/// this is what keeps the vehicle/production logic decoupled from UI.
/// </summary>
public class SceneEventPublisher
{
    public event Action<FilmScene> OnSceneStarted;
    public event Action<FilmScene> OnSceneCompleted;
    public event Action<string> OnShotExecuted; // passes shot name for HUD feedback

    // bool = true if all scenes were completed (win), false if light ran out first (lose)
    public event Action<bool> OnDayWrapped;

    // Fired every time a scene gets resolved (success or fail), so the UI
    // can show "X of Y scenes done" without polling every frame.
    public event Action<int, int> OnProgressUpdated;

    public void RaiseSceneStarted(FilmScene scene) => OnSceneStarted?.Invoke(scene);
    public void RaiseSceneCompleted(FilmScene scene) => OnSceneCompleted?.Invoke(scene);
    public void RaiseShotExecuted(string shotName) => OnShotExecuted?.Invoke(shotName);
    public void RaiseDayWrapped(bool allScenesComplete) => OnDayWrapped?.Invoke(allScenesComplete);
    public void RaiseProgressUpdated(int resolved, int total) => OnProgressUpdated?.Invoke(resolved, total);
}
