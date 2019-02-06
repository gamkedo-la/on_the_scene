using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

    public string mainMenuScene;
    public GameObject pauseMenu;
    public GameObject soundMenu;
    public GameObject toPauseText;
    public bool isPaused;

	// Use this for initialization
	void Start () {
        isPaused = false;
        pauseMenu.SetActive(false);
        soundMenu.SetActive(false);
        toPauseText.SetActive(true);
        Time.timeScale = 1f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
                return;
            }
            isPaused = true;
            pauseMenu.SetActive(true);
            toPauseText.SetActive(false);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        soundMenu.SetActive(false);
        toPauseText.SetActive(true);
        Time.timeScale = 1f;
    }

    public void SoundMenu()
    {
        pauseMenu.SetActive(false);
        soundMenu.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        pauseMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(mainMenuScene);
        Start();
    }
}
