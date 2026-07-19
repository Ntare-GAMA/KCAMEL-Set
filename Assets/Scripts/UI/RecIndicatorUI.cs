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
