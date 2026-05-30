using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrosshairController : MonoBehaviour
{
    [Header("Configuración Default")]
    [SerializeField] private Vector2 sizeDefault = new Vector2(4, 4);
    [SerializeField] private Color colorDefault = Color.white;

    [Header("Configuración Highlight")]
    [SerializeField] private Vector2 sizeHighlight = new Vector2(12, 12);
    [SerializeField] private Color colorHighlight = new Color(1f, 0.85f, 0.3f, 1f);

    [Header("Animación")]
    [SerializeField] private float duracionTransicion = 0.15f;

    private RectTransform rect;
    private Image image;
    private bool isHighlighted = false;
    private Coroutine animacionActual;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        rect.sizeDelta = sizeDefault;
        image.color = colorDefault;
    }

    public void SetHighlight(bool highlight)
    {
        if (highlight == isHighlighted) return;
        isHighlighted = highlight;

        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarTransicion(
            highlight ? sizeHighlight : sizeDefault,
            highlight ? colorHighlight : colorDefault
        ));
    }

    private IEnumerator AnimarTransicion(Vector2 targetSize, Color targetColor)
    {
        Vector2 startSize = rect.sizeDelta;
        Color startColor = image.color;
        float elapsed = 0f;

        while (elapsed < duracionTransicion)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duracionTransicion;
            float tEased = 1f - Mathf.Pow(1f - t, 3f);

            rect.sizeDelta = Vector2.Lerp(startSize, targetSize, tEased);
            image.color = Color.Lerp(startColor, targetColor, tEased);

            yield return null;
        }

        rect.sizeDelta = targetSize;
        image.color = targetColor;
    }
}
