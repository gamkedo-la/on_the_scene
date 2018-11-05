using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour {
    public static WorldState instance;

    private float DayTimer = 0;
    public bool isDay = true;
    public int CycleTime = 90;
    private string[] directions = {"North", "South", "East", "West", "NorthEast", "NorthWest", "SouthEast", "SouthWest" }; 
    public string windDirection;
    public int windSpeed = 0;
    public int maxWindSpeed = 0;
    public int minWindSpeed = 30;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        DayTimer += Time.deltaTime;
        //Debug.Log(isDay);
        //Debug.Log(DayTimer);
        //Debug.Log(windSpeed);
        //Debug.Log(windDirection);

        if (windDirection == null)
        {
            windDirection = directions[0];
        }

        if (DayTimer >= 90)
        {
            DayTimer = 0;

            isDay = !isDay;

            SetWindDirectionAndSpeed(directions[Random.Range(0, directions.Length - 1)], Random.Range(maxWindSpeed, minWindSpeed)); //first parameter is for direction, second parameter is for speed
        }
    }

    public void SetWindDirectionAndSpeed(string direction = "none", int speed = -1)
    {
        if (direction != "none")
        {
            windDirection = direction;
        }
        if (speed >= 0)
        {
            windSpeed = speed;
        }
    }
}
