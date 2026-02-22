using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickerStar : MonoBehaviour
{
    public float minAlpha = 0.3f;   // transparansi paling redup
    public float maxAlpha = 1f;     // transparansi paling terang
    public float minSpeed = 1f;     // speed paling lambat
    public float maxSpeed = 3f;     // speed paling cepat

    private float targetAlpha;
    private float speed;
    private Image uiImage;
    private SpriteRenderer spriteRenderer;
    private Color color;

    void Start()
    {
        uiImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        PickNewTarget();
    }

    void Update()
    {
        if (uiImage != null) color = uiImage.color;
        else if (spriteRenderer != null) color = spriteRenderer.color;

        color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * speed);

        if (uiImage != null) uiImage.color = color;
        else if (spriteRenderer != null) spriteRenderer.color = color;

        if (Mathf.Abs(color.a - targetAlpha) < 0.05f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        targetAlpha = Random.Range(minAlpha, maxAlpha);
        speed = Random.Range(minSpeed, maxSpeed);
    }
}
