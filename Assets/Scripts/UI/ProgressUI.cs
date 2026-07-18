// ProgressUI.cs
// Location in Unity project: Assets/Scripts/UI/ProgressUI.cs
//
// Observer pattern subscriber. Shows "X of Y scenes done" so the player
// has visible stakes/urgency, instead of progress only existing in the
// Console log.

using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Text progressText;

    public void Subscribe(SceneEventPublisher publisher)
    {
        publisher.OnProgressUpdated += HandleProgressUpdated;
    }

    public void Unsubscribe(SceneEventPublisher publisher)
    {
        publisher.OnProgressUpdated -= HandleProgressUpdated;
    }

    private void HandleProgressUpdated(int resolved, int total)
    {
        if (progressText != null)
            progressText.text = $"Scenes: {resolved}/{total}";
    }
}
