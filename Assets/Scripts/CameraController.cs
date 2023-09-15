using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [Tooltip("The size of the island in meters. This establishes the camera boundaries.")]
    [SerializeField]
    private float islandSize = 1024f;

    private CinemachineTransposer virtualCameraTransposer;

    [Tooltip("The layer used to raycast camera height.")]
    [SerializeField]
    private LayerMask terrainLayerMask;

    [Header("Camera Pan Settings")]
    [Tooltip("The speed that the camera can pan when fully zoomed out.")]
    [SerializeField]
    private float maxPanSpeed = 120f;

    [Tooltip("The speed that the camera can pan when fully zoomed in.")]
    [SerializeField]
    private float minPanSpeed = 20f;

    [Tooltip("Determines how quickly the camera pan speed accelerates and decelerates.")]
    [SerializeField]
    private float panAcceleration = 4f;

    private Vector3 panVector;

    private float targetHeight;

    [Header("Camera Rotate Settings")]
    [Tooltip("The maximum speed that the camera can rotate.")]
    [SerializeField]
    private float maxRotateSpeed = 64f;

    [SerializeField]
    [Tooltip("Determines how quickly the camera rotation accelerates and decelerates.")]
    private float rotateAcceleration = 8f;

    private float rotateSpeed;

    [Header("Camera Zoom Settings")]
    [Tooltip("Determines how low the camera can move while zooming.")]
    [SerializeField]
    private float minOffsetY = 6f;

    [Tooltip("Determines how high the camera can move while zooming.")]
    [SerializeField]
    private float maxOffsetY = 160f;

    private float offsetY;

    [Tooltip("Determines how quickly the camera will adjust to a new zoom height.")]
    [SerializeField]
    private float zoomAcceleration = 4f;

    [Tooltip("Adjusts the zoom speed when using the mouse scroll wheel.")]
    [SerializeField]
    private float scrollZoomAdjust = 4f;

    [Tooltip("Adjusts the zoom speed when using an axis input.")]
    [SerializeField]
    private float axisZoomAdjust = 64f;

    private GameInputActions gameInputActions;

    private void Awake()
    {
        gameInputActions = new GameInputActions();
    }

    private void OnEnable()
    {
        gameInputActions.Camera.ScrollZoom.performed += HandleScrollZoom;
        gameInputActions.Camera.Enable();
    }

    private void OnDisable()
    {
        gameInputActions.Camera.ScrollZoom.performed -= HandleScrollZoom;
    }

    private void Start()
    {
        virtualCameraTransposer = virtualCamera.GetComponentInChildren<CinemachineTransposer>();
        offsetY = virtualCameraTransposer.m_FollowOffset.y;
        RecalculateHeight();
    }

    private void Update()
    {
        HandleTargetPan();
        HandleTargetRotate();
        HandleCameraZoom();
    }

    private void HandleTargetPan()
    {
        Vector2 inputVector = gameInputActions.Camera.Pan.ReadValue<Vector2>();
        Vector3 targetDir = new Vector3(inputVector.x, 0f, inputVector.y);
        targetDir = transform.TransformVector(targetDir);
        float adjustedMaxPanSpeed = CalculateMaxPanSpeed();
        panVector = Vector3.Lerp(panVector, targetDir * adjustedMaxPanSpeed, panAcceleration * Time.deltaTime);

        // bound pan vector to the size of the island
        panVector = ApplyPanBoundaries(panVector);

        transform.position += panVector * Time.deltaTime;
        if (panVector != Vector3.zero || Mathf.Abs(transform.position.y - targetHeight) > 0.1f)
        {
            RecalculateHeight();

            float newHeight = Mathf.Lerp(transform.position.y, targetHeight, panAcceleration * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
        }
    }

    private void HandleTargetRotate()
    {
        float inputAxis = gameInputActions.Camera.Rotate.ReadValue<float>();
        rotateSpeed = Mathf.Lerp(rotateSpeed, inputAxis * maxRotateSpeed, rotateAcceleration * Time.deltaTime);

        if (rotateSpeed != 0f)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }
    }

    private void HandleCameraZoom()
    {
        float inputAxis = gameInputActions.Camera.AxisZoom.ReadValue<float>() * axisZoomAdjust;
        offsetY = Mathf.Clamp(offsetY + (inputAxis * Time.deltaTime), minOffsetY, maxOffsetY);

        // adjust the camera zoom by altering the camera body Y-offset
        Vector3 currentOffset = virtualCameraTransposer.m_FollowOffset;
        Vector3 targetOffset = new Vector3(currentOffset.x, offsetY, currentOffset.z);
        if (Vector3.Distance(currentOffset, targetOffset) > 0.1f)
        {
            Vector3 newOffset = Vector3.Lerp(currentOffset, targetOffset, zoomAcceleration * Time.deltaTime);
            virtualCameraTransposer.m_FollowOffset = newOffset;
        }
    }

    private void RecalculateHeight()
    {
        float raycastHeight = 100f; // the range of terrain heights should be less than 100 and greater than -100
        Vector3 raycastOrigin = new Vector3(transform.position.x, raycastHeight, transform.position.z);

        RaycastHit hitInfo;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hitInfo, raycastHeight * 2f, terrainLayerMask))
        {
            targetHeight = hitInfo.point.y;
        }
        else
        {
            // the main camera should always be positioned above the terrain or surrounding water
            Debug.LogError("Failed to raycast terrain!");
        }
    }

    private float CalculateMaxPanSpeed()
    {
        // interpolate the pan speed between the min and max speeds based on the camera zoom
        float zoomPercent = (offsetY - minOffsetY) / (maxOffsetY - minOffsetY);
        return (maxPanSpeed - minPanSpeed) * zoomPercent + minPanSpeed;
    }

    private Vector3 ApplyPanBoundaries(Vector3 panVector)
    {
        // clamps the pan vector based on the island size and current camera position
        float offsetIslandSize = islandSize / 2f;
        float minX = -offsetIslandSize - transform.position.x;
        float maxX = offsetIslandSize - transform.position.x;
        float minZ = -offsetIslandSize - transform.position.z;
        float maxZ = offsetIslandSize - transform.position.z;

        return new Vector3(Mathf.Clamp(panVector.x, minX, maxX), 0f, Mathf.Clamp(panVector.z, minZ, maxZ));
    }

    private void HandleScrollZoom(InputAction.CallbackContext context)
    {
        // scrolling can't be polled in the update so we need to immediately add mouse wheel
        // input to the zoom offset
        float inputAxis = context.ReadValue<float>() * scrollZoomAdjust;
        offsetY = Mathf.Clamp(offsetY + inputAxis, minOffsetY, maxOffsetY);
    }
}
