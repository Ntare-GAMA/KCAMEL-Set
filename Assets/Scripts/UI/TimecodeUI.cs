// TimecodeUI.cs
// Location in Unity project: Assets/Scripts/UI/TimecodeUI.cs
//
// Purely cosmetic — a running timecode readout (HH:MM:SS), like a real
// camera viewfinder display. Counts up from the moment the scene loads.

using UnityEngine;
using UnityEngine.UI;

public class TimecodeUI : MonoBehaviour
{
    [SerializeField] private Text timecodeText;

    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (timecodeText != null)
        {
            int hours = Mathf.FloorToInt(elapsedTime / 3600f);
            int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timecodeText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
        }
    }
}
