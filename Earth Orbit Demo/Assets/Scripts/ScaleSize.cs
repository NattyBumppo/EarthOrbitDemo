﻿using UnityEngine;
using System.Collections;

public class ScaleSize : MonoBehaviour
{
    public Transform mainObject;
    public Transform additionalLayer;
    public float scaleMultiplier;
    public float averageObjectRadiusKm;

    public bool forceOriginalScale;

    private float previousScaleMultiplier;

    void Start()
    {
        if (forceOriginalScale)
        {
            // Don't affect current scale!
        }
        else
        {
            ScaleObject();
            previousScaleMultiplier = scaleMultiplier;
        }

    }

    void Update()
    {
        if (forceOriginalScale)
        {
            // Don't affect current scale!
        }
        else
        {
            if (previousScaleMultiplier != scaleMultiplier)
            {
                ScaleObject();

                previousScaleMultiplier = scaleMultiplier;
            }
        }
    }

    private void ScaleObject()
    {
        float mainObjectCurrentRadius = mainObject.GetComponent<SphereCollider>().radius;

        float mainObjectScaleFactor = averageObjectRadiusKm * scaleMultiplier / mainObjectCurrentRadius;

        if (additionalLayer)
        {
            float additionalLayerCurrentRadius = additionalLayer.GetComponent<SphereCollider>().radius;

            float additionalLayerToMainObjectRadiusRatio = additionalLayerCurrentRadius / mainObjectCurrentRadius;
            float additionalLayerScaleFactor = averageObjectRadiusKm * additionalLayerToMainObjectRadiusRatio * scaleMultiplier / additionalLayerCurrentRadius;
            additionalLayer.localScale = new Vector3(additionalLayerScaleFactor, additionalLayerScaleFactor, additionalLayerScaleFactor);
        }

        mainObject.localScale = new Vector3(mainObjectScaleFactor, mainObjectScaleFactor, mainObjectScaleFactor);
    }

}
