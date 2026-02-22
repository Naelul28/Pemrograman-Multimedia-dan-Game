using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundcheck : MonoBehaviour
{
    PlayerMovement logicMovement;
    public string groundTag = "Ground";

    // Waktu toleransi (0.1 detik biasanya cukup untuk menutup celah antar object)
    [SerializeField] private float toleranceTime = 0.15f;
    private float lastTimeTouchedGround;

    void Start()
    {
        logicMovement = this.GetComponentInParent<PlayerMovement>();
        if (logicMovement == null)
        {
            Debug.LogError("Tidak menemukan PlayerMovement di parent!");
        }
        // Inisialisasi agar tidak langsung jatuh saat start
        lastTimeTouchedGround = Time.time;
    }

    // Gunakan OnTriggerStay, bukan Enter. 
    // Ini berjalan SETIAP FRAME selama nempel.
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(groundTag))
        {
            // Selalu update "waktu terakhir menyentuh tanah" ke waktu sekarang
            lastTimeTouchedGround = Time.time;
        }
    }

    private void Update()
    {
        // LOGIKA UTAMA:
        // Cek selisih waktu sekarang dengan waktu terakhir sentuh tanah.
        if (Time.time - lastTimeTouchedGround <= toleranceTime)
        {
            // Masih dianggap di tanah (walaupun sedang pindah objek)
            logicMovement.SetGrounded(true);
        }
        else
        {
            // Sudah melewati batas toleransi, berarti benar-benar terbang
            logicMovement.SetGrounded(false);
        }
    }

    // Kita tidak butuh OnTriggerEnter atau Exit lagi untuk logika ini.
    // Tapi jika ingin debug visual, bisa biarkan kosong atau hapus.
}