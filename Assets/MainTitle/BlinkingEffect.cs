using UnityEngine;
using System.Collections;

public class BlinkingEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float blinkDuration = 2f;
    public int blinkCount = 3;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(BlinkSprite());
    }

    IEnumerator BlinkSprite()
    {
        Color originalColor = spriteRenderer.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        for (int i = 0; i < blinkCount; i++)
        {
            // Fade out
            float elapsedTime = 0;
            while (elapsedTime < blinkDuration / 2)
            {
                spriteRenderer.color = Color.Lerp(originalColor, transparentColor, elapsedTime / (blinkDuration / 2));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = transparentColor;

            // Fade in
            elapsedTime = 0;
            while (elapsedTime < blinkDuration / 2)
            {
                spriteRenderer.color = Color.Lerp(transparentColor, originalColor, elapsedTime / (blinkDuration / 2));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = originalColor;
        }
    }
}
