using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliCrashSounds : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string CrashOneEvent;
    private FMOD.Studio.EventInstance crashOneSound;

    [FMODUnity.EventRef]
    public string CrashTwoEvent;
    private FMOD.Studio.EventInstance crashTwoSound;

    [FMODUnity.EventRef]
    public string CrashThreeEvent;
    private FMOD.Studio.EventInstance crashThreeSound;

    private Rigidbody cachedRigidBody;
    // Use this for initialization
    void Start()
    {
        cachedRigidBody = HeliController.instance.GetComponentInParent<Rigidbody>();
        if (cachedRigidBody == null)
        {
            Debug.Log("Unable to get rigidbody from HeliController");
        }

        crashOneSound = FMODUnity.RuntimeManager.CreateInstance(CrashOneEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(crashOneSound, GetComponent<Transform>(), GetComponent<Rigidbody>());

        crashTwoSound = FMODUnity.RuntimeManager.CreateInstance(CrashTwoEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(crashTwoSound, GetComponent<Transform>(), GetComponent<Rigidbody>());

        crashThreeSound = FMODUnity.RuntimeManager.CreateInstance(CrashThreeEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(crashThreeSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    private void OnTriggerEnter(Collider other)
    {
        HeliController tempHC = other.gameObject.GetComponentInChildren<HeliController>();
        if (tempHC != null)
        {
            Debug.Log("heli entered meeee");
            crashOneSound.start();
        }
    }
}
