using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Logic : MonoBehaviour
{
    [Header("Panels")]

    [Header("Background Transition")]
    public BackgroundTransitionSprite bgTransition;

    public GameObject panelMainMenu;
    public GameObject panelAbout;
    public GameObject panelSetting;

    // ============================
    // MAIN MENU BUTTONS
    // ============================

    public void OnPlayButton()
    {
        bgTransition.FadeOutAll();
        SceneManager.LoadScene("Pase Instruksi");
    }

    public void OnAboutButton()
    {
        bgTransition.FadeOutAll();
        panelMainMenu.SetActive(false);
        panelAbout.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnSettingButton()
    {
        bgTransition.FadeOutAll();
        panelMainMenu.SetActive(false);
        panelSetting.SetActive(true);
    }

    // ============================
    // ABOUT PANEL
    // ============================

    public void OnCloseAbout()
    {
        panelAbout.SetActive(false);
        panelMainMenu.SetActive(true);
        bgTransition.FadeInAll();
    }

    // ============================
    // SETTING PANEL
    // ============================

    public void OnCloseSetting()
    {
        panelSetting.SetActive(false);
        panelMainMenu.SetActive(true);
        bgTransition.FadeInAll();
    }
}
