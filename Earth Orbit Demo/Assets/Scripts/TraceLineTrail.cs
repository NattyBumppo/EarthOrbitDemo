using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TraceLineTrail : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int numFramesPerLine;
    public int numStoredPositions;
    public Transform objectToTrack;

    private int currentFrameCount;
    private List<Vector3> positions;

	void Start()
    {
        currentFrameCount = 0;
        positions = new List<Vector3>();
    }
	
	void Update()
    {
        // We probably don't want to sample/draw every frame
        if (currentFrameCount % numFramesPerLine == 0)
        {
            // Add this position along the line
            positions.Add(transform.position);

            // Remove oldest position, if we're storing too many
            if (positions.Count > numStoredPositions)
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
