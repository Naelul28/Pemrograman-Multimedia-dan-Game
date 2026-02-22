using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinishDoorLogic : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public GameObject panelSelesai;

    public AudioSource audioBacksound; // musik game
    public AudioSource audioWin;       // musik kemenangan

    bool playerNear = false;

    void Start()
    {
        promptText.gameObject.SetActive(false);
        panelSelesai.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            playerNear = true;
            promptText.text = "Tekan F";
            promptText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            playerNear = false;
            promptText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.F))
        {
            // Show UI
            panelSelesai.SetActive(true);
            promptText.gameObject.SetActive(false);

            // Stop gameplay & sound
            Time.timeScale = 0f;
            if (audioBacksound != null) audioBacksound.Stop();
            if (audioWin != null) audioWin.Play();

            Cursor.lockState = CursorLockMode.None; // Lepas kunci mouse
            Cursor.visible = true;
        }

        
    }
    public void KeMainMenu()
    {
        // PENTING: Kembalikan waktu agar game selanjutnya tidak freeze
        Time.timeScale = 1f;

        // Pindah ke scene main menu
        SceneManager.LoadScene("main_menu");
    }
}
