using UnityEngine;
using System.Collections;

public class ISSLOD : MonoBehaviour
{

    public float levelOfDetailTransitionDistance;
    public GameObject highLODModel;
    public GameObject lowLODModel;

    private bool usingHighLOD;

	// Use this for initialization
	void Start()
    {
        usingHighLOD = false;

        highLODModel.SetActive(usingHighLOD);
        lowLODModel.SetActive(!usingHighLOD);
    }
	
	// Update is called once per frame
	void Update()
    {
        float cameraDistanceToISS = Vector3.Distance(Camera.main.transform.position, this.transform.position);

        usingHighLOD = (cameraDistanceToISS < levelOfDetailTransitionDistance);

        highLODModel.SetActive(usingHighLOD);
        lowLODModel.SetActive(!usingHighLOD);
    }

    
}
