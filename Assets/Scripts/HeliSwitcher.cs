using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliSwitcher : MonoBehaviour {

    public HeloController[] copters;
    private int currentCopterIndex = 0;
	// Update is called once per frame
	
    void UpdateCurrentCopter(int indexDelta) {
        currentCopterIndex += indexDelta;
        if (currentCopterIndex >= copters.Length) {
            currentCopterIndex = 0;
        }
        if (currentCopterIndex < 0)
        {
            currentCopterIndex = copters.Length -1;
        }
        for (int i = 0; i < copters.Length; i++) {
            copters[i].gameObject.SetActive(i == currentCopterIndex);
            if (i == currentCopterIndex) {
                copters[i].TakeInstance();
            }
        }
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Period)) {
            UpdateCurrentCopter(1);
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            UpdateCurrentCopter(-1);
        }
	}
}
