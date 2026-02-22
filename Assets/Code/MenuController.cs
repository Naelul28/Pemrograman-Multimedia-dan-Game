using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelMenu;
    public GameObject panelLevel;
    public GameObject panelTentang;

    // tombol MULAI
    public void OpenPanelLevel()
    {
        panelMenu.SetActive(false);
        panelLevel.SetActive(true);
        panelTentang.SetActive(false);
    }

    // tombol TENTANG
    public void OpenPanelTentang()
    {
        panelMenu.SetActive(false);
        panelLevel.SetActive(false);
        panelTentang.SetActive(true);
    }

    // tombol KEMBALI (dari Tentang & Level)
    public void BackToMainMenu()
    {
        panelMenu.SetActive(true);
        panelLevel.SetActive(false);
        panelTentang.SetActive(false);
    }

    // tombol KELUAR
    public void QuitGame()
    {
        Debug.Log("QUIT GAME"); // bekerja di Editor
        Application.Quit();     // bekerja saat build
    }
}
