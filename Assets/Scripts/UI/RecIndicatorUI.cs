// RecIndicatorUI.cs
// Location in Unity project: Assets/Scripts/UI/RecIndicatorUI.cs
//
// Purely cosmetic — makes a red dot blink like a camera's REC indicator,
// reinforcing the "you're looking through a viewfinder" framing device.
// Not tied to any game state; runs continuously while shooting.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RecIndicatorUI : MonoBehaviour
{
    [SerializeField] private Image recDot;
    [SerializeField] private float blinkInterval = 0.6f;

    private void OnEnable()
    {
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            if (recDot != null)
                recDot.enabled = !recDot.enabled;

            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
