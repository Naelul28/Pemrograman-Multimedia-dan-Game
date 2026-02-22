using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverPop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Pop Settings")]
    public RectTransform target;     // RectTransform dari button
    public float hoverScale = 10.08f; // seberapa besar pop
    public float speed = 8f;         // kecepatan animasi

    private Vector3 originalScale;

    void Start()
    {
        if (target == null) target = GetComponent<RectTransform>();
        originalScale = target.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(target, originalScale * hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(target, originalScale));
    }

    System.Collections.IEnumerator ScaleTo(RectTransform obj, Vector3 targetScale)
    {
        while (Vector3.Distance(obj.localScale, targetScale) > 0.001f)
        {
            obj.localScale = Vector3.Lerp(obj.localScale, targetScale, Time.deltaTime * speed);
            yield return null;
        }

        obj.localScale = targetScale;
    }
}
