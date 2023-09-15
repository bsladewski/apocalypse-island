using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Instance;

    [SerializeField]
    private Camera raycastCamera;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private LayerMask terrainLayer;

    [SerializeField]
    private LayerMask interactionLayer;

    private GameInputActions gameInputActions;

    public event EventHandler<GameObject> OnSelect;

    public event EventHandler OnDeselect;

    public event EventHandler<Vector3> OnMoveInteraction;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Singleton InteractionSystem already instantiated!");
        }
        Instance = this;
        gameInputActions = new GameInputActions();
    }

    private void OnEnable()
    {
        gameInputActions.Interactions.LeftClick.performed += HandleLeftClick;
        gameInputActions.Interactions.RightClick.performed += HandleRightClick;
        gameInputActions.Interactions.Enable();
    }

    private void OnDisable()
    {
        gameInputActions.Interactions.LeftClick.performed -= HandleLeftClick;
        gameInputActions.Interactions.RightClick.performed -= HandleRightClick;
    }

    private void HandleLeftClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = gameInputActions.Interactions.MousePosition.ReadValue<Vector2>();
        Ray ray = raycastCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, layerMask))
        {
            int hitLayer = hit.transform.gameObject.layer;
            if (CollisionUtils.LayerMaskContainsAny(interactionLayer, hitLayer))
            {
                if (hit.transform.GetComponent<Selectable>() != null)
                {
                    OnSelect?.Invoke(this, hit.transform.gameObject);
                }
            }

            if (CollisionUtils.LayerMaskContainsAny(terrainLayer, hitLayer))
            {
                OnDeselect?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void HandleRightClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = gameInputActions.Interactions.MousePosition.ReadValue<Vector2>();
        Ray ray = raycastCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, layerMask))
        {
            int hitLayer = hit.transform.gameObject.layer;
            if (CollisionUtils.LayerMaskContainsAny(terrainLayer, hitLayer))
            {
                OnMoveInteraction?.Invoke(this, hit.point);
            }
        }
    }
}
