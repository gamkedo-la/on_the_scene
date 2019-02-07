using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private bool playing = false;

    private FMOD.Studio.Bus master;
    private FMOD.Studio.Bus music;
    private FMOD.Studio.Bus SFX;
    private FMOD.Studio.Bus chopperSFX;
    private FMOD.Studio.Bus notChopperSFX;

    [FMODUnity.EventRef]
    private FMOD.Studio.EventInstance SFXTest;
    [FMODUnity.EventRef]
    private FMOD.Studio.EventInstance MusicTest;
    [FMODUnity.EventRef]
    private FMOD.Studio.EventInstance ChopperSFXTest;

    // Use this for initialization
    void Start()
    {
        master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        chopperSFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX/Chopper");
        notChopperSFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX/NotChopper");
        master.setVolume(1.0f);
        music.setVolume(0.5f);
        SFX.setVolume(0.5f);
        chopperSFX.setVolume(0.05f);
        notChopperSFX.setVolume(1.0f);

        SFXTest = FMODUnity.RuntimeManager.CreateInstance("event:/Missions/MissionObjectiveComplete");
        MusicTest = FMODUnity.RuntimeManager.CreateInstance("event:/Music/MenuSong");
        ChopperSFXTest = FMODUnity.RuntimeManager.CreateInstance("event:/Chopper/Accellerate");

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(SFXTest, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(MusicTest, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ChopperSFXTest, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    public void SetVolume(Slider volume)
    {
        playing = true;
        MusicTest.setVolume(volume.value);
        master.setVolume(volume.value);
        StartCoroutine(TestSound(MusicTest));
    }

    public void SetMusicVolume(Slider volume)
    {
        playing = true;
        MusicTest.setVolume(volume.value);
        music.setVolume(volume.value);
        StartCoroutine(TestSound(MusicTest));
    }

    public void SetSFXVolume(Slider volume)
    {
        playing = true;
        SFXTest.setVolume(volume.value);
        SFX.setVolume(volume.value);
        StartCoroutine(TestSound(SFXTest));
    }

    public void SetChopperSFXVolume(Slider volume)
    {
        playing = true;
        ChopperSFXTest.setVolume(volume.value);
        chopperSFX.setVolume(volume.value);
        StartCoroutine(TestSound(ChopperSFXTest));
    }

    public void SetNotChopperSFXVolume(Slider volume)
    {
        playing = true;
        SFXTest.setVolume(volume.value);
        notChopperSFX.setVolume(volume.value);
        StartCoroutine(TestSound(SFXTest));
    }

    IEnumerator TestSound(FMOD.Studio.EventInstance instance)
    {
        while (playing)
        {
            instance.start();
            yield return new WaitForSecondsRealtime(1.8f);
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            playing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
