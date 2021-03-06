﻿using UnityEngine;
using System.Collections;

public class RotateTowardsSun : MonoBehaviour
{
    public Light sunLight;

	void Start()
	{
        RotateTowardsSunDirection();
    }
	
	void LateUpdate()
	{
        RotateTowardsSunDirection();
    }

    public void RotateTowardsSunDirection()
    {
        if (sunLight.type == LightType.Directional)
        {
            transform.rotation = Quaternion.LookRotation(-sunLight.transform.forward);
        }
        else if (sunLight.type == LightType.Point)
        {
            transform.LookAt(sunLight.transform);
        }
    }
}
