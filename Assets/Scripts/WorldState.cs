using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState instance;

    public Light sunLight;
    public Material nightSkybox;
    public Material daySkybox;
    public List<Transform> transformsToCycle = new List<Transform>();

    private float DayTimer = 0;
    public bool isDay = true;
    public int CycleTime = 90;
    private string[] directions = { "North", "South", "East", "West", "NorthEast", "NorthWest", "SouthEast", "SouthWest" };
    public string windDirection;
    public int windSpeed = 0;
    public int maxWindSpeed = 0;
    public int minWindSpeed = 30;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SetVariables();
        ApplyNightOrDay();
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

            ToggleNightOrDay();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleNightOrDay();
        }
    }

    private void ApplyNightOrDay()
    {
        //first parameter is for direction, second parameter is for speed
        SetWindDirectionAndSpeed(directions[Random.Range(0, directions.Length - 1)], Random.Range(maxWindSpeed, minWindSpeed));

        sunLight.enabled = isDay;

        RenderSettings.skybox = (isDay ? daySkybox : nightSkybox);
        RenderSettings.fog = !isDay;

        foreach (var item in transformsToCycle)
        {
            item.BroadcastMessage("Cycle", isDay, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ToggleNightOrDay()
    {
        isDay = !isDay;
        ApplyNightOrDay();
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

    public void SetVariables()// is used for initializing the materials, textures and lights
    {
        sunLight = GameObject.FindGameObjectWithTag("SunLight").GetComponent<Light>();
        GameObject tempBuildings = GameObject.FindGameObjectWithTag("Buildings");
        for (int i = 0; i < tempBuildings.transform.childCount; i++)
        {
            transformsToCycle.Add(tempBuildings.transform.GetChild(i));
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("StreetLamp"))
        {
            transformsToCycle.Add(item.transform);
        }
    }
}
