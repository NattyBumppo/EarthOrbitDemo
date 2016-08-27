using System;
using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
    public Transform centralBody;
    public Transform orbitingBody;

    // Value below taken from http://nssdc.gsfc.nasa.gov/planetary/factsheet/earthfact.html
    private const double EARTH_RADIUS = 6378.137; // Equatorial in km; polar is 6356.752 km

    // Values below taken from http://heavens-above.com/orbit.aspx?satid=25544
    private const double LAMBDA = 3.986004418 * (double)(10^14); // Lambda: standard gravitational parameter, in kg
    private const double ISSMASS = 419600; // kg
    private const double ISS_ORBITAL_ECCENTRICITY = 0.0003240;
    private const double ISS_INCLINATION = 51.6419;
    private const double ISS_PERIGEE_KM = 402;
    private const double ISS_APOGEE_KM = 406;
    private const double RAAN = 62.2683;
    private const double ARG_PERIGEE = 261.0262;
    private const double ISS_ORBITAL_PERIOD = 5558.58767; // s
    private const double MEAN_ANOMALY_AT_EPOCH = 99.0524;
    private const double ORBIT_NUMBER_AT_EPOCH = 1607;
    private DateTime EPOCH_DATETIME = new DateTime(2016, 8, 27, 11, 36, 31, DateTimeKind.Utc);

    // This is approximate; incorrect assumption of equatorial orbit
    private double orbitalSemimajorAxis = ISS_PERIGEE_KM + EARTH_RADIUS * 2.0 + ISS_APOGEE_KM;


    void Start()
    {
	    // Position orbiting body
	}
	
	void Update()
    {
	
	}

    double GetMeanAnomaly(double meanMotion, double timeSincePerihelion)
    {
        return meanMotion * timeSincePerihelion;
    }

    double GetMeanMotion(double period)
    {
        return ((2.0 * Math.PI) / period);
    }

    //double GetEccentricAnomaly(double meanAnomaly, double eccentricity)
    //{
    //    // Solve meanAnomaly = eccentricAnomaly - eccentricity * sin (eccentricAnomaly) numerically or with tables...
    //}

    //double GetTrueAnomaly()
    //{

    //}

    double GetHeliocentricDistance(double semimajorAxis, double eccentricity, double GetEccentricAnomaly)
    {
        return semimajorAxis * (1.0 - eccentricity * Math.Cos(GetEccentricAnomaly));
    }
}
