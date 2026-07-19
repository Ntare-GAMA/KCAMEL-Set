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
