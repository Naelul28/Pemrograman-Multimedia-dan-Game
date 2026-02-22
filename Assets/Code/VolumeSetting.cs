using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;   // Audio Source untuk musik
    [SerializeField] private Slider volumeSlider;       // Slider UI untuk volume

    private void Start()
    {
        // Load volume terakhir yang disimpan
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Ketika slider digeser, update volume
        volumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);   // Simpan agar tetap saat restart game
    }
}
