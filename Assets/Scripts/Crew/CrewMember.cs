// CrewMember.cs
// Location in Unity project: Assets/Scripts/Crew/CrewMember.cs

using UnityEngine;

/// <summary>
/// Abstract base for all crew roles. Demonstrates Abstraction (PerformTask
/// is declared here but has no single correct implementation) and sets up
/// Inheritance/Polymorphism for CameraOperator, Gaffer, SoundTech.
/// </summary>
public abstract class CrewMember
{
    public string CrewName { get; private set; }

    protected CrewMember(string crewName)
    {
        CrewName = crewName;
    }

    /// <summary>
    /// Each concrete crew role performs its task differently —
    /// called polymorphically through a CrewMember reference.
    /// </summary>
    public abstract void PerformTask();
}
