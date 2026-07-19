using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrewStatusUI : MonoBehaviour
{
    [SerializeField] private Text crewStatusText;

    [Header("Crew Icons (assign in Inspector)")]
    [SerializeField] private Image cameraIcon;
    [SerializeField] private Image gafferIcon;
    [SerializeField] private Image soundIcon;

    [Header("Reaction Settings")]
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float highlightDuration = 0.4f;

    private Color cameraDefaultColor;
    private Color gafferDefaultColor;
    private Color soundDefaultColor;

    private void Awake()
    {
        // Cache each icon's original color so we can reliably reset after flashing
        if (cameraIcon != null) cameraDefaultColor = cameraIcon.color;
        if (gafferIcon != null) gafferDefaultColor = gafferIcon.color;
        if (soundIcon != null) soundDefaultColor = soundIcon.color;
    }

    public void Subscribe(SceneEventPublisher publisher)
    {
        publisher.OnSceneStarted += HandleSceneStarted;
        publisher.OnShotExecuted += HandleShotExecuted;
    }

    public void Unsubscribe(SceneEventPublisher publisher)
    {
        publisher.OnSceneStarted -= HandleSceneStarted;
        publisher.OnShotExecuted -= HandleShotExecuted;
    }

    private void HandleSceneStarted(FilmScene scene)
    {
        if (crewStatusText != null)
            crewStatusText.text = $"Crew ready for: {scene.SceneName}";

        // All 3 crew roles react together when a new scene begins
        FlashIcon(cameraIcon, cameraDefaultColor);
        FlashIcon(gafferIcon, gafferDefaultColor);
        FlashIcon(soundIcon, soundDefaultColor);
    }

    private void HandleShotExecuted(string shotName)
    {
        if (crewStatusText != null)
            crewStatusText.text = $"Shot taken: {shotName}";

        // Camera icon reacts specifically to a shot being executed
        FlashIcon(cameraIcon, cameraDefaultColor);
    }

    private void FlashIcon(Image icon, Color defaultColor)
    {
        if (icon == null) return;
        StartCoroutine(FlashRoutine(icon, defaultColor));
    }

    private IEnumerator FlashRoutine(Image icon, Color defaultColor)
    {
        icon.color = highlightColor;
        yield return new WaitForSeconds(highlightDuration);
        icon.color = defaultColor;
    }
}
