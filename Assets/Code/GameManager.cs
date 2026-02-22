using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Ini adalah 'Singleton pattern', cara mudah untuk mengakses skrip ini dari mana saja
    public static GameManager instance;

    // Ini akan menyimpan progres player
    public int currentLevelIndex = 1;

    void Awake()
    {
        // Pengaturan Singleton
        if (instance == null)
        {
            instance = this;
            // JANGAN hancurkan objek ini saat pindah scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jika sudah ada GameManager lain, hancurkan yang ini
            Destroy(gameObject);
        }
    }
}