using UnityEngine;

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
