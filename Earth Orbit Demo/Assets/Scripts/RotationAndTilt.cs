using UnityEngine;
using System;
using System.Collections;

public class RotationAndTilt : MonoBehaviour
{
    // From http://www.universetoday.com/47176/earths-axis/
    private const double EARTH_TILT_DEG = 23.439281;

    private const int SECONDS_PER_DAY = 86400;
    private const double SECONDS_PER_YEAR = 365.25 * 24.0 * 3600.0;

    //public double rotationSpeedMultiplier;

    public Transform cloudLayer;
    //public double cloudLayerRotationDegPerSec;

    //private double rotationDegrees = 0.0;
    //private double cloudLayerRotationDegrees = 0.0f;

    // Rotates the provided transform to match Earth's rotation in UTC time
    private void RotateEarthToMatchUtcTime(Transform t, DateTime utcTime)
    {
        // The provided Earth model is oriented so that the +Y axis points to the
        // north pole, and the +Z axis points towards Africa. Hence, the Z-Y plane
        // defines the prime meridian/antimeridian, while the Z-X plane defines
        // the equator. We'll use (UTC noon == sunlight centered on the prime meridian)
        // to rotate the Earth to match the provided time.
        double secondsSinceMidnight = utcTime.TimeOfDay.TotalSeconds;
        //Debug.Log("secondsSinceMidnight: " + secondsSinceMidnight.ToString());
        double secondsSinceNoon = secondsSinceMidnight - SECONDS_PER_DAY / 2.0;
        if (secondsSinceNoon < 0.0)
        {
            secondsSinceNoon += SECONDS_PER_DAY;
        }
        //Debug.Log("secondsSinceNoon: " + secondsSinceNoon.ToString());

        double rotationAngleDeg = -360.0 * (secondsSinceNoon / SECONDS_PER_DAY);

        //Debug.Log("rotationAngleDeg: " + rotationAngleDeg.ToString());

        //rotationAngleDeg = 0.0;

        t.localRotation = Quaternion.AngleAxis((float)rotationAngleDeg, Vector3.up);
    }


    void Start()
	{
        //transform.Rotate(Vector3.up, (float)rotationDegrees);
        //cloudLayer.Rotate(Vector3.up, (float)cloudLayerRotationDegrees);
	}
	
	void Update()
	{
        RotateEarthToMatchUtcTime(this.transform, DateTime.UtcNow);
        RotateEarthToMatchUtcTime(cloudLayer, DateTime.UtcNow);
        RotateToEffectTilt();
    }

    DateTime GetCurrentDateTime()
    {
        return DateTime.UtcNow;
    }

    private void RotateToEffectTilt()
    {
        // This is super-approximate...
        double secondsSinceVernalEquinox = (GetCurrentDateTime() - new DateTime(GetCurrentDateTime().Year, 3, 20)).TotalSeconds;
        if (secondsSinceVernalEquinox < 0)
        {
            secondsSinceVernalEquinox += SECONDS_PER_YEAR;
        }

        double percentageOfYearSinceVernalEquinoxDeg = (secondsSinceVernalEquinox / SECONDS_PER_YEAR);




    }
}
