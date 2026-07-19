using UnityEngine;
using UnityEngine.UI;

public class LightMeterUI : MonoBehaviour
{
    [SerializeField] private RectTransform fillBarRect; // the bar's own RectTransform
    [SerializeField] private float maxLight = 32f;
    [SerializeField] private float fullBarWidth = 260f; // width of the bar at 100% light

    public void Subscribe(LightBudgetManager lightManager)
    {
        lightManager.OnLightChanged += HandleLightChanged;
        lightManager.OnLightDepleted += HandleLightDepleted;

        // Initialize bar to current value immediately
        HandleLightChanged(lightManager.CurrentLight);
    }

    public void Unsubscribe(LightBudgetManager lightManager)
    {
        lightManager.OnLightChanged -= HandleLightChanged;
        lightManager.OnLightDepleted -= HandleLightDepleted;
    }

    private void HandleLightChanged(float currentLight)
    {
        if (fillBarRect == null) return;

        float percent = Mathf.Clamp01(currentLight / maxLight);
        float newWidth = fullBarWidth * percent;

        Vector2 size = fillBarRect.sizeDelta;
        size.x = newWidth;
        fillBarRect.sizeDelta = size;
    }

    private void HandleLightDepleted()
    {
        Debug.Log("[LightMeterUI] Light depleted — day should end.");
    }
}