using UnityEngine;
using System.Collections;

public class SetRunInBackground : MonoBehaviour
{
    public bool runInBackground;

	void Start()
	{
        Application.runInBackground = runInBackground;
    }
	
	void Update()
	{
        if (Application.runInBackground != runInBackground)
        {
            Application.runInBackground = runInBackground;
        }
        
    }
}
