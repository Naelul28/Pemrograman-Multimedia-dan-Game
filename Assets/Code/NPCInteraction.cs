using UnityEngine;
using TMPro;
using Cinemachine;
using System.Collections;
using UnityEngine.SceneManagement; // <-- Tambahkan ini untuk pindah scene

public class NPCInteraction : MonoBehaviour
{
    // ... (Semua variabel UI & Kamera Anda tetap sama) ...
    public GameObject interactionPrompt;
    public CinemachineVirtualCamera dialogueCamera;
    public GameObject dialoguePanel;
    public TextMeshProUGUI textComponent;
    public float textSpeed;

    [Header("Data Dialog Per Level")]
    public string[] level1Lines;
    public string[] level2Lines;
    public string[] level3Lines;

    private string[] currentLines; // Dialog yang akan digunakan sekarang
    private int index;
    // ... (sisa variabel player) ...
    private PlayerMovement playerMovementScript;
    private Transform playerTransform;
    private bool isPlayerInRange = false;

    // ... (Start() dan Update() tetap sama) ...

    void Start()
    {
        // ... (kode Anda untuk sembunyikan UI, dll) ...
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (dialogueCamera != null) dialogueCamera.Priority = 5;
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            // Saat mulai dialog, tentukan dulu dialog mana yang mau dipakai
            StartDialogue();
        }

        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponent.text == currentLines[index]) // <-- Cek 'currentLines'
            {
                NextLine();
            }
            // ... (sisa logika 'else' Anda) ...
            else
            {
                StopAllCoroutines();
                textComponent.text = currentLines[index]; // <-- Cek 'currentLines'
            }
        }
    }

    void StartDialogue()
    {
        // --- "Tanya" GameManager, kita di level berapa? ---
        if (GameManager.instance.currentLevelIndex == 1)
        {
            currentLines = level1Lines;
        }
        else if (GameManager.instance.currentLevelIndex == 2)
        {
            currentLines = level2Lines;
        }
        else if (GameManager.instance.currentLevelIndex == 3)
        {
            currentLines = level3Lines;
        }
        // ---------------------------------------------------

        // --- Sisa Logika Dialog Anda ---
        Debug.Log("Memulai Dialog...");
        interactionPrompt.SetActive(false);
        if (playerMovementScript != null) playerMovementScript.enabled = false;

        // Kode untuk membuat NPC menoleh ke player
        if (playerTransform != null)
        {
            Vector3 lookPosition = playerTransform.position;
            lookPosition.y = transform.position.y;
            transform.LookAt(lookPosition);
        }

        // Kode untuk memindahkan kamera dialog
        if (dialogueCamera != null && playerTransform != null)
        {
            Vector3 offset = (playerTransform.forward * -2.0f)
                             + (playerTransform.right * 0.7f)
                             + (Vector3.up * 1.6f);
            dialogueCamera.transform.position = playerTransform.position + offset;
            dialogueCamera.Priority = 20;
        }

        // Tampilkan UI dan mulai dialog
        dialoguePanel.SetActive(true);
        isPlayerInRange = false;

        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Gunakan 'currentLines'
        foreach (char c in currentLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        // Gunakan 'currentLines'
        if (index < currentLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            // Dialog Selesai!
            EndDialogue(true); // Kirim 'true' yang berarti "lanjut ke level"
        }
    }

    // Kita modifikasi EndDialogue sedikit
    public void EndDialogue(bool lanjutKeLevel)
    {
        Debug.Log("Mengakhiri Dialog...");
        if (playerMovementScript != null) playerMovementScript.enabled = true;
        if (dialogueCamera != null) dialogueCamera.Priority = 5;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        textComponent.text = string.Empty;

        
    }

    // ... (OnTriggerEnter dan OnTriggerExit tetap sama) ...
    // Jangan lupa panggil EndDialogue(false) jika player pergi
    private void OnTriggerEnter(Collider other)
    {
        

        // 1. Gunakan GetComponent untuk mengecek apakah ini Player utama (bukan GroundCheck)
        PlayerMovement movementScript = other.GetComponent<PlayerMovement>();

        if (movementScript != null) // Ini hanya akan benar untuk Player utama
        {
            
            interactionPrompt.SetActive(true);
            isPlayerInRange = true;

            // 2. INI BAGIAN PENTING YANG HILANG:
            // Simpan referensi skrip player agar StartDialogue bisa menggunakannya
            playerMovementScript = movementScript;

            // 3. Simpan juga transform-nya
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Gunakan GetComponent juga di sini
        PlayerMovement movementScript = other.GetComponent<PlayerMovement>();

        // Cek jika Player utama (bukan GroundCheck) yang KELUAR
        if (movementScript != null)
        {
            Debug.Log("Player (Badan) KELUAR. Menghilangkan prompt...");
            interactionPrompt.SetActive(false); // <-- INI YANG MENGHILANGKAN PROMPT
            isPlayerInRange = false;

            // Logika untuk membatalkan dialog jika player pergi
            if (playerMovementScript != null && playerMovementScript.enabled == false)
            {
                EndDialogue(false);
            }
        }
    }
    }