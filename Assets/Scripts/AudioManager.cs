using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    //float db = linear > 0 ? 20.0f * Mathf.Log10(linear * Mathf.Sqrt(2.0f)) : -80.0f;

    string masterBusString = "Bus:/";
    FMOD.Studio.Bus masterBus;


    // Use this for initialization
    void Start () {
        masterBus = FMODUnity.RuntimeManager.GetBus(masterBusString);

    }

    public void SetVolume(Slider volume) {
        masterBus.setVolume(volume.value);
    }
	
	// Update is called once per frame
	void Update () {

	}
}
