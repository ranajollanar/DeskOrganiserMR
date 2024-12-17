using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ConstrainedDrawingPen : MonoBehaviour
{
    [SerializeField] private Transform penTip; // The object to follow (e.g., pen tip or collider)
    [SerializeField] private Transform whiteboard; // The plane to constrain the drawing
    [SerializeField] private float minDistance = 0.01f; // Minimum distance between points to draw
    [SerializeField] private float planeOffset = 0.2f; // Offset for the plane along its forward direction
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private Plane drawingPlane;
    private bool isCollidingWithWhiteboard = false; // Tracks if the pen tip is colliding with the whiteboard

    private void Start()
    {
        // Initialize LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // Start with no points
        lineRenderer.useWorldSpace = true; // Use world space for drawing
    }

    private void Update()
    {
        // Update the drawing plane to ensure it's aligned with the current position of the whiteboard
        UpdateDrawingPlane();

        // Only draw if the pen tip is colliding with the whiteboard
        if (!isCollidingWithWhiteboard) return;

        // Get the pen tip's projected position onto the plane
        Vector3 projectedPosition = ProjectPointOntoPlane(penTip.position);

        // Check if the pen tip has moved enough to add a new point
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], projectedPosition) >= minDistance)
        {
            AddPoint(projectedPosition);
        }
    }

    private void UpdateDrawingPlane()
    {
        // Define the drawing plane based on the whiteboard, shifted by planeOffset along its forward direction
        Vector3 planeNormal = whiteboard.forward; // Use the whiteboard's local forward as the plane's normal
        Vector3 planePosition = whiteboard.position + planeNormal * planeOffset; // Shift the plane position
        drawingPlane = new Plane(planeNormal, planePosition); // Update the drawing plane
    }

    private Vector3 ProjectPointOntoPlane(Vector3 point)
    {
        // Get the perpendicular distance from the point to the plane
        float distanceToPlane = drawingPlane.GetDistanceToPoint(point);

        // Subtract the distance along the plane's normal to project onto the plane
        return point - drawingPlane.normal * distanceToPlane;
    }

    private void AddPoint(Vector3 newPoint)
    {
        points.Add(newPoint); // Add the new point to the list
        lineRenderer.positionCount = points.Count; // Update the LineRenderer's point count
        lineRenderer.SetPosition(points.Count - 1, newPoint); // Set the new point in the LineRenderer
    }

    public void ClearDrawing()
    {
        points.Clear(); // Clear the points list
        lineRenderer.positionCount = 0; // Reset the LineRenderer
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the whiteboard
        if (other.transform == whiteboard)
        {
            isCollidingWithWhiteboard = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the whiteboard
        if (other.transform == whiteboard)
        {
            isCollidingWithWhiteboard = false;
        }
    }
}
