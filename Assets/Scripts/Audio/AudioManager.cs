using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    private FMOD.Studio.Bus master;
    private FMOD.Studio.Bus music;
    private FMOD.Studio.Bus SFX;

    // Use this for initialization
    void Start()
    {
        master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        master.setVolume(1.0f);
        music.setVolume(0.2f);
        SFX.setVolume(0.2f);
    }

    public void SetVolume(Slider volume)
    {
        master.setVolume(volume.value);
    }

    public void SetMusicVolume(Slider volume)
    {
        music.setVolume(volume.value);
    }

    public void SetSFXVolume(Slider volume)
    {
        SFX.setVolume(volume.value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
