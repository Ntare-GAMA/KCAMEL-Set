// LightMeterUI.cs
// Location in Unity project: Assets/Scripts/UI/LightMeterUI.cs
//
// Observer pattern subscriber to LightBudgetManager's events.

using UnityEngine;
using UnityEngine.UI;

public class LightMeterUI : MonoBehaviour
{
    [SerializeField] private Image fillBar; // set Image.type = Filled in Inspector
    [SerializeField] private float maxLight = 100f;

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
        if (fillBar != null)
            fillBar.fillAmount = currentLight / maxLight;
    }

    private void HandleLightDepleted()
    {
        Debug.Log("[LightMeterUI] Light depleted — day should end.");
    }
}
