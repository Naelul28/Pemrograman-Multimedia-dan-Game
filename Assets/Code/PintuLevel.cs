using UnityEngine;
using TMPro; // <-- Jangan lupa
using UnityEngine.SceneManagement; // <-- Sangat penting

public class PintuLevel : MonoBehaviour
{
    [Header("Pengaturan")]
    // 1. Drag objek UI Text (TMP) Anda ke sini (misal: "InteractionPrompt")
    public TextMeshProUGUI promptText;

    // 2. Tulis nama dasar scene level Anda
    // Skrip akan menggabung ini menjadi "Level1", "Level2", dst.
    public string namaSceneLevel = "Level";

    private bool isPlayerNear = false;
    private int levelIndexToLoad; // Level yang akan dimuat

    void Start()
    {
        // Pastikan prompt kosong di awal
        if (promptText != null) promptText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // "Tanya" ke GameManager
            if (GameManager.instance == null)
            {
                Debug.LogError("GameManager tidak ditemukan!");
                return;
            }

            // Ambil level saat ini dari GameManager
            levelIndexToLoad = GameManager.instance.currentLevelIndex;

            // Tampilkan prompt yang sesuai ke player
            if (promptText != null)
            {
                // \n adalah karakter untuk "baris baru"
                promptText.text = "Tekan [F]\n";
            }

            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Sembunyikan prompt saat player pergi
            if (promptText != null) promptText.text = "";
            isPlayerNear = false;
        }
    }

    private void Update()
    {
        // Cek jika player dekat DAN menekan tombol F
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            // Sembunyikan prompt sebelum pindah
            if (promptText != null) promptText.text = "";

            // Buat nama scene yang benar, misal: "Level" + 1 = "Level1"
            string sceneToLoad = namaSceneLevel + levelIndexToLoad;

            Debug.Log("Memuat scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}