using UnityEngine;

public class SoundTech : CrewMember
{
    public SoundTech(string crewName) : base(crewName) { }

    public override void PerformTask()
    {
        Debug.Log($"[Crew] {CrewName} (Sound Tech) is checking boom mic levels.");
    }
}
