using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    // Variabel untuk menyimpan referensi ke script utama di induk
    private PlayerMovement parentScript;

    void Start()
    {
        // Cari script PlayerMovement di objek induk (Parent)
        parentScript = GetComponentInParent<PlayerMovement>();

        if (parentScript == null)
        {
            Debug.LogError("Script PlayerMovement tidak ditemukan di Parent!");
        }
    }

    // Fungsi ini namanya HARUS SAMA dengan yang ditulis di Animation Event
    public void Footstep()
    {
        // Teruskan perintah ke script induk
        if (parentScript != null)
        {
            parentScript.Footstep();
        }
    }
}