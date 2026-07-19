using UnityEngine;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject endScreenPanel; // parent panel, hidden by default
    [SerializeField] private Text endScreenText;

    public void Subscribe(SceneEventPublisher publisher)
    {
        publisher.OnDayWrapped += HandleDayWrapped;
    }

    public void Unsubscribe(SceneEventPublisher publisher)
    {
        publisher.OnDayWrapped -= HandleDayWrapped;
    }

    private void HandleDayWrapped(bool allScenesComplete)
    {
        if (endScreenPanel != null)
            endScreenPanel.SetActive(true);

        if (endScreenText != null)
        {
            endScreenText.text = allScenesComplete
                ? "That's a wrap! All scenes shot successfully."
                : "Ran out of light, day cut short.";
        }
    }
}
