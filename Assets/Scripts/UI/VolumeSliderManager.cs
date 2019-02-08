using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderManager : MonoBehaviour {

    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private Slider chopperSfxVolumeSlider;
    private Slider notChopperSfxVolumeSlider;
	
	// Use this for initialization
	void Awake () {

		Debug.Log("Made it");
        masterVolumeSlider = GameObject.Find("SoundSlider").GetComponent<Slider>();
        musicVolumeSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        sfxVolumeSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();
        chopperSfxVolumeSlider = GameObject.Find("ChopperSFXSlider").GetComponent<Slider>();
        notChopperSfxVolumeSlider = GameObject.Find("NotChopperSFXSlider").GetComponent<Slider>();

		SetSliderValues();
	}

	void SetSliderValues() {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
        float chopperSfxVolume = PlayerPrefs.GetFloat("chopperSfxVolume", 0.05f);
        float notChopperSfxVolume = PlayerPrefs.GetFloat("notChopperSfxVolume", 1f);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
        chopperSfxVolumeSlider.value = chopperSfxVolume;
        notChopperSfxVolumeSlider.value = notChopperSfxVolume;

        Debug.Log("Done");
	}
}
