using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    [HideInInspector] public static MusicPlayer instance;

    [FMODUnity.EventRef]
    public string[] MusicEventList;
    public string[] MusicCreditsText;
    private FMOD.Studio.EventInstance music;
    public PauseMenuController PMC;
    public Text radioCredits;
    private bool stopMusic;
    private bool manuallyStoppedMusic = false;

    private int songNum = 0;

    void Start()
    {
        instance = this;
        songNum = Random.Range(0, MusicEventList.Length);
        music = FMODUnity.RuntimeManager.CreateInstance(MusicEventList[songNum]);
        UpdateSongCredits();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());
        music.start();
    }

    void UpdateSongCredits()
    {
        radioCredits.text =
            "[ and ] change radio\n< and > change helicopter\nSong by \n" + MusicCreditsText[songNum];
    }

    void ChangeSong()
    {
        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        music.release();
        music = FMODUnity.RuntimeManager.CreateInstance(MusicEventList[songNum]);
        UpdateSongCredits();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());
        music.start();
    }

    public void StopMusic()
    {
        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        manuallyStoppedMusic = true;
    }

    public void StartMusic()
    {
        manuallyStoppedMusic = false;
        music.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (PMC.GetPaused() == false) // avoid cycling song event instance while it's paused
        {
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                songNum--;
                if (songNum < 0)
                {
                    songNum = MusicEventList.Length - 1;
                }
                ChangeSong();
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                songNum++;
                if (songNum >= MusicEventList.Length)
                {
                    songNum = 0;
                }
                ChangeSong();
            }
        }

        FMOD.Studio.PLAYBACK_STATE PBState;
        music.getPlaybackState(out PBState);
        stopMusic = PMC.GetPaused();
        if (PBState != FMOD.Studio.PLAYBACK_STATE.PLAYING && manuallyStoppedMusic == false)
        {
            music.start();
            if (stopMusic)
            {
                music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }
}
