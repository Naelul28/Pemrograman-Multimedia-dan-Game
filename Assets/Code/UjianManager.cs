using UnityEngine;

public class UjianManager : MonoBehaviour
{
    // --- TAMBAHKAN INI KEMBALI ---
    [Header("Referensi Scene")]
    public Transform startPoint; // <-- Pintu butuh ini untuk tahu lokasi hukuman
    // -----------------------------

    [Header("Data Soal (Database)")]
    public SetSoal[] level1Soal;
    public SetSoal[] level2Soal;
    public SetSoal[] level3Soal;
}