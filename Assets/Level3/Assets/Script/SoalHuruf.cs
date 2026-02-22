using UnityEngine;
using TMPro;

public class SoalHuruf : MonoBehaviour
{
    [Header("UI Teks di Pintu")]
    public TMP_Text teksKiri;
    public TMP_Text teksKanan;

    [Header("Animator Pintu")]
    public Animator pintuKiri;
    public Animator pintuKanan;

    private string[] hurufBenar = { "ن", "م", "ي", "و" };
    private string[] hurufSalah = 
    { 
        "ء","أ","ب","ت","ث","ج","ح","خ","د","ذ","ر","ز","س","ش","ص","ض","ط","ظ","ع","غ","ف","ق","ك","ل","ه"
    };

    private bool kananAdalahBenar;

    void Start()
    {
        // tentukan huruf benar
        string jawabanBenar = hurufBenar[Random.Range(0, hurufBenar.Length)];

        // tentukan huruf salah, jangan sama
        string jawabanSalah;
        do {
            jawabanSalah = hurufSalah[Random.Range(0, hurufSalah.Length)];
        } while (jawabanSalah == jawabanBenar);

        // random posisi yang benar
        kananAdalahBenar = Random.value > 0.5f;

        if (kananAdalahBenar)
        {
            teksKanan.text = jawabanBenar;
            teksKiri.text = jawabanSalah;
        }
        else
        {
            teksKanan.text = jawabanSalah;
            teksKiri.text = jawabanBenar;
        }
    }

private bool pintuSudahDibuka = false;

public void BukaPintuBenar()
{
    if (pintuSudahDibuka) return; // mencegah double trigger
    pintuSudahDibuka = true;

    if (kananAdalahBenar)
        pintuKanan.SetBool("IsOpen", true);
    else
        pintuKiri.SetBool("IsOpen", true);
}

}