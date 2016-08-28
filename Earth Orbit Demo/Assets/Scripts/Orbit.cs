using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zeptomoby.OrbitTools;

public enum OrbitalSystem
{
    EARTH_ISS,
    EARTH_MOON,
    SUN_EARTH
}

public class Orbit : MonoBehaviour
{
    public Transform centralBody;
    public Transform orbitingBody;

    // Values below can be taken from http://heavens-above.com/orbit.aspx?satid=25544. Periapsis and apoapsis for Earth
    // can be estimated using EARTH_RADIUS = (6378.137 + 6356.752) / 2.0.
    public double eccentricity;
    public double inclination;
    public double periapsisKm;
    public double apoapsisKm;
    public double raanDeg;
    public double argPeriapsisDeg;
    public double periodSec;
    public double meanAnomalyAtEpoch;
    public double orbitNumberAtEpoch;
    public int epochYear;
    public int epochMonth;
    public int epochDay;
    public int epochHours;
    public int epochMinutes;
    public int epochSeconds;
    public OrbitalSystem orbitalSystem;

    private const double SECONDS_PER_YEAR = 365.25 * 24.0 * 3600.0;

    public float orbitScaleFactor;

    private DateTime epochDatetime;

    private double orbitalSemimajorAxis;

    public bool simulateAtSuperSpeed;

    public bool fixTimeAtEpoch;

    void Start()
    {
        orbitalSemimajorAxis = (periapsisKm + apoapsisKm) / 2.0;
        epochDatetime = new DateTime(epochYear, epochMonth, epochDay, epochHours, epochMinutes, epochSeconds, DateTimeKind.Utc);
    }

    float GetVernalEquinoxAngle()
    {
        Julian j = new Julian(DateTime.UtcNow);
        float vernalEquinoxAngle = Mathf.Rad2Deg * (float)j.ToGmst();

        return vernalEquinoxAngle;
    }

