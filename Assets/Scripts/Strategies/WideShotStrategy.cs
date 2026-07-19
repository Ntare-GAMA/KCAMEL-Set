using UnityEngine;

/// <summary>
/// Fast, low-cost, moderate-quality shot. The "safe" choice — zero risk
/// of a botched take, good for scenes where you can't afford to gamble.
/// </summary>
public class WideShotStrategy : IShotStrategy
{
    public string ShotName => "Wide Shot";
    public float LightCost => 5f;
    public float TimeCost => 3f;
    public float FailureChance => 0f; // completely safe

    public float Execute()
    {
        float baseQuality = 6f;
        Debug.Log($"[Strategy] Executing {ShotName} — cost: {LightCost} light, {TimeCost} time");
        return baseQuality;
    }
}
