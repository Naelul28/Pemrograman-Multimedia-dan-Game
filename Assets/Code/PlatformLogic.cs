using UnityEngine;

public class PlatformLogic : MonoBehaviour
{
    public bool isCorrect = false;  // true hanya untuk pijakan benar
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;   // awalnya diam
            rb.isKinematic = true;   // kokoh dan tidak jatuh
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isCorrect)
            {
                Debug.Log($"{gameObject.name} adalah pijakan benar!");
            }
            else
            {
                Debug.Log($"{gameObject.name} salah!");

                if (rb != null)
                {
                    StartCoroutine(FallAfterDelay(0f)); // jatuh setelah 0.2 detik 
                }
            }
        }
    }

    private System.Collections.IEnumerator FallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.isKinematic = false;  // aktifkan fisika
        rb.useGravity = true;    // jatuh
    }
}
