﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliSwitcher : MonoBehaviour
{

    public GameObject[] copters;
    private int currentCopterIndex = 0;
    public PauseMenuController PMC;

    void UpdateCurrentCopter(int indexDelta)
    {
        MusicPlayer.instance.StopMusic();
        currentCopterIndex += indexDelta;
        if (currentCopterIndex >= copters.Length)
        {
            currentCopterIndex = 0;
        }
        if (currentCopterIndex < 0)
        {
            currentCopterIndex = copters.Length - 1;
        }
        for (int i = 0; i < copters.Length; i++)
        {
            copters[i].SetActive(i == currentCopterIndex);
            if (i == currentCopterIndex)
            {
                HeliController chopper = copters[i].transform.GetComponentInChildren<HeliController>();
                chopper.TakeInstance();
                chopper.ReportToMainCamera();
                MusicPlayer.instance.StartMusic();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period))
        {
            UpdateCurrentCopter(1);
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            UpdateCurrentCopter(-1);
        }
    }
}
