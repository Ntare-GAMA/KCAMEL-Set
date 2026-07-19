using UnityEngine;
using UnityEngine.UI;

public class DirectorHUD : MonoBehaviour
{
    [SerializeField] private Text statusText; // assign in Inspector

    /// <summary>
    /// Called once by ProductionManager on startup to wire this HUD
    /// to the shared event publisher.
    /// </summary>
    public void Subscribe(SceneEventPublisher publisher)
    {
        publisher.OnSceneStarted += HandleSceneStarted;
        publisher.OnSceneCompleted += HandleSceneCompleted;
        publisher.OnShotExecuted += HandleShotExecuted;
    }

    public void Unsubscribe(SceneEventPublisher publisher)
    {
        publisher.OnSceneStarted -= HandleSceneStarted;
        publisher.OnSceneCompleted -= HandleSceneCompleted;
        publisher.OnShotExecuted -= HandleShotExecuted;
    }

    private void HandleSceneStarted(FilmScene scene)
    {
        if (statusText != null)
            statusText.text = $"Now shooting: {scene.SceneName}";
    }

    private void HandleSceneCompleted(FilmScene scene)
    {
        if (statusText != null)
            statusText.text = $"Wrapped: {scene.SceneName}";
    }

    private void HandleShotExecuted(string shotName)
    {
        if (statusText != null)
            statusText.text = $"Shot taken: {shotName}";
    }
}
