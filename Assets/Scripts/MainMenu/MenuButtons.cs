using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public string sceneToLoad;
    public void ClickAction()
    {
        if (transform.parent.gameObject.name == "New Game") {
            ClearSavedMissionData();
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    void ClearSavedMissionData()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
        
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }
}
