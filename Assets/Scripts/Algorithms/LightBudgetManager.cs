using System;
using UnityEngine;

public class LightBudgetManager : MonoBehaviour
{
    // Tuned against 3 scenes with tiered targets (Easy/Medium/Hard) and the
    // 3 shot strategies' costs (Wide=5, CloseUp=8, Tracking=14). The
    // "correct" minimal-cost path (Wide+CloseUp+Tracking) costs exactly 27 —
    // startingLight leaves only a small buffer, so using an oversized
    // strategy on an easy scene can genuinely cost you a harder scene later.
    [SerializeField] private float startingLight = 32f;
    [SerializeField] private float warningThreshold = 8f;

    public float CurrentLight { get; private set; }
    public bool IsLightCritical => CurrentLight <= warningThreshold;

    // Observer-style events other systems (HUD, ProductionManager) can subscribe to
    public event Action<float> OnLightChanged;
    public event Action OnLightDepleted;

    private void Awake()
    {
        CurrentLight = startingLight;
    }

    /// <summary>
    /// Consumes light based on the cost of the executed shot strategy.
    /// Called by ProductionManager right after IShotStrategy.Execute().
    /// </summary>
    public void ConsumeLight(float amount)
    {
        CurrentLight = Mathf.Max(0f, CurrentLight - amount);
        OnLightChanged?.Invoke(CurrentLight);

        if (CurrentLight <= 0f)
        {
            OnLightDepleted?.Invoke();
        }
        else if (IsLightCritical)
        {
            Debug.LogWarning("[LightBudgetManager] Light critical — golden hour running out.");
        }
    }
}
