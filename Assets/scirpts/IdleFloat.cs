using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleFloat : MonoBehaviour
{
    [Header("Idle Settings")]
    public float amplitude = 20f;   // Jarak naik-turun (pixel)
    public float speed = 2f;        // Kecepatan animasi

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = new Vector3(startPos.x, startPos.y + y, startPos.z);
    }
}
