using UnityEngine;
using TMPro;

public class QuizRoomSetup : MonoBehaviour
{
    [Header("Komponen Text")]
    public TextMeshPro textKiri;
    public TextMeshPro textKanan;

    [Header("Pintu")]
    public PintuJawaban scriptPintuKiri;
    public PintuJawaban scriptPintuKanan;

    public void SiapkanDataRuangan(SetSoal dataSoal)
    {
        // Cari titik start untuk hukuman
        UjianManager manager = FindObjectOfType<UjianManager>();
        Vector3 titikStart = Vector3.zero;
        if (manager != null && manager.startPoint != null) titikStart = manager.startPoint.position;

        // --- LOGIKA PENGACAKAN ---
        // Lempar koin: 0 atau 1
        // Jika 0 = Kiri Benar. Jika 1 = Kanan Benar.
        int acak = Random.Range(0, 2);

        if (acak == 0)
        {
            // --- POSISI 1: KIRI ITU BENAR ---
            // Teks Kiri diisi Jawaban Benar (dari data Pintu Kiri)
            textKiri.text = dataSoal.teksPintuKiri;
            textKanan.text = dataSoal.teksPintuKanan;

            // Setting Pintu
            AturPintu(scriptPintuKiri, true, titikStart);  // Kiri Benar
            AturPintu(scriptPintuKanan, false, titikStart); // Kanan Salah
        }
        else
        {
            // --- POSISI 2: KANAN ITU BENAR (DIBALIK) ---
            // Teks Kiri diisi Jawaban Salah (dari data Pintu Kanan)
            textKiri.text = dataSoal.teksPintuKanan;
            // Teks Kanan diisi Jawaban Benar (dari data Pintu Kiri)
            textKanan.text = dataSoal.teksPintuKiri;

            // Setting Pintu
            AturPintu(scriptPintuKiri, false, titikStart); // Kiri Salah
            AturPintu(scriptPintuKanan, true, titikStart);  // Kanan Benar
        }
    }

    void AturPintu(PintuJawaban pintu, bool benar, Vector3 lokasiHukuman)
    {
        if (pintu != null)
        {
            pintu.adalahPintuBenar = benar;
            pintu.lokasiTeleportJikaSalah = lokasiHukuman;
        }
    }
}