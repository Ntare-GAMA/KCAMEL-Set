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
