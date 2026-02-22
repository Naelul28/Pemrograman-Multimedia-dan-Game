using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTransitionSprite : MonoBehaviour
{
    public List<SpriteRenderer> layers;     // bg-1, bg-2, bg-3 ...
    public float fadeDuration = 0.6f;       // waktu fade per layer
    public float delayBetween = 0.08f;      // jeda antar layer

    // ============================
    // FADE OUT (dari depan → belakang)
    // ============================

    public void FadeOutAll()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutSequence());
    }

    IEnumerator FadeOutSequence()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            StartCoroutine(FadeSprite(layers[i], 0f));
            yield return new WaitForSeconds(delayBetween);
        }
    }

    // ============================
    // FADE IN (kebalikan urutan)
    // ============================

    public void FadeInAll()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInSequence());
    }

    IEnumerator FadeInSequence()
    {
        for (int i = layers.Count - 1; i >= 0; i--)
        {
            StartCoroutine(FadeSprite(layers[i], 1f));
            yield return new WaitForSeconds(delayBetween);
        }
    }

    // ============================
    // FUNGSI FADE (umum)
    // ============================

    IEnumerator FadeSprite(SpriteRenderer sr, float targetAlpha)
    {
        Color c = sr.color;
        float startAlpha = c.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            sr.color = c;

            yield return null;
        }

        c.a = targetAlpha;
        sr.color = c;
    }
}
