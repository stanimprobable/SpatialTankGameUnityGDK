using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float _DampTime = 0.2f;                       // Approximate time for the camera to refocus.
    public float _ScreenEdgeBuffer = 4f;               // Space between the top/bottom most target and the screen edge.
    public float _MinSize = 30f;                       // The smallest orthographic size the camera can be.
    public Transform[] Targets;

    private Camera  _Camera;                        // Used for referencing the camera.
    private float  _ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3  _MoveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3  _DesiredPosition;              // The position the camera is moving towards.

    // Start is called before the first frame update
    void Awake()
    {
        _Camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, _DesiredPosition, ref _MoveVelocity, _DampTime);
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numberTargets = 0;

        for (int i = 0; i < Targets.Length; i++)
        {
            if (!Targets[i].gameObject.activeSelf)
                continue;

            averagePos += Targets[i].position;
            numberTargets++;
        }

        if (numberTargets > 0)
        {
            averagePos /= numberTargets;
        }

        averagePos.y = transform.position.y;
        _DesiredPosition = averagePos;
    }

    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        _Camera.orthographicSize = Mathf.SmoothDamp(_Camera.orthographicSize, requiredSize, ref _ZoomSpeed, _DampTime);
    }

    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(_DesiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        for (int i = 0; i <Targets.Length; i++)
        {
            // ... and if they aren't active continue on to the next target.
            if (!Targets[i].gameObject.activeSelf)
                continue;

            // Otherwise, find the position of the target in the camera's local space.
            Vector3 targetLocalPos = transform.InverseTransformPoint(Targets[i].position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) /_Camera.aspect);
        }

        // Add the edge buffer to the size.
        size += _ScreenEdgeBuffer;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, _MinSize);

        return size;

    }

    public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        // Set the camera's position to the desired position without damping.
        transform.position = _DesiredPosition;

        // Find and set the required size of the camera.
        _Camera.orthographicSize = FindRequiredSize();
    }
}
