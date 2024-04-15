using UnityEngine;
using System.Collections;

public class SpriteFader : MonoBehaviour
{
    public float fadeDuration = 1.0f;  // Duration of the fade effect
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeSpriteTo(1));  // Fade to fully opaque
    }

    public void FadeOut()
    {
        StartCoroutine(FadeSpriteTo(0));  // Fade to fully transparent
    }

    IEnumerator FadeSpriteTo(float targetAlpha)
    {
        float currentAlpha = spriteRenderer.color.a;
        float elapsed = 0.0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsed / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
    }
}
