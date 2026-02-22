using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class pintuLevel2 : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    private bool playerIsNear = false;

    // Gunakan Start untuk memastikan prompt bersih di awal
    void Start()
    {
        if (promptText != null) promptText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        // PERBAIKAN 1: Cek PlayerMovement, bukan Tag
        // Agar tidak tertukar dengan GroundCheck
        if (other.GetComponent<PlayerMovement>() != null)
        {
            playerIsNear = true;
            if (promptText != null) promptText.text = "Tekan [F]";
            Debug.Log("Player Masuk Pintu Level 2");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // PERBAIKAN 1: Cek PlayerMovement juga di sini
        if (other.GetComponent<PlayerMovement>() != null)
        {
            playerIsNear = false;
            if (promptText != null) promptText.text = "";
            Debug.Log("Player Keluar Pintu Level 2");
        }
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Tombol F Ditekan!");

            // PERBAIKAN 2: Cek GameManager sebelum akses
            if (GameManager.instance != null)
            {
                GameManager.instance.currentLevelIndex = 3; // Set level selanjutnya
            }
            else
            {
                Debug.LogWarning("GameManager tidak ditemukan! Pastikan main dari scene awal.");
            }

            // Pindah Scene
            // Pastikan nama scene di sini SAMA PERSIS dengan di Build Settings
            // Apakah "Pase Instruksi" atau "Fase Instruksi"? Cek ejaannya.
            SceneManager.LoadScene("Pase Instruksi");

            if (promptText != null) promptText.text = "";
        }
    }
}