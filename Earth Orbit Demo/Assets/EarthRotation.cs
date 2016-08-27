using UnityEngine;
using System;
using System.Collections;

public class EarthRotation : MonoBehaviour
{
    // Obtained by 360 / 86,400
    private const double ROTATIONAL_VELOCITY_DEG_PER_SEC = 0.00416666667;
    
    public double rotationSpeedMultiplier;

    public Transform cloudLayer;
    public double cloudLayerRotationDegPerSec;

    private double rotationDegrees = 0.0;
    private double cloudLayerRotationDegrees = 0.0f;

    void Start()
	{
        transform.Rotate(Vector3.up, (float)rotationDegrees);
        cloudLayer.Rotate(Vector3.up, (float)cloudLayerRotationDegrees);
	}
	
	void Update()
	{
        rotationDegrees -= Time.deltaTime * ROTATIONAL_VELOCITY_DEG_PER_SEC * rotationSpeedMultiplier;
        transform.rotation = Quaternion.AngleAxis((float)rotationDegrees, Vector3.up);

        cloudLayerRotationDegrees -= Time.deltaTime * cloudLayerRotationDegPerSec * rotationSpeedMultiplier;
        cloudLayer.rotation = Quaternion.AngleAxis((float)cloudLayerRotationDegrees, Vector3.up);
    }
}
