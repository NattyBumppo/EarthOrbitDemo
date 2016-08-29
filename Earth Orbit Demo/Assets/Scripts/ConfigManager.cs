using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConfigManager : MonoBehaviour
{
    public Text dateTimeLabel;

    public bool simulateAtSuperSpeed;
    public float superSpeedMultiplier;
    public bool realtimeSimulation;
    public bool drawSatelliteTrails;
    public Material satelliteTrailmaterial;
    public Transform cloudLayer;

    public int traceOrbitNumFramesPerLineNormal;
    public int traceOrbitNumStoredPositionsNormal;
    public int traceOrbitNumFramesPerLineSuperSpeed;
    public int traceOrbitNumStoredPositionsSuperSpeed;
    
    public List<Transform> satellites;

    const string dateFormatString = "M/dd/yy H:mm:ss";

    private DateTime timeSuperSpeedStarted;

    void Start()
	{
        timeSuperSpeedStarted = DateTime.UtcNow;
    }
	
	void Update()
	{
        SetDateTimeLabel();
    }

    public int GetTraceOrbitNumFramesPerLine()
    {
        if (simulateAtSuperSpeed)
        {
            return traceOrbitNumFramesPerLineSuperSpeed;
        }
        else
        {
            return traceOrbitNumFramesPerLineNormal;
        }
    }

    public int GetTraceOrbitNumStoredPositions()
    {
        if (simulateAtSuperSpeed)
        {
            return traceOrbitNumStoredPositionsSuperSpeed;
        }
        else
        {
            return traceOrbitNumStoredPositionsNormal;
        }
    }

    public void OnToggleSuperSpeed(bool newValue)
    {
        ClearSatelliteTrails();
        simulateAtSuperSpeed = newValue;

        if (simulateAtSuperSpeed)
        {
            timeSuperSpeedStarted = DateTime.UtcNow;
        }
    }

    public void OnToggleRealtime(bool newValue)
    {
        realtimeSimulation = newValue;
    }

    public void OnToggleSatelliteTrails(bool newValue)
    {
        drawSatelliteTrails = newValue;
        ClearSatelliteTrails();
    }

    public void OnToggleCloudLayer(bool newValue)
    {
        cloudLayer.gameObject.SetActive(newValue);
    }

    private void SetDateTimeLabel()
    {
        DateTime currentDateTime = GetCurrentDateTime();
        dateTimeLabel.text = "Sim Time: " + currentDateTime.ToString(dateFormatString) + " (UTC)";
    }

    private void ClearSatelliteTrails()
    {
        foreach (Transform t in satellites)
        {
            t.GetComponent<TraceLineTrail>().EraseLine();
         }
    }

    public DateTime GetCurrentDateTime()
    {
        if (simulateAtSuperSpeed)
        {
            TimeSpan elapsedTimeSinceSuperSpeedStarted = DateTime.UtcNow - timeSuperSpeedStarted;
            // Scale by the multiplier
            DateTime scaledTime = DateTime.UtcNow.AddSeconds((DateTime.UtcNow - timeSuperSpeedStarted).TotalSeconds * superSpeedMultiplier);
            return scaledTime;
        }
        else
        {
            return DateTime.UtcNow;
        }
    }
}
