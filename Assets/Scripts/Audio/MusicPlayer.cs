﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string MusicEvent;
    private FMOD.Studio.EventInstance music;
    public PauseMenuController PMC;
    private bool stopMusic;

    private Rigidbody cachedRigidBody;

    void Start()
    {
        cachedRigidBody = GetComponentInParent<Rigidbody>();
        if (cachedRigidBody == null)
        {
            Debug.Log("Unable to get rigidbody off helicontroller");
        }
        music = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());
        music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
        music.start();
    }

    // Update is called once per frame
    void Update()
    {
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
