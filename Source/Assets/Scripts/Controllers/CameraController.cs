using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Space]
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    [Space]
    public float pitch = 2f;
    public float yawSpeed = 100f;

    float currentZoom = 10f;
    float currentYaw = 0f;

    void Update()
    {
        // Altering value of current zoom
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        // Altering value of angle yaw
        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        // Position of camera is dependent on an offset
        transform.position = target.position - offset * currentZoom;
        // Camera alternatively looks at the target position
        transform.LookAt(target.position - Vector3.up * pitch);
        // Camera rotates around the target position by angle yaw
        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }
}
