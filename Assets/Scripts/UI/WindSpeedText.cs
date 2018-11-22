using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpeedText : MonoBehaviour {

    // Use this for initialization
    private UnityEngine.UI.Text textComponent;

    void Start () {
        textComponent = GetComponent<UnityEngine.UI.Text>();

    }
	
	// Update is called once per frame
	void Update () {
        textComponent.text = WorldState.instance.windSpeed.ToString();
    }
}
