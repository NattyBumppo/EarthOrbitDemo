using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TraceLineTrail : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform objectToTrack;

    private int currentFrameCount;
    private List<Vector3> positions;

    private ConfigManager configManager;

	void Start()
    {
        currentFrameCount = 0;
        positions = new List<Vector3>();

        configManager = UnityEngine.Object.FindObjectOfType<ConfigManager>();

        lineRenderer.material = configManager.satelliteTrailmaterial;
    }
	
	void LateUpdate()
    {
        if (configManager.drawSatelliteTrails)
        {
            lineRenderer.material = configManager.satelliteTrailmaterial;

            // We probably don't want to sample/draw every frame
            if (currentFrameCount % configManager.GetTraceOrbitNumFramesPerLine() == 0)
            {
                // Add this position along the line
                positions.Add(transform.position);

                // Remove oldest position, if we're storing too many
                if (positions.Count > configManager.GetTraceOrbitNumStoredPositions())
                {
                    positions.RemoveAt(0);
                }

                // Draw as line renderer
                lineRenderer.SetVertexCount(positions.Count);
                lineRenderer.SetPositions(positions.ToArray());

                currentFrameCount = 0;
            }
            currentFrameCount++;
        }

        
    }

    public void EraseLine()
    {
        positions.Clear();
        lineRenderer.SetVertexCount(0);
    }
}
