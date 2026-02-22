using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

public class MazeLogic : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 15;
    public int depth = 15;
    public int scale = 6;

    [Header("Objects")]
    public List<GameObject> Cube; // List Dinding
    public GameObject LantaiPrefab;
    public GameObject AtapPrefab;// <-- TAMBAHAN: Masukkan Prefab Lantai di sini
    public GameObject Character;

    [Header("Room & Quiz Settings")]
    public int Roomcount = 3;
    public int RoomMiniSize = 6;
    public int RoomMaxSize = 10;

    [Header("Quiz Components")]
    public GameObject gerbangKuisPrefab;
    public UjianManager ujianManager;

    public byte[,] map;

    void Start()
    {
        InitialiseMap();
        AddRoomsAndQuiz(Roomcount, RoomMiniSize, RoomMaxSize);
        GenerateMaps();

        // PENTING: DrawMaps sekarang akan membuat lantai juga
        DrawMaps();

        PlaceCharacter();
    }

    void InitialiseMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1; // Awalnya semua dinding
            }
    }

    public virtual void GenerateMaps() { } // Di-override RecursiveDFS

    void DrawMaps()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x * scale, 0, z * scale);

                // --- 1. SELALU SPAWN LANTAI DI SETIAP KOTAK ---
                if (LantaiPrefab != null)
                {
                    GameObject lantai = Instantiate(LantaiPrefab, pos, Quaternion.identity);                  
                    lantai.transform.localScale = new Vector3(scale, 0.1f, scale);
                    lantai.name = $"Lantai_{x}_{z}";
                }
                if (AtapPrefab != null)
                {
                    // Posisi atap di ketinggian sesuai scale (tinggi dinding)
                    // Asumsi scale dinding Anda = 6, maka atap di Y=6
                    Vector3 posAtap = pos + new Vector3(0, scale+3, 0);

                    // Rotasi 180 di sumbu X agar menghadap ke bawah
                    GameObject atap = Instantiate(AtapPrefab, posAtap, Quaternion.Euler(180, 0, 0), transform);
                    atap.transform.localScale = new Vector3(scale, 0.1f, scale);
                    atap.name = $"Atap_{x}_{z}";
                }
                // -----------------------------------------------

                // --- 2. JIKA DINDING (1), SPAWN DINDING ---
                if (map[x, z] == 1)
                {
                    if (Cube.Count > 0)
                    {
                        GameObject wall = Instantiate(Cube[Random.Range(0, Cube.Count)], pos, Quaternion.identity);
                        wall.transform.localScale = new Vector3(scale, scale * 1.5f, scale); // Tinggi dinding
                        wall.name = $"Dinding_{x}_{z}";
                    }
                }
            }
    }

    // ... (Fungsi CountSquareNeighbours, PlaceCharacter, AddRoomsAndQuiz TETAP SAMA seperti sebelumnya)
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        return count;
    }

    public virtual void PlaceCharacter()
    {
        bool PlayerSet = false;
        int attempts = 0;
        while (!PlayerSet && attempts < 1000)
        {
            attempts++;
            int x = Random.Range(1, width - 1);
            int z = Random.Range(1, depth - 1);

            // Cari posisi yang BUKAN dinding (0 = jalan, 2 = ruangan)
            if (map[x, z] == 0 || map[x, z] == 2)
            {
                // Pindahkan Player ke sana
                Character.transform.position = new Vector3(x * scale, 1, z * scale); // Y=1 biar gak nyangkut
                PlayerSet = true;
                Debug.Log($"Player ditempatkan di {x}, {z}");
            }
        }
    }

    public virtual void AddRoomsAndQuiz(int count, int miniSize, int maxSize)
    {
        SetSoal[] daftarSoal = null;
        if (ujianManager != null) daftarSoal = ujianManager.level1Soal;

        int soalIndex = 0;

        for (int c = 0; c < count; c++)
        {
            bool ruanganBerhasilDibuat = false;
            int percobaan = 0;
            int batasPercobaan = 100;

            while (!ruanganBerhasilDibuat && percobaan < batasPercobaan)
            {
                percobaan++;

                int startX = Random.Range(2, width - 2); // Ubah batas safety jadi 2
                int startZ = Random.Range(2, depth - 2);
                int roomWidth = Random.Range(miniSize, maxSize);
                int roomDepth = Random.Range(miniSize, maxSize);

                // --- PERBAIKAN UTAMA DI SINI ---

                // 1. Hitung batas akhir yang valid (agar tidak keluar map)
                // Kita paksa berhenti jika melebihi (width - 2)
                int endX = Mathf.Min(startX + roomWidth, width - 2);
                int endZ = Mathf.Min(startZ + roomDepth, depth - 2);

                // 2. Cek apakah ukuran ruangan hasil pemotongan masih layak?
                // Jika terpotong jadi terlalu kecil, batalkan dan coba lagi
                if ((endX - startX) < miniSize || (endZ - startZ) < miniSize)
                {
                    continue;
                }

                // 3. Kosongkan Area (Berdasarkan batas valid tadi)
                for (int x = startX; x < endX; x++)
                {
                    for (int z = startZ; z < endZ; z++)
                    {
                        map[x, z] = 2; // Jadi Ruangan
                    }
                }

                // 4. Spawn Gerbang Kuis
                if (gerbangKuisPrefab != null && daftarSoal != null && soalIndex < daftarSoal.Length)
                {
                    // Hitung titik tengah berdasarkan area yang BENAR-BENAR dibersihkan
                    int centerX = (startX + endX) / 2;
                    int centerZ = (startZ + endZ) / 2;

                    // --- PERBAIKAN: TEKNIK BULLDOZER (SAFETY CLEAR) ---
                    // Kita paksa bersihkan area 3x3 di sekitar titik tengah
                    // agar pintu tidak pernah tertanam di dinding
                    for (int cx = centerX - 1; cx <= centerX + 1; cx++)
                    {
                        for (int cz = centerZ - 1; cz <= centerZ + 1; cz++)
                        {
                            // Pastikan tidak keluar map
                            if (cx > 0 && cx < width - 1 && cz > 0 && cz < depth - 1)
                            {
                                map[cx, cz] = 2; // Paksa jadi lantai (Hancurkan dinding)
                            }
                        }
                    }
                    // --------------------------------------------------

                    Vector3 posKuis = new Vector3(centerX * scale, 0, centerZ * scale);

                    GameObject kuisObj = Instantiate(gerbangKuisPrefab, posKuis, Quaternion.identity);

                    SetupPintuKuis(kuisObj, daftarSoal[soalIndex]);
                    soalIndex++;
                }

                ruanganBerhasilDibuat = true;
            }
        }
    }

    void SetupPintuKuis(GameObject gerbangObj, SetSoal dataSoal)
    {
        // ... (COPY PASTE ISI FUNGSI INI DARI OBROLAN SEBELUMNYA) ...
        PintuJawaban[] pintu = gerbangObj.GetComponentsInChildren<PintuJawaban>();
        if (pintu.Length >= 2)
        {
            Vector3 startPoint = Vector3.zero;
            if (ujianManager != null && ujianManager.startPoint != null)
                startPoint = ujianManager.startPoint.position;

            int acak = Random.Range(0, 2);
            if (acak == 0)
            {
                pintu[0].SetDataPintu(dataSoal.teksPintuKiri, true, startPoint);
                pintu[1].SetDataPintu(dataSoal.teksPintuKanan, false, startPoint);
            }
            else
            {
                pintu[0].SetDataPintu(dataSoal.teksPintuKanan, false, startPoint);
                pintu[1].SetDataPintu(dataSoal.teksPintuKiri, true, startPoint);
            }
        }
    }
}