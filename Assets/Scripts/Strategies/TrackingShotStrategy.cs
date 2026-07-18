// TrackingShotStrategy.cs
// Location in Unity project: Assets/Scripts/Strategies/TrackingShotStrategy.cs

using UnityEngine;

/// <summary>
/// Highest cost in light and time, highest quality yield — but also the
/// riskiest shot to attempt. A botched Tracking shot still costs full
/// light and time, but returns much less quality, which is exactly the
/// "high risk, high reward" tension that makes Strategy choice meaningful.
/// </summary>
public class TrackingShotStrategy : IShotStrategy
{
    public string ShotName => "Tracking Shot";
    public float LightCost => 14f;
    public float TimeCost => 10f;
    public float FailureChance => 0.30f; // 30% chance of a botched take

    public float Execute()
    {
        float baseQuality = 10f;
        Debug.Log($"[Strategy] Executing {ShotName} — cost: {LightCost} light, {TimeCost} time");

        if (Random.value < FailureChance)
        {
            float botchedQuality = baseQuality * 0.5f;
            Debug.LogWarning($"[Strategy] {ShotName} was botched! Quality reduced to {botchedQuality}");
            return botchedQuality;
        }

        return baseQuality;
    }
}
