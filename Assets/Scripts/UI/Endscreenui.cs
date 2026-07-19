// EndScreenUI.cs
// Location in Unity project: Assets/Scripts/UI/EndScreenUI.cs
//
// Observer pattern subscriber. Shows a win or lose panel when
// ProductionManager raises OnDayWrapped. Kept hidden until then.

using UnityEngine;
using UnityEngine.UI;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject endScreenPanel; // parent panel, hidden by default
    [SerializeField] private Text endScreenText;

    // Note: EndScreenPanel should be unchecked (inactive) in the Unity
    // editor by default. We deliberately do NOT call SetActive(false) here
    // in Awake(), because Awake only runs the first time a GameObject
    // becomes active — which would be exactly when HandleDayWrapped()
    // calls SetActive(true), immediately undoing it in the same frame.

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
