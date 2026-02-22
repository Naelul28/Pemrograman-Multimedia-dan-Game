using UnityEngine;

public class RintanganSpawner : MonoBehaviour
{
    public GameObject rintanganPrefab;
    public float spawnRate = 4f;   // waktu jeda antar spawn (detik)
    public Transform spawnPoint;   // posisi spawn

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnRintangan();
            timer = 0f;
        }
    }

    void SpawnRintangan()
    {
        Instantiate(rintanganPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
