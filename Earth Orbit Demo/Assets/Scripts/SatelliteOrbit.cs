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

    public string TLEStringTopLine;
    public string TLEStringBottomLine;

    private Satellite sat;

    private ConfigManager configManager;

    float GetVernalEquinoxAngle()
    {
        Julian j = new Julian(configManager.GetCurrentDateTime());
        float vernalEquinoxAngle = Mathf.Rad2Deg * (float)j.ToGmst();

        return vernalEquinoxAngle;
    }

    void Start()
    {
        configManager = UnityEngine.Object.FindObjectOfType<ConfigManager>();

        Tle tle = new Tle("", TLEStringTopLine, TLEStringBottomLine);
        sat = new Satellite(tle);
    }

    void Update()
    {
        Eci eci = sat.PositionEci(configManager.GetCurrentDateTime());

        orbitingBody.transform.localPosition = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y) * orbitScaleFactor;

        float vernalEquinoxAngle = GetVernalEquinoxAngle();

        orbitingBody.RotateAround(centralBody.position, Vector3.up, vernalEquinoxAngle);

        // Correct local rotation
        orbitingBody.localRotation = Quaternion.identity;

        return;
    }
}
