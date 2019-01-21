using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{

    public static DebugUI instance;
    private Text debugOutput;
    // Use this for initialization

    private void Awake()
    {
        instance = this;
        debugOutput = transform.GetComponentInChildren<Text>();
    }

    public void SetDebugText(string textToSet)
    {
        debugOutput.text = textToSet;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
