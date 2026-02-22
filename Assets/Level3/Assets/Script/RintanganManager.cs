using UnityEngine;

public class RintanganManager : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 10f;

    private bool sudahDijalankan = false;
    private SoalHuruf soal;

    void Start()
    {
        soal = GetComponent<SoalHuruf>();

        // otomatis cari player lewat Tag
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (!sudahDijalankan && Vector3.Distance(player.position, transform.position) < triggerDistance)
        {
            sudahDijalankan = true;
            soal.BukaPintuBenar();
        }
    }
}
