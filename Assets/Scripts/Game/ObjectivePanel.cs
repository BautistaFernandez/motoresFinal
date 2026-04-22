using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectivePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float slideDistance = 20f;

    private RectTransform rect;
    private Vector2 finalPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        finalPosition = rect.anchoredPosition;
        canvasGroup.alpha = 0f;
    }

    public void Show(string text)
    {
        objectiveText.text = text;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Vector2 startPos = finalPosition + new Vector2(0, -slideDistance);

        canvasGroup.alpha = 0f;
        rect.anchoredPosition = startPos;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = t;
            rect.anchoredPosition = Vector2.Lerp(startPos, finalPosition, t);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        rect.anchoredPosition = finalPosition;
    }
}
