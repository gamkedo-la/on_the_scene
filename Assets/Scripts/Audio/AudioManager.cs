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

        SFXTest = FMODUnity.RuntimeManager.CreateInstance("event:/Missions/MissionObjectiveComplete");
        MusicTest = FMODUnity.RuntimeManager.CreateInstance("event:/Music/MenuSong");
        ChopperSFXTest = FMODUnity.RuntimeManager.CreateInstance("event:/Chopper/Accelerate");

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(SFXTest, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(MusicTest, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ChopperSFXTest, GetComponent<Transform>(), GetComponent<Rigidbody>());

        InitializeVolumeLevels();
    }

    void InitializeVolumeLevels() {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
        float chopperSfxVolume = PlayerPrefs.GetFloat("chopperSfxVolume", 0.05f);
        float notChopperSfxVolume = PlayerPrefs.GetFloat("notChopperSfxVolume", 1f);

        master.setVolume(masterVolume);
        music.setVolume(musicVolume);
        SFX.setVolume(sfxVolume);
        chopperSFX.setVolume(chopperSfxVolume);
        notChopperSFX.setVolume(notChopperSfxVolume);
    }

    public void SetVolume(Slider volume)
    {
        Debug.Log(volume);
        playing = true;
        MusicTest.setVolume(volume.value);
        master.setVolume(volume.value);
        PlayerPrefs.SetFloat("masterVolume", volume.value);
        StartCoroutine(TestSound(MusicTest));
    }

    public void SetMusicVolume(Slider volume)
    {
        playing = true;
        MusicTest.setVolume(volume.value);
        music.setVolume(volume.value);
        PlayerPrefs.SetFloat("musicVolume", volume.value);
        StartCoroutine(TestSound(MusicTest));
    }

    public void SetSFXVolume(Slider volume)
    {
        playing = true;
        SFXTest.setVolume(volume.value);
        SFX.setVolume(volume.value);
        PlayerPrefs.SetFloat("sfxVolume", volume.value);
        StartCoroutine(TestSound(SFXTest));
    }

    public void SetChopperSFXVolume(Slider volume)
    {
        playing = true;
        ChopperSFXTest.setVolume(volume.value);
        chopperSFX.setVolume(volume.value);
        PlayerPrefs.SetFloat("chopperSfxVolume", volume.value);
        StartCoroutine(TestSound(ChopperSFXTest));
    }

    public void SetNotChopperSFXVolume(Slider volume)
    {
        playing = true;
        SFXTest.setVolume(volume.value);
        notChopperSFX.setVolume(volume.value);
        PlayerPrefs.SetFloat("notChopperSfxVolume", volume.value);
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
