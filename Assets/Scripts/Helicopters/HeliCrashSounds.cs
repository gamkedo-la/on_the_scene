﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliCrashSounds : MonoBehaviour
{
    private List<FMOD.Studio.EventInstance> sounds;
    [FMODUnity.EventRef]
    public string CrashOneEvent;
    private FMOD.Studio.EventInstance crashOneSound;

    [FMODUnity.EventRef]
    public string CrashTwoEvent;
    private FMOD.Studio.EventInstance crashTwoSound;

    [FMODUnity.EventRef]
    public string CrashThreeEvent;
    private FMOD.Studio.EventInstance crashThreeSound;

    private int soundCount = 0;
    private Rigidbody cachedRigidBody;
    // Use this for initialization
    void Start()
    {
        sounds = new List<FMOD.Studio.EventInstance>();
        cachedRigidBody = HeliController.instance.GetComponentInParent<Rigidbody>();
        if (cachedRigidBody == null)
        {
            Debug.Log("Unable to get rigidbody from HeliController");
        }

        crashOneSound = FMODUnity.RuntimeManager.CreateInstance(CrashOneEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(crashOneSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
        sounds.Add(crashOneSound);
        crashTwoSound = FMODUnity.RuntimeManager.CreateInstance(CrashTwoEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(crashTwoSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
        sounds.Add(crashTwoSound);
        crashThreeSound = FMODUnity.RuntimeManager.CreateInstance(CrashThreeEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(crashThreeSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
        sounds.Add(crashThreeSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        HeliController tempHC = other.gameObject.GetComponentInChildren<HeliController>();
        if (tempHC != null)
        {
            FMOD.Studio.EventInstance randomSound = SoundPicker();
            randomSound.start();
        }
    }

    private FMOD.Studio.EventInstance SoundPicker()
    {
        if (soundCount >= sounds.Count)
        {
            soundCount = 0;
        }
        int tempCounter = soundCount;
        soundCount += 1;
        return sounds[tempCounter];
    }
}
