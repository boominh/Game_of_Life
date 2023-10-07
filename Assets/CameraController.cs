using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zoomSpeed = 0.5f;

    public float minZoom = 2f;
    public float maxZoom = 10f;

    Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed;
        transform.Translate(moveDirection * Time.unscaledDeltaTime);

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = mainCamera.orthographicSize - scrollWheelInput * zoomSpeed;
        Vector3 zoomDelta = mousePosition - transform.position;
        transform.position += zoomDelta * (mainCamera.orthographicSize - newZoom) / mainCamera.orthographicSize;
        mainCamera.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }
}