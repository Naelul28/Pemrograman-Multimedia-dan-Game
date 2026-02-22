using UnityEngine;
using UnityEngine.SceneManagement; // Kita perlu ini untuk mengelola scene

/// <summary>
/// Skrip ini akan me-restart level (memuat ulang scene)
/// jika pemain menyentuhnya.
/// </summary>
public class DeathZone : MonoBehaviour
{
    // Fungsi ini akan dipanggil BANYAK SATU KALI
    // ketika sebuah collider masuk ke trigger ini.
    // Pastikan collider ini di-set sebagai 'Is Trigger'.
    void OnTriggerEnter(Collider other)
    {
        // Pertama, kita periksa apakah yang menyentuh kita adalah "Player"
        // (Pastikan objek Player Anda memiliki Tag "Player")
        if (other.gameObject.CompareTag("Player"))
        {
            // Tampilkan pesan di konsol untuk debugging
            Debug.Log("Pemain jatuh ke Death Zone! Mengulang level...");

            // Muat ulang scene yang sedang aktif saat ini.
            // Ini adalah cara termudah untuk "mengulang" game dan
            // mengembalikan player ke posisi awal.
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}