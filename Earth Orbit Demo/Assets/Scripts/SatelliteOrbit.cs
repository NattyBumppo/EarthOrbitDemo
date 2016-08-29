using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zeptomoby.OrbitTools;

public class SatelliteOrbit : MonoBehaviour
{
    public Transform centralBody;
    public Transform orbitingBody;
    
    public float orbitScaleFactor;

    public bool simulateAtSuperSpeed;
    public float superSpeedMultiplier;

    public string TLEStringTopLine;
    public string TLEStringBottomLine;

    private Satellite sat;

    float GetVernalEquinoxAngle()
    {
        Julian j = new Julian(GetCurrentDateTime());
        float vernalEquinoxAngle = Mathf.Rad2Deg * (float)j.ToGmst();

        return vernalEquinoxAngle;
    }

    void Start()
    {
        Tle tle = new Tle("", TLEStringTopLine, TLEStringBottomLine);
        sat = new Satellite(tle);
    }

    void Update()
    {
        Eci eci = sat.PositionEci(GetCurrentDateTime());

        orbitingBody.transform.localPosition = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y) * orbitScaleFactor;

        float vernalEquinoxAngle = GetVernalEquinoxAngle();

        orbitingBody.RotateAround(centralBody.position, Vector3.up, vernalEquinoxAngle);

        // Correct local rotation
        orbitingBody.localRotation = Quaternion.identity;

        return;
    }

    DateTime GetCurrentDateTime()
    {
        if (simulateAtSuperSpeed)
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds(Time.realtimeSinceStartup * superSpeedMultiplier);
        }
        else
        {
            return DateTime.UtcNow;
        }
    }
}
