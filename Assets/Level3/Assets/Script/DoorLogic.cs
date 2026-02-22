using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    public Animator anim;
    public bool AdalahBenar;   // True = pintu jawaban benar
    public float PushForce = 12f;

    bool sudahTerbuka = false;

    public void BukaPintu()
    {
        if (sudahTerbuka) return; // Cegah animasi dipanggil 2x
        sudahTerbuka = true;

        anim.SetBool("IsOpen", true);
    }
}
