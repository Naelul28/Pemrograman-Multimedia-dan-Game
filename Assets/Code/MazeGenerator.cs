using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MazeGenerator : MonoBehaviour
{
    [Header("Komponen")]
    public GameObject dindingPrefab;
    public GameObject lantaiPrefab;
    public GameObject pintuPrefab; // Prefab PintuJawaban Anda
    public GameObject atapPrefab;  // <-- SUDAH ADA (Benar)

    [Header("Pengaturan Maze")]
    public int lebarMaze = 10;
    public int panjangMaze = 10;
    public float ukuranSel = 4f;
    public float tinggiDinding = 3f;

    [Header("Referensi")]
    public UjianManager ujianManager;
    public Transform player;

    // Struktur Data untuk Sel Maze
    public class MazeCell
    {
        public bool visited = false;
        public GameObject dindingAtas;
        public GameObject dindingBawah;
        public GameObject dindingKiri;
        public GameObject dindingKanan;
        public Vector2Int posisi;
    }

    private MazeCell[,] grid;

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // 1. Ambil Soal Acak
        if (ujianManager == null || ujianManager.level1Soal.Length == 0) return;

        SetSoal[] daftarSoal = ujianManager.level1Soal;
        SetSoal soal = daftarSoal[Random.Range(0, daftarSoal.Length)];

        // 2. Inisialisasi Grid (Buat Lantai & Dinding Penuh)
        InitGrid();

        // 3. Tentukan Posisi Start (Tengah Bawah)
        int startX = lebarMaze / 2;
        int startY = 0;
        Vector2Int startPos = new Vector2Int(startX, startY);

        // Posisikan Player
        Vector3 playerPos = new Vector3(startX * ukuranSel, 1, startY * ukuranSel);
        if (player != null) player.position = playerPos;

        // Tandai start visited
        grid[startX, startY].visited = true;

        // 4. BUAT PERCABANGAN & SPAWN PINTU
        Vector3 startPointHukuman = playerPos;

        // --- CABANG A (KIRI) ---
        Vector2Int posKiri = new Vector2Int(startX - 1, startY);
        HancurkanDinding(startPos, posKiri);
        SpawnPintu(startPos, posKiri, soal, true, startPointHukuman);

        // --- CABANG B (KANAN) ---
        Vector2Int posKanan = new Vector2Int(startX + 1, startY);
        HancurkanDinding(startPos, posKanan);
        SpawnPintu(startPos, posKanan, soal, false, startPointHukuman);

        // 5. GALI LABIRIN
        GaliJalur(posKiri.x, posKiri.y);
        GaliJalur(posKanan.x, posKanan.y);
    }

    void InitGrid()
    {
        grid = new MazeCell[lebarMaze, panjangMaze];

        for (int x = 0; x < lebarMaze; x++)
        {
            for (int y = 0; y < panjangMaze; y++)
            {
                Vector3 pos = new Vector3(x * ukuranSel, 0, y * ukuranSel);

                // 1. Spawn Lantai
                Instantiate(lantaiPrefab, pos, Quaternion.identity, transform);

                // 2. Spawn Atap (BARU: SUDAH DIPERBAIKI)
                if (atapPrefab != null)
                {
                    Vector3 posAtap = pos + new Vector3(0, tinggiDinding, 0);
                    // Putar 180 derajat agar menghadap ke bawah
                    Instantiate(atapPrefab, posAtap, Quaternion.Euler(180, 0, 0), transform);
                }

                MazeCell cell = new MazeCell();
                cell.posisi = new Vector2Int(x, y);

                // 3. Spawn Dinding (Scale otomatis diatur script)
                GameObject dAtas = Instantiate(dindingPrefab, pos + new Vector3(0, tinggiDinding / 2, ukuranSel / 2), Quaternion.identity, transform);
                dAtas.transform.localScale = new Vector3(ukuranSel, tinggiDinding, 0.2f);
                cell.dindingAtas = dAtas;

                GameObject dKanan = Instantiate(dindingPrefab, pos + new Vector3(ukuranSel / 2, tinggiDinding / 2, 0), Quaternion.Euler(0, 90, 0), transform);
                dKanan.transform.localScale = new Vector3(ukuranSel, tinggiDinding, 0.2f);
                cell.dindingKanan = dKanan;

                // Link ke tetangga (untuk referensi penghancuran)
                if (y > 0) cell.dindingBawah = grid[x, y - 1].dindingAtas;
                if (x > 0) cell.dindingKiri = grid[x - 1, y].dindingKanan;

                grid[x, y] = cell;
            }
        }
    }

    void GaliJalur(int x, int y)
    {
        grid[x, y].visited = true;

        List<Vector2Int> tetangga = new List<Vector2Int> {
            new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0)
        };
        Shuffle(tetangga);

        foreach (Vector2Int arah in tetangga)
        {
            int nx = x + arah.x;
            int ny = y + arah.y;

            if (nx >= 0 && nx < lebarMaze && ny >= 0 && ny < panjangMaze && !grid[nx, ny].visited)
            {
                HancurkanDinding(new Vector2Int(x, y), new Vector2Int(nx, ny));
                GaliJalur(nx, ny);
            }
        }
    }

    void HancurkanDinding(Vector2Int a, Vector2Int b)
    {
        if (a.x == b.x) // Atas/Bawah
        {
            if (a.y < b.y) Destroy(grid[a.x, a.y].dindingAtas);
            else Destroy(grid[b.x, b.y].dindingAtas);
        }
        else // Kiri/Kanan
        {
            if (a.x < b.x) Destroy(grid[a.x, a.y].dindingKanan);
            else Destroy(grid[b.x, b.y].dindingKanan);
        }
    }

    // --- PERBAIKAN UTAMA FUNGSI SPAWN PINTU (MENGATASI ERROR CS0029) ---
    void SpawnPintu(Vector2Int dari, Vector2Int ke, SetSoal soal, bool isJalurKiri, Vector3 hukuman)
    {
        Vector3 posDari = new Vector3(dari.x * ukuranSel, 0, dari.y * ukuranSel);
        Vector3 posKe = new Vector3(ke.x * ukuranSel, 0, ke.y * ukuranSel);
        Vector3 posisiPintu = (posDari + posKe) / 2;
        Quaternion rotasiPintu = Quaternion.LookRotation(posKe - posDari);

        // 1. Spawn
        GameObject pintuObj = Instantiate(pintuPrefab, posisiPintu, rotasiPintu);

        // 2. Ambil Script PintuJawaban
        PintuJawaban scriptPintu = pintuObj.GetComponent<PintuJawaban>();

        // 3. Set Data (Teks & Logika Benar/Salah)
        if (scriptPintu != null)
        {
            if (isJalurKiri)
            {
                // Kiri = Benar (Contoh logika, nanti diacak otomatis oleh SetDataPintu jika mau)
                scriptPintu.SetDataPintu(soal.teksPintuKiri, true, hukuman);
            }
            else
            {
                // Kanan = Salah
                scriptPintu.SetDataPintu(soal.teksPintuKanan, false, hukuman);
            }
        }
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}