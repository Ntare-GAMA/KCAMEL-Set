// ShotSelectButton.cs
// Location in Unity project: Assets/Scripts/UI/ShotSelectButton.cs
//
// Attach this to each of your 3 shot-selection buttons (Wide/Close-up/Tracking).
// Set the "shotType" dropdown in the Inspector to match which button this is.
// On click, it builds the matching IShotStrategy and calls ProductionManager.ExecuteShot().

using UnityEngine;
using UnityEngine.UI;

public class ShotSelectButton : MonoBehaviour
{
    public enum ShotType { Wide, CloseUp, Tracking }

    [SerializeField] private ShotType shotType;
    [SerializeField] private ProductionManager productionManager;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        IShotStrategy strategy = shotType switch
        {
            ShotType.Wide => new WideShotStrategy(),
            ShotType.CloseUp => new CloseUpStrategy(),
            ShotType.Tracking => new TrackingShotStrategy(),
            _ => new WideShotStrategy()
        };

        productionManager.ExecuteShot(strategy);
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(HandleClick);
    }
}
