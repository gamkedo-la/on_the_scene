using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{

    public GameObject arrow;
    public GameObject stopSign;
    public enum signState { None, StopSign, Arrow };
    public Transform nearestObjective;
    public bool nearestObjectiveActive;
    public signState currentState = signState.None;

    // Use this for initialization
    void Start()
    {
        SwitchIndicator(signState.None);
    }

    public void SwitchIndicator(signState state)
    {
        Debug.Log("Switch indicator new state: " + state);
        currentState = state;
        stopSign.SetActive(currentState == signState.StopSign);
        arrow.SetActive(currentState == signState.Arrow);
    }

    void Update()
    {
        if (MissionController.GetMissionObjectives() <= 0)
        {
            return;
        }
        nearestObjective = MissionController.GetNearestObjective().transform;
        nearestObjectiveActive = MissionController.GetNearestObjective().activeInHierarchy;
        if (nearestObjective != null && nearestObjectiveActive)
        {
            if (currentState == signState.None)
            {
                currentState = signState.Arrow;
            }
            transform.position = HeliController.instance.transform.position + Vector3.up * 2.0f;
            return;
        }
        currentState = signState.None;
    }
}
