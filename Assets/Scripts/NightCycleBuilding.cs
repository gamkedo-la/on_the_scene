using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightCycleBuilding : MonoBehaviour
{
    public Texture dayTexture, nightTexture;

    public void Cycle(bool isDay) //Day and night cycle
    {
        //transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", (isDay ? dayTexture : nightTexture));
        foreach (var item in transform.GetComponent<MeshRenderer>().materials)
        {
            if (item.mainTexture.name == dayTexture.name && !isDay)
            {
                item.SetTexture("_MainTex", (nightTexture));
            }
            else if(item.mainTexture.name == nightTexture.name && isDay)
            {
                item.SetTexture("_MainTex", (dayTexture));
            }
        }
        
        
    }
}
