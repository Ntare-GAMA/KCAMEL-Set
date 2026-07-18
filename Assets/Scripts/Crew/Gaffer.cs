// Gaffer.cs
// Location in Unity project: Assets/Scripts/Crew/Gaffer.cs

using UnityEngine;

public class Gaffer : CrewMember
{
    public Gaffer(string crewName) : base(crewName) { }

    public override void PerformTask()
    {
        Debug.Log($"[Crew] {CrewName} (Gaffer) is adjusting the lighting rig.");
    }
}
