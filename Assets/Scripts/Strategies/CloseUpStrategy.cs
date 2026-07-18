// CloseUpStrategy.cs
// Location in Unity project: Assets/Scripts/Strategies/CloseUpStrategy.cs

using UnityEngine;

/// <summary>
/// Slower to frame precisely, higher quality yield than a wide shot.
/// Small risk of a fumbled take — mid-tier risk/reward.
/// </summary>
public class CloseUpStrategy : IShotStrategy
{
    public string ShotName => "Close-Up";
    public float LightCost => 8f;
    public float TimeCost => 6f;
    public float FailureChance => 0.10f; // 10% chance of a botched take

    public float Execute()
    {
        float baseQuality = 8.5f;
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
