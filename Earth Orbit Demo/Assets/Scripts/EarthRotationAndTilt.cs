using UnityEngine;
using System;
using System.Collections;

public class EarthRotationAndTilt : MonoBehaviour
{
    // From http://www.universetoday.com/47176/earths-axis/
    private const double EARTH_TILT_DEG = 23.439281;

    private const int SECONDS_PER_DAY = 86400;
    private const double SECONDS_PER_YEAR = 365.25 * 24.0 * 3600.0;
    public float correctionAngle;

    public Transform cloudLayer;

    public Light sunDirectionalLight;

    public bool maintainCorrectTilt;

    private Quaternion initialRotation;

    private ConfigManager configManager;

    void Start()
    {
        // Save initial rotation
        initialRotation = sunDirectionalLight.transform.rotation;

        configManager = UnityEngine.Object.FindObjectOfType<ConfigManager>();
    }

	void Update()
	{
        RotateEarthToMatchUtcTime(this.transform, configManager.GetCurrentDateTime());

        if (maintainCorrectTilt)
        {
            RotateToEffectTilt();
        }
        
    }

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

        double rotationAngleDeg = -360.0 * (secondsSinceNoon / SECONDS_PER_DAY);

        t.localRotation = Quaternion.AngleAxis((float)rotationAngleDeg + correctionAngle, Vector3.up);
    }

    private void RotateToEffectTilt()
    {
        // This is super-approximate...
        double secondsSinceVernalEquinox = (configManager.GetCurrentDateTime() - new DateTime(configManager.GetCurrentDateTime().Year, 3, 20)).TotalSeconds;
        if (secondsSinceVernalEquinox < 0)
        {
            secondsSinceVernalEquinox += SECONDS_PER_YEAR;
        }

        double percentageOfYearSinceVernalEquinoxDeg = (secondsSinceVernalEquinox / SECONDS_PER_YEAR);

        // At vernal equinox, Earth's tilt vector lines up with the Earth-Sun vector. A quarter of a year
        // later, the two vectors are perpindicular. We can use the time after vernal equinox to determine
        // the axial tilt vector with respect to the Earth-Sun vector.
        Vector3 earthSunVector = -sunDirectionalLight.transform.forward.normalized;

        double rotationAmount = percentageOfYearSinceVernalEquinoxDeg * 360.0;

        Vector3 tiltVector = Quaternion.AngleAxis((float)rotationAmount, Vector3.up) * earthSunVector;

        // Reset to initial rotation, then apply calculated rotation
        sunDirectionalLight.transform.rotation = initialRotation;
        sunDirectionalLight.transform.RotateAround(transform.position, -tiltVector, (float)EARTH_TILT_DEG);
    }
}
