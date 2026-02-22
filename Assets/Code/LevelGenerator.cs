using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject lorongPrefab;
    public GameObject ruangSoalPrefab;

    [Header("Pengaturan")]
    public int jumlahLorongAwal = 2; // Lorong sebelum soal
    public float panjangKepingan = 10f;

    private Vector3 spawnPosition = Vector3.zero;
    public UjianManager ujianManager;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // 1. Ambil daftar soal
        SetSoal[] daftarSoal = ujianManager.level1Soal;

        // 2. Pilih SATU soal secara acak dari daftar
        int indexAcak = Random.Range(0, daftarSoal.Length);
        SetSoal soalTerpilih = daftarSoal[indexAcak];

        // 3. Spawn Lorong Awal (Pemanasan)
        for (int i = 0; i < jumlahLorongAwal; i++)
        {
            SpawnPrefab(lorongPrefab);
        }

        // 4. Spawn SATU Ruang Soal
        GameObject roomObj = SpawnPrefab(ruangSoalPrefab);
        QuizRoomSetup setupScript = roomObj.GetComponent<QuizRoomSetup>();

        if (setupScript != null)
        {
            // Kirim soal yang terpilih tadi
            setupScript.SiapkanDataRuangan(soalTerpilih);
        }

        // Selesai! Tidak perlu finish line karena pintu benar langsung pindah scene.
    }

    GameObject SpawnPrefab(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);
        spawnPosition += new Vector3(0, 0, panjangKepingan);
        return obj;
    }
}