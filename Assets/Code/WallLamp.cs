using UnityEngine;

public class WallLamp : MonoBehaviour
{
    [Header("Pengaturan")]
    [Range(0f, 1f)] public float peluangMuncul = 0.2f; // 20% kemungkinan muncul

    void Start()
    {
        // 1. LOGIKA ACAK (Muncul atau Tidak?)
        float acak = Random.value; // Angka acak 0.0 sampai 1.0
        if (acak > peluangMuncul)
        {
            // Jika gagal acak, hancurkan/matikan lampu ini
            gameObject.SetActive(false);
            return;
        }

        // 2. LOGIKA ANTI-GEPENG (Fix Scale)
        // Karena dinding di-scale (misal: 4, 3, 0.2), lampu ikut gepeng.
        // Kita harus membalikan scale-nya agar kembali kotak (1, 1, 1).

        if (transform.parent != null)
        {
            Vector3 parentScale = transform.parent.localScale;

            // Rumus: Scale Baru = 1 / Scale Orang Tua
            // Contoh: Jika tembok tebalnya 0.2, kita harus scale lampu 5x lipat (1/0.2) biar normal.
            transform.localScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            ) * 0.7f; // Dikali 0.5f agar ukuran lampu jadi setengah meter (sesuaikan selera)
        }
    }
}