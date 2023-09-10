using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    private CinemachineTransposer virtualCameraTransposer;

    [Tooltip("The layer used to raycast camera height.")]
    [SerializeField]
    private LayerMask terrainLayerMask;

    [Header("Camera Pan Settings")]
    [Tooltip("The maximum speed that the camera can pan.")]
    [SerializeField]
    private float maxPanSpeed = 50f;

    [Tooltip("Determines how quickly the camera pan speed accelerates and decelerates.")]
    [SerializeField]
    private float panAcceleration = 4f;

    private Vector3 panVector;

    private float targetHeight;

    [Header("Camera Rotate Settings")]
    [Tooltip("The maximum speed that the camera can rotate.")]
    [SerializeField]
    private float maxRotateSpeed = 100f;

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
        gameInputActions.Camera.ScrollZoom.performed += context =>
        {
            float inputAxis = context.ReadValue<float>() * scrollZoomAdjust;
            offsetY = Mathf.Clamp(offsetY + inputAxis, minOffsetY, maxOffsetY);
        };
    }

    private void OnEnable()
    {
        gameInputActions.Camera.Enable();
    }

    private void OnDisable()
    {
        gameInputActions.Camera.Disable();
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
        panVector = Vector3.Lerp(panVector, targetDir * maxPanSpeed, panAcceleration * Time.deltaTime);

        transform.position += panVector * Time.deltaTime;
        if (panVector != Vector3.zero || Mathf.Abs(transform.position.y - targetHeight) > 0.1f)
        {
            RecalculateHeight();
            transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
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
        // handle camera zoom for gamepad controls TODO: use gamepad instead of keys
        float inputAxis = gameInputActions.Camera.TestZoom.ReadValue<float>() * axisZoomAdjust;
        offsetY = Mathf.Clamp(offsetY + (inputAxis * Time.deltaTime), minOffsetY, maxOffsetY);

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
        float raycastHeight = 100f;
        Vector3 raycastOrigin = new Vector3(transform.position.x, raycastHeight, transform.position.z);

        RaycastHit hitInfo;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hitInfo, raycastHeight * 2f, terrainLayerMask))
        {
            targetHeight = hitInfo.point.y;
        }
        else
        {
            Debug.LogError("Failed to raycast terrain!");
        }
    }
}
