// ProductionState.cs
// Location in Unity project: Assets/Scripts/Core/ProductionState.cs

/// <summary>
/// State pattern: the valid stages of a shoot day.
/// Kept as an enum + guarded transitions in ProductionManager rather than
/// scattered bools (isShooting, isWrapped, etc.), which is exactly the
/// kind of implicit-state bug this pattern is meant to prevent.
/// </summary>
public enum ProductionState
{
    PreProduction,
    Shooting,
    Wrapped
}
