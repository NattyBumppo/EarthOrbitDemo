using UnityEngine;
using System.Collections;

public class TurnTextTowardsViewer : MonoBehaviour
{
	void Update()
	{
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
