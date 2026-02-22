using UnityEngine;

public class RintanganMover : MonoBehaviour
{
    public float moveSpeed = 5f; // kecepatan gerak
    public Vector3 moveDirection = Vector3.forward; // arah gerak

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
