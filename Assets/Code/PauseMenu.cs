using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;

    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;

    [Header("Audio")]
    [SerializeField] private AudioSource bgmSource;

    [Header("Scene Navigation")]
    [SerializeField] private string mainMenuScene = "main_menu";

    void Start()
    {
        // Pastikan awalnya tidak pause
        GamePaused = false;
        Time.timeScale = 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Volume SELALU default 1 setiap game dibuka
        float defaultVolume = 1f;

        if (bgmSource != null)
            bgmSource.volume = defaultVolume;

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = defaultVolume;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }

        // Mode cursor saat sedang main (FPS)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        GamePaused = true;

        // === Cursor untuk UI ===
        Cursor.lockState = CursorLockMode.None;   // lepas kunci
        Cursor.visible = true;                    // tampilkan cursor
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        GamePaused = false;

        // === Kembalikan ke mode gameplay ===
        Cursor.lockState = CursorLockMode.Locked; // kunci di tengah (kalau FPS)
        Cursor.visible = false;                   // sembunyikan cursor
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        GamePaused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuScene);
    }

    void UpdateVolume(float value)
    {
        if (bgmSource != null)
            bgmSource.volume = value;   // cuma ubah volume sekarang, tidak disimpan
    }
}
