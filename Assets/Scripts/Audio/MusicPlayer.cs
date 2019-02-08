using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string[] MusicEventList;
    public string[] MusicCreditsText;
    private FMOD.Studio.EventInstance music;
    public PauseMenuController PMC;
    public Text radioCredits;
    private bool stopMusic;

    private int songNum = 0;

    private Rigidbody cachedRigidBody;

    void Start()
    {
        cachedRigidBody = GetComponentInParent<Rigidbody>();
        if (cachedRigidBody == null)
        {
            Debug.Log("Unable to get rigidbody off helicontroller");
        }
        songNum = Random.Range(0,MusicEventList.Length);
        music = FMODUnity.RuntimeManager.CreateInstance(MusicEventList[songNum]);
        UpdateSongCredits();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());
        music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
        music.start();
    }

    void UpdateSongCredits()
    {
        radioCredits.text =
            "[ and ] change radio\nSong by "+MusicCreditsText[songNum];
    }

    void ChangeSong()
    {
        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        music.release();
        music = FMODUnity.RuntimeManager.CreateInstance(MusicEventList[songNum]);
        UpdateSongCredits();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());
        music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
        music.start();
    }

    // Update is called once per frame
    void Update()
    {
        if(PMC.GetPaused() == false) // avoid cycling song event instance while it's paused
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

        music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
        FMOD.Studio.PLAYBACK_STATE PBState;
        music.getPlaybackState(out PBState);
        stopMusic = PMC.GetPaused();
        if (PBState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            music.start();
            if (stopMusic)
            {
                music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }
}
