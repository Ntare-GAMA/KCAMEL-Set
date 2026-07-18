// LightBudgetManager.cs
// Location in Unity project: Assets/Scripts/Algorithms/LightBudgetManager.cs
//
// ALGORITHM #2: Resource optimization (light budget management)
//
// Problem it solves: the shoot day has finite light. Every shot strategy
// consumes some of that light. The player must budget which strategies
// to use across remaining scenes before light runs out ("losing golden hour").
//
// Approach: a decaying resource pool with a warning threshold, decremented
// by whatever LightCost the chosen IShotStrategy reports. This is a simple
// greedy resource-tracking model rather than a full optimizer — the
// "optimization" is left to the player's decisions (Strategy pattern choice),
// while this class just tracks and reports state honestly.
//
// Complexity: O(1) per update — a single subtraction and threshold check.
// Factors affecting "performance" here aren't computational but strategic:
// how many high-cost shots (Tracking) are chosen early drains the budget
// faster, forcing cheaper choices (Wide) later in the day.

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
