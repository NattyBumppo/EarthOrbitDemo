using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabilize : MonoBehaviour {

    public Transform earthSphere;
    private SphereCollider earthCollider;
    private Vector3 stabilizationPlaneNormal;
    private Camera mainCamera;
    private Transform mainCameraTransform;
    private Plane[] planes;
    private Vector3 focusPosition;

    void Start()
    {
        earthCollider = earthSphere.GetComponent<SphereCollider>();
        mainCamera = Camera.main;
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Set stabilization plane 2m ahead of user if they can't see the Earth;
        // set it to the camera intersection with the Earth if they can
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        if (GeometryUtility.TestPlanesAABB(planes, earthCollider.bounds))
        {
            focusPosition = earthSphere.position;
        }
        else
        {
            focusPosition = mainCameraTransform.position + 2.0f * mainCameraTransform.forward;
        }

        stabilizationPlaneNormal = -mainCameraTransform.forward;
        UnityEngine.VR.WSA.HolographicSettings.SetFocusPointForFrame(focusPosition, stabilizationPlaneNormal);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(focusPosition, 0.1f);
    //}
}
