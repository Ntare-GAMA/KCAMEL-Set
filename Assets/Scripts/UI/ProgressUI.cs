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
