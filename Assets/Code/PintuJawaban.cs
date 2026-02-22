using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PintuJawaban : MonoBehaviour
{
    [Header("UI Prompt")]
    public TextMeshProUGUI promptText;
    public TextMeshPro teksJawabanDiAtasPintu;

    // Variabel ini akan diisi otomatis oleh QuizRoomSetup
    [HideInInspector] public bool adalahPintuBenar = false;
    [HideInInspector] public Vector3 lokasiTeleportJikaSalah;

    private bool playerIsNear = false;
    private Transform playerTransform;


    void Start()
    {
        if (promptText != null) promptText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            playerIsNear = true;
            playerTransform = other.transform;
            if (promptText != null) promptText.text = "Tekan [F]";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            playerIsNear = false;
            if (promptText != null) promptText.text = "";
        }
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.F))
        {
            CekJawaban();
        }
    }
    public void SetDataPintu(string teks, bool benar, Vector3 lokasiHukuman)
    {
        if (teksJawabanDiAtasPintu != null) teksJawabanDiAtasPintu.text = teks;
        adalahPintuBenar = benar;
        lokasiTeleportJikaSalah = lokasiHukuman;
    }

    void CekJawaban()
    {
        if (promptText != null) promptText.text = "";

        if (adalahPintuBenar)
        {
            // --- JAWABAN BENAR = MENANG ---
            Debug.Log("Jawaban Benar! Level Selesai.");

            // 1. Update GameManager (Naik Level)
            if (GameManager.instance != null)
            {
                GameManager.instance.currentLevelIndex++;
            }

            // 2. Pindah Scene ke Pase Instruksi
            SceneManager.LoadScene("Pase Instruksi");
        }
        else
        {
            // --- JAWABAN SALAH = HUKUMAN ---
            Debug.Log("Jawaban Salah! Kembali ke awal.");

            if (playerTransform != null)
            {
                CharacterController cc = playerTransform.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;
                playerTransform.position = lokasiTeleportJikaSalah;
                if (cc != null) cc.enabled = true;
            }
        }
    }
}