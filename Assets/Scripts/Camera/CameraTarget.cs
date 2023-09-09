using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]
    private LayerMask terrainLayerMask;

    private void Start()
    {
        RecalculateHeight();
    }

    private void RecalculateHeight()
    {
        float raycastHeight = 100f;
        Vector3 raycastOrigin = new Vector3(transform.position.x, raycastHeight, transform.position.z);

        RaycastHit hitInfo;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hitInfo, raycastHeight * 2f, terrainLayerMask))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }
        else
        {
            Debug.LogError("Failed to raycast terrain!");
        }
    }
}
