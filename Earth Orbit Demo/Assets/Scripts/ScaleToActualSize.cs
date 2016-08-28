using UnityEngine;
using System.Collections;

public class ScaleToActualSize : MonoBehaviour
{
    public Transform mainObject;
    public Transform cloudLayer;
    public float scaleMultiplier;
    public float averageObjectRadiusKm;

    void Start()
    {
        float mainObjectCurrentRadius = mainObject.GetComponent<SphereCollider>().radius;
        float cloudLayerCurrentRadius = cloudLayer.GetComponent<SphereCollider>().radius;

        float cloudLayerToMainObjectRadiusRatio = cloudLayerCurrentRadius / mainObjectCurrentRadius;

        float mainObjectScaleFactor = averageObjectRadiusKm * scaleMultiplier / mainObjectCurrentRadius;
        float cloudLayerScaleFactor = averageObjectRadiusKm * cloudLayerToMainObjectRadiusRatio * scaleMultiplier / cloudLayerCurrentRadius;

        mainObject.localScale = new Vector3(mainObjectScaleFactor, mainObjectScaleFactor, mainObjectScaleFactor);
        cloudLayer.localScale = new Vector3(cloudLayerScaleFactor, cloudLayerScaleFactor, cloudLayerScaleFactor);
    }

}
