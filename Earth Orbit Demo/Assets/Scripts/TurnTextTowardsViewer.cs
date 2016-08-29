using UnityEngine;
using System.Collections;

public class TurnTextTowardsViewer : MonoBehaviour
{
	void LateUpdate()
	{
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
