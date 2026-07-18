// FilmScene.cs
// Location in Unity project: Assets/Scripts/Scenes_Data/FilmScene.cs
//
// NOTE: Named "FilmScene" instead of "Scene" to avoid clashing with
// UnityEngine.SceneManagement.Scene.

using UnityEngine;

/// <summary>
/// Plain data class representing a single shoot location/scene on set.
///
/// DESIGN CHANGE: each scene now only gets ONE shot attempt — no retries,
/// no stacking multiple shots to slowly reach the target. Whatever
/// strategy the player commits to either meets the QualityTarget (Complete)
/// or falls short (Failed). This is what turns strategy choice into a real
/// decision: an expensive Tracking shot "wasted" on an easy scene is light
/// you can never get back for a harder scene later.
///
/// Demonstrates encapsulation: IsComplete/IsFailed/AccumulatedQuality can
/// only change through TryCompleteShot(), never set directly from outside.
/// </summary>
public class FilmScene
{
    public string SceneName { get; private set; }
    public Vector2 Location { get; private set; }
    public float QualityTarget { get; private set; }

    public bool IsComplete { get; private set; }
    public bool IsFailed { get; private set; }
    public float AccumulatedQuality { get; private set; }

    // A scene is "resolved" once it's been attempted, successfully or not —
    // resolved scenes are removed from consideration by NearestSceneFinder.
    public bool IsResolved => IsComplete || IsFailed;

    public FilmScene(string sceneName, Vector2 location, float qualityTarget)
    {
        SceneName = sceneName;
        Location = location;
        QualityTarget = qualityTarget;
        IsComplete = false;
        IsFailed = false;
        AccumulatedQuality = 0f;
    }

    /// <summary>
    /// The one and only shot attempt for this scene. Called by
    /// ProductionManager right after a shot strategy executes.
    /// </summary>
    public void TryCompleteShot(float qualityGained)
    {
        if (IsResolved)
        {
            Debug.LogWarning($"[FilmScene] {SceneName} was already resolved — ignoring extra shot.");
            return;
        }

        AccumulatedQuality = qualityGained;

        if (AccumulatedQuality >= QualityTarget)
        {
            IsComplete = true;
            Debug.Log($"[FilmScene] {SceneName} wrapped successfully — {AccumulatedQuality}/{QualityTarget}");
        }
        else
        {
            IsFailed = true;
            Debug.LogWarning($"[FilmScene] {SceneName} FAILED — only reached {AccumulatedQuality}/{QualityTarget}. No retries.");
        }
    }
}
