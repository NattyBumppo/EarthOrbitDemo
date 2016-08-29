using UnityEngine;
using System.Collections;

public class StartCameraOnLightSideOfEarth : MonoBehaviour
{
    public Light sunLight;
    public Transform centralBody;

    void Start()
    {
        float currentDistance = Vector3.Distance(transform.position, centralBody.position);

        if (sunLight.type == LightType.Directional)
        {
            // Put the "Sun" at our backs
            transform.position = centralBody.position + -sunLight.transform.forward * currentDistance;

            // Look towards the central body
            transform.LookAt(centralBody, Vector3.up);
        }
        else if (sunLight.type == LightType.Point)
        {
            // Put the "Sun" at our backs
            transform.position = centralBody.position + currentDistance * (sunLight.transform.position - centralBody.position).normalized;

            // Look towards the central body
            transform.LookAt(centralBody, Vector3.up);
        }
    }
}