    Vector3 GetOrbitalPlaneRotationalAxis()
    {
        // The orbital plane is inclined on an axis that is described by the RAAN, which is an angular
        // offset from the vector to the vernal equinox (a.k.a. the First Point of Aries).
        // First, we have to find the angle of the vernal equinox in world space...

        // This is super-approximate...
        double secondsSinceVernalEquinox = (GetCurrentDateTime() - new DateTime(GetCurrentDateTime().Year, 3, 20)).TotalSeconds;
        if (secondsSinceVernalEquinox < 0)
        {
            secondsSinceVernalEquinox += SECONDS_PER_YEAR;
        }

        double angleTraveledThroughOrbitSinceVernalEquinoxDeg = 360.0 * (secondsSinceVernalEquinox / SECONDS_PER_YEAR);

        //Debug.Log(angleTraveledThroughOrbitSinceVernalEquinoxDeg);

        // Now, use this angle as an offset to the RAAN to calculate the orbital plane rotation
        double totalAngle = -angleTraveledThroughOrbitSinceVernalEquinoxDeg + raanDeg;
        //double totalAngle = raanDeg;
        Vector3 orbitalPlaneRotationalAxis = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (float)totalAngle), 0.0f, Mathf.Sin(Mathf.Deg2Rad * (float)totalAngle));

        return orbitalPlaneRotationalAxis;
    }



    void Update()
    {

        // Orbit tools test
        string str1 = "ISS test";
        string str2 = "1 25544U 98067A   16241.14655215  .00003187  00000-0  54953-4 0  9992";
        string str3 = "2 25544  51.6453  58.9571 0002981 250.0016 221.6665 15.54339053 16187";

        Tle issTle = new Tle(str1, str2, str3);
        Satellite sat = new Satellite(issTle);
        Eci eci = sat.PositionEci(GetCurrentDateTime());
        //Debug.Log("(" + eci.Position.X + ", " + eci.Position.Y + ", " + eci.Position.Z + ")");
        

        //Debug.DrawLine(new Vector3(0f, 0f, 0f), GetOrbitalPlaneRotationalAxis().normalized * 500.0f, Color.red);

        orbitingBody.transform.position = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y) * orbitScaleFactor;

        float vernalEquinoxAngle = GetVernalEquinoxAngle();

        orbitingBody.RotateAround(centralBody.position, Vector3.up, vernalEquinoxAngle);

        // Correct local rotation
        orbitingBody.localRotation = Quaternion.identity;

        return;
        // Test
        //Debug.Log("Earth radius: " + (centralBody.GetComponent<Renderer>().bounds.extents.magnitude / 2.0f).ToString());

        // Get true anomaly and orbital radius to position in orbit
        double meanAnomaly = GetMeanAnomaly(GetMeanMotion(), GetTimeSincePeriapsis());
        //Debug.Log("meanAnomaly: " + meanAnomaly.ToString());
        double eccentricAnomaly = GetEccentricAnomaly(meanAnomaly);
        //Debug.Log("eccentricAnomaly: " + eccentricAnomaly.ToString());
        double trueAnomaly = GetTrueAnomaly(eccentricAnomaly);
        //Debug.Log("trueAnomaly: " + (trueAnomaly * Mathf.Rad2Deg).ToString());

        double currentRadius = orbitScaleFactor * GetRadiusInOrbit(trueAnomaly);
        //Debug.Log("Radius in orbit: " + currentRadius);

        MoveToPositionInOrbit(trueAnomaly, currentRadius);
        //Debug.Log("Position: " + orbitingBody.position);
    }

    DateTime GetCurrentDateTime()
    {
        if (fixTimeAtEpoch)
        {
            return epochDatetime;
        }
        else if (simulateAtSuperSpeed)
        {
            return DateTime.UtcNow + TimeSpan.FromSeconds(Time.realtimeSinceStartup * 500.0);
        }
        else
        {
            return DateTime.UtcNow;
        }
    }

    double GetTimeSincePeriapsis()
    {

        double timeSincePeriapsisAtEpoch = meanAnomalyAtEpoch / GetMeanMotion();

        TimeSpan elapsedTime;

        elapsedTime = GetCurrentDateTime() - epochDatetime;


        //Debug.Log("Time since peri at epoch: " + timeSincePeriapsisAtEpoch.ToString());

        //

        return elapsedTime.TotalSeconds + timeSincePeriapsisAtEpoch;
    }

    double GetMeanAnomaly(double meanMotion, double timeSincePeriapsis)
    {
        //Debug.Log("Mean motion: " + meanMotion.ToString());
        //Debug.Log("Time since periapsis: " + timeSincePeriapsis.ToString());

        double uncorrectedMeanAnomaly = meanMotion * timeSincePeriapsis;

        // Return a version clamped to [0, 2pi]
        return uncorrectedMeanAnomaly % (2.0 * Math.PI);
    }

    double GetMeanMotion()
    {
        return ((2.0 * Math.PI) / this.periodSec);
    }

    double GetEccentricAnomaly(double meanAnomaly)
    {
        // Use table to solve meanAnomaly = eccentricAnomaly - eccentricity * sin (eccentricAnomaly)
        double eccentric_anomaly = 0.0;
        switch (orbitalSystem)
        {
            case OrbitalSystem.EARTH_ISS:
                eccentric_anomaly = LookUpEccentricAnomaly(meanAnomaly, LookUpTables.eccentricAnomaliesEarthISS);
                break;
            case OrbitalSystem.EARTH_MOON:

                break;
            case OrbitalSystem.SUN_EARTH:

                break;
            default:

                break;
        }

        return eccentric_anomaly;
    }

    double GetTrueAnomaly(double eccentricAnomaly)
    {
        double argX = Math.Sqrt(1.0 - eccentricity) * Math.Cos(eccentricAnomaly / 2.0);
        double argY = Math.Sqrt(1.0 + eccentricity) * Math.Sin(eccentricAnomaly / 2.0);
        return 2.0 * Math.Atan2(argY, argX);
    }

    double LookUpEccentricAnomaly(double meanAnomaly, List<double> eccentricAnomalyList)
    {
        // meanAnomaly should range from 0 to 2*pi
        if ((meanAnomaly > 0.0) && (meanAnomaly < 2.0 * Math.PI))
        {
            // Convert meanAnomaly to lookup table (float) index
            int numSteps = eccentricAnomalyList.Count;
            float stepSize = (2 * Mathf.PI) / (float)numSteps;
            float floatIndex = (float)meanAnomaly / stepSize;

            // Use floatIndex to lerp in eccentric anomaly space
            int bottomIndex = Mathf.FloorToInt(floatIndex);
            int topIndex = (int)Mathf.Clamp(Mathf.Ceil(floatIndex), 0.0f, (float)eccentricAnomalyList.Count - 1.0f);

            return Mathf.Lerp((float)eccentricAnomalyList[bottomIndex], (float)eccentricAnomalyList[topIndex], floatIndex - (float)bottomIndex);
        }
        else
        {
            //Debug.LogError("Error: mean anomaly of " + meanAnomaly.ToString() + " is invalid!");
            return 0.0;
        }
    }

    double GetRadiusInOrbit(double trueAnomaly)
    {
        double numerator = 1.0 - this.eccentricity * this.eccentricity;
        double denominator = 1 + this.eccentricity * trueAnomaly;
        return orbitalSemimajorAxis * (numerator / denominator);
    }

    void MoveToPositionInOrbit(double trueAnomaly, double currentRadius)
    {
        // Angle in orbit will be argument of periapsis + true anomaly, corrected by the RAAN
        //double angleInOrbit = Mathf.Deg2Rad * argPeriapsisDeg + trueAnomaly + Mathf.Deg2Rad * raanDeg;

        // Angle in orbit will be argument of periapsis + true anomaly
        double angleInOrbit = Mathf.Deg2Rad * argPeriapsisDeg + trueAnomaly;

        //Debug.Log("angle: " + angleInOrbit.ToString());

        // Determine a position at this point in orbit
        Vector3 eclipticOrbitalPosition = new Vector3((float)currentRadius * (float)Math.Cos(angleInOrbit), 0.0f, (float)currentRadius * (float)Math.Sin(angleInOrbit));

        //Debug.Log(eclipticOrbitalPosition);

        orbitingBody.localPosition = eclipticOrbitalPosition;

        // Now, rotate the position around the axis of the ascending node by the inclination
        //Vector3 centralBodyVector = transform.position - centralBody.position;

        orbitingBody.RotateAround(centralBody.position, GetOrbitalPlaneRotationalAxis(), (float)inclination);

        // Correct local rotation
        orbitingBody.localRotation = Quaternion.identity;

        Debug.Log("Position in orbit: " + orbitingBody.position);

        //Debug.DrawLine(new Vector3(0f, 0f, 0f), orbitingBody.position, Color.green);
    }
}
