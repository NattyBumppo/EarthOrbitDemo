using UnityEngine;
using System.Collections;

public class ScaleSize : MonoBehaviour
{
    public Transform mainObject;
    public Transform additionalLayer;
    public float scaleMultiplier;
    public float averageObjectRadiusKm;

    private float previousScaleMultiplier;

    void Start()
    {
        ScaleObject();
        previousScaleMultiplier = scaleMultiplier;
    }

    void Update()
    {
        if (previousScaleMultiplier != scaleMultiplier)
        {
            ScaleObject();

            previousScaleMultiplier = scaleMultiplier;
        }
    }

    private void ScaleObject()
    {
        float mainObjectCurrentRadius = mainObject.GetComponent<SphereCollider>().radius;

        float mainObjectScaleFactor = averageObjectRadiusKm * scaleMultiplier / mainObjectCurrentRadius;

        if (additionalLayer)
        {
            float cloudLayerCurrentRadius = additionalLayer.GetComponent<SphereCollider>().radius;

            float cloudLayerToMainObjectRadiusRatio = cloudLayerCurrentRadius / mainObjectCurrentRadius;
            float cloudLayerScaleFactor = averageObjectRadiusKm * cloudLayerToMainObjectRadiusRatio * scaleMultiplier / cloudLayerCurrentRadius;
            additionalLayer.localScale = new Vector3(cloudLayerScaleFactor, cloudLayerScaleFactor, cloudLayerScaleFactor);
        }

        mainObject.localScale = new Vector3(mainObjectScaleFactor, mainObjectScaleFactor, mainObjectScaleFactor);
    }

}
