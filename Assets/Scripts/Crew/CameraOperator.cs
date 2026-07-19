using UnityEngine;

public class CameraOperator : CrewMember
{
    public CameraOperator(string crewName) : base(crewName) { }

    public override void PerformTask()
    {
        Debug.Log($"[Crew] {CrewName} (Camera Operator) is framing the shot.");
    }
}
