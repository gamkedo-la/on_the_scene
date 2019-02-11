using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string MenuMusicEvent;
    private FMOD.Studio.EventInstance music;

    private GameObject menuPanel;
    private GameObject settingsPanel;
    private GameObject logo;

    void Awake()
    {
        menuPanel = GameObject.Find("MenuPanel");
        settingsPanel = GameObject.Find("SettingsPanel");
        logo = GameObject.Find("Logo");
        if (settingsPanel)
        {
            settingsPanel.SetActive(false);
        }
    }

    // Use this for initialization
    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(MenuMusicEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());

        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);

        music.setVolume(musicVolume);
        music.start();
    }

    public void ToggleSettings()
    {
        menuPanel.SetActive(settingsPanel.activeSelf);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        logo.SetActive(!logo.activeSelf);
    }

    void OnDestroy()
    {
        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        music.release();
    }
}
