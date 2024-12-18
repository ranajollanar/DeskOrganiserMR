using System;
using System.Collections.Generic;

using UnityEngine;

public class DrawingPen : MonoBehaviour
{
    [SerializeField] private Transform penTip;
    [SerializeField] private Transform whiteboard;
    [SerializeField] private float minDistance = 0.01f;
    [SerializeField] private float planeOffset = 0.2f;
    [SerializeField] private LineRenderer lineRendererPreset;
    [SerializeField] private LineRenderer lineRendererBlue;
    [SerializeField] private LineRenderer lineRendererPink;
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private List<LineRenderer> drawnLines = new List<LineRenderer>();  // List to track drawn lines
    private Plane drawingPlane;
    private bool isCollidingWithWhiteboard = false;
    private bool isDrawing = false;
    private String penColor = "Black";

    private void Start()
    {
        CreateNewLineRenderer();
    }

    private void Update()
    {
        UpdateDrawingPlane();

        if (isCollidingWithWhiteboard)
        {
            // Get the pen tip's projected position onto the plane
            Vector3 projectedPosition = ProjectPointOntoPlane(penTip.position);

            if (!isDrawing)
            {
                CreateNewLineRenderer();
                isDrawing = true;
            }

            if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], projectedPosition) >= minDistance)
            {
                AddPoint(projectedPosition);
            }
        }
        else
        {
            isDrawing = false;
        }
    }

    public void ChangeLineRendererColor(string colour)
    {
        penColor = colour;

    }


    private void UpdateDrawingPlane()
    {
        Vector3 planeNormal = whiteboard.forward; // Use the whiteboard's local forward as the plane's normal
        Vector3 planePosition = whiteboard.position + planeNormal * planeOffset; // Shift the plane position
        drawingPlane = new Plane(planeNormal, planePosition); 
    }

    private Vector3 ProjectPointOntoPlane(Vector3 point)
    {
        float distanceToPlane = drawingPlane.GetDistanceToPoint(point);

        return point - drawingPlane.normal * distanceToPlane;
    }

    private void AddPoint(Vector3 newPoint)
    {
        if (points.Count == 0)
        {
            points.Add(newPoint);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, newPoint);
            return;
        }
        
        Vector3 lastPoint = points[points.Count - 1];

        float distance = Vector3.Distance(lastPoint, newPoint);
        int interpolationSteps = Mathf.CeilToInt(distance / minDistance);

        for (int i = 1; i <= interpolationSteps; i++)
        {
            float t = i * 3 / (float)interpolationSteps;
            Vector3 interpolatedPoint = Vector3.Lerp(lastPoint, newPoint, t);

            points.Add(interpolatedPoint);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, interpolatedPoint);
        }
    }

    private void CreateNewLineRenderer()
    {
        LineRenderer newLineRenderer = null;

        if (penColor == "Black")
        {
            newLineRenderer = Instantiate(lineRendererPreset, Vector3.zero, Quaternion.identity);
        }
        else if (penColor == "Blue")
        {
            newLineRenderer = Instantiate(lineRendererBlue, Vector3.zero, Quaternion.identity);
        }
        else if (penColor == "Pink")
        {
            newLineRenderer = Instantiate(lineRendererPink, Vector3.zero, Quaternion.identity);
        }

        lineRenderer = newLineRenderer;

        // Parent the line renderer to the whiteboard
        lineRenderer.transform.SetParent(whiteboard);

        // Clear any previous points and reset the LineRenderer
        lineRenderer.positionCount = 0;
        points.Clear(); // Reset the points list

        // Add this LineRenderer to the list of drawn lines
        drawnLines.Add(lineRenderer);
    }


    public void DeleteLastLineRenderer()
    {
        if (drawnLines.Count <= 0) return;
        if (drawnLines.Count > 0)
        {
            // Destroy the last drawn line
            LineRenderer lastLine = drawnLines[drawnLines.Count - 1];
            drawnLines.RemoveAt(drawnLines.Count - 1);
            Destroy(lastLine.gameObject); // Destroy the last LineRenderer

            // Reset the points and the LineRenderer for new drawing
            if (drawnLines.Count > 0)
            {
                lineRenderer = drawnLines[drawnLines.Count - 1]; // Set the current line to the last drawn one
                points.Clear();
                // Recreate the line with the points from the last line (optional, for a smooth transition)
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == whiteboard)
        {
            isCollidingWithWhiteboard = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == whiteboard)
        {
            isCollidingWithWhiteboard = false;
        }
    }
}
