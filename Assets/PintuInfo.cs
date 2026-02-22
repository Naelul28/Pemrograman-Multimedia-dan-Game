using UnityEngine;
using TMPro;

public class PintuInfo : MonoBehaviour
{
    public TextMeshPro teksJawaban; // Drag TextMeshPro di atas pintu ke sini

    public void SetTeks(string teks)
    {
        if (teksJawaban != null) teksJawaban.text = teks;
    }
}