using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string MenuMusicEvent;
    private FMOD.Studio.EventInstance music;

    // Use this for initialization
    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance(MenuMusicEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());
        music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody>()));

        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);

        music.setVolume(musicVolume);
        music.start();
    }

    void OnDestroy()
    {
        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        music.release();
    }
}
