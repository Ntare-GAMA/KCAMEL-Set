// IShotStrategy.cs
// Location in Unity project: Assets/Scripts/Strategies/IShotStrategy.cs

/// <summary>
/// Strategy pattern: defines a swappable shot-execution behavior.
/// Each concrete strategy trades off time/light cost against quality yield,
/// giving the player a meaningful choice per scene rather than a fixed outcome.
/// </summary>
public interface IShotStrategy
{
    // Human-readable name shown in UI (e.g. shot selection buttons)
    string ShotName { get; }

    // How much light budget this shot consumes when executed
    float LightCost { get; }

    // How much in-game time this shot takes to set up and shoot
    float TimeCost { get; }

    // Chance (0.0–1.0) that this shot gets botched — a failed take
    // returns reduced quality instead of the full amount. Riskier,
    // more ambitious shots (Tracking) carry higher failure chance,
    // so "always pick the biggest number" stops being a safe default.
    float FailureChance { get; }

    // Executes the shot and returns the quality score it contributes
    // to the FilmScene's target (see FilmScene.cs)
    float Execute();
}
