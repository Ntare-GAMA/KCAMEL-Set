// NOTE: Named "FilmScene" instead of "Scene" to avoid clashing with
// UnityEngine.SceneManagement.Scene.

using UnityEngine;

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
