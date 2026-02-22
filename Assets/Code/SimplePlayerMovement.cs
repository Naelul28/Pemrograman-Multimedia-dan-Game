using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Gerakan WASD
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * moveSpeed;
        rb.MovePosition(rb.position + move * Time.deltaTime);

        // Lompat
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // cegah lompat ganda
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // kalau menyentuh tanah atau pijakan
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.name.Contains("Bridge"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // kalau lepas dari tanah
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.name.Contains("Bridge"))
        {
            isGrounded = false;
        }
    }
}
