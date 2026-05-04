using UnityEngine;
using UnityEngine.InputSystem;

public class PCPingSystem : MonoBehaviour
{
    [Header("Settings")]
    public Camera pcCamera;
    public GameObject pingPrefab;

    [Header("Ping Timing")]
    public float pingDuration = 20f;

    [Header("VR Ping - Compass Target")]
    public float pingYPosition = 20f;
    public float pingVRScale = 1f;

    [Header("PC Ping - Big Visual Marker")]
    public float pingPCYPosition = 50f;
    public float pingPCScale = 9f;

    [Header("Raycast")]
    public LayerMask raycastLayerMask = ~0;

    private GameObject currentVRPing;
    private GameObject currentPCPing;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlacePing();
        }
    }

    private void TryPlacePing()
    {
        if (pcCamera == null)
            return;

        Vector3 mousePos = Mouse.current.position.ReadValue();

        // Handles multiple displays
        if (Display.displays.Length > 1)
        {
            Vector3 relativeMousePos = Display.RelativeMouseAt(mousePos);

            if (relativeMousePos != Vector3.zero)
            {
                if ((int)relativeMousePos.z == pcCamera.targetDisplay)
                {
                    mousePos = new Vector3(relativeMousePos.x, relativeMousePos.y, 0f);
                }
                else
                {
                    return;
                }
            }
        }

        Ray ray = pcCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, raycastLayerMask))
        {
            SpawnVRPing(hit.point);
            SpawnPCPing(hit.point);
        }
    }

    private void SpawnVRPing(Vector3 position)
    {
        Vector3 pingPosition = new Vector3(position.x, pingYPosition, position.z);
        Quaternion pingRotation = Quaternion.Euler(-90f, 0f, 0f);

        if (currentVRPing != null)
        {
            Destroy(currentVRPing);
            PingTargetManager.Instance?.ClearPingTarget();
        }

        currentVRPing = CreatePing(pingPosition, pingRotation, pingVRScale, "VR Ping");

        if (currentVRPing != null)
        {
            PingTargetManager.Instance?.SetPingTarget(currentVRPing.transform);
        }

        CancelInvoke(nameof(ClearVRPing));
        Invoke(nameof(ClearVRPing), pingDuration);
    }

    private void SpawnPCPing(Vector3 position)
    {
        Vector3 pingPosition = new Vector3(position.x, pingPCYPosition, position.z);
        Quaternion pingRotation = Quaternion.identity;

        if (currentPCPing != null)
        {
            Destroy(currentPCPing);
        }

        currentPCPing = CreatePing(pingPosition, pingRotation, pingPCScale, "PC Ping");

        CancelInvoke(nameof(ClearPCPing));
        Invoke(nameof(ClearPCPing), pingDuration);
    }

    private GameObject CreatePing(Vector3 position, Quaternion rotation, float scaleMultiplier, string objectName)
    {
        GameObject pingObject;

        if (pingPrefab != null)
        {
            pingObject = Instantiate(pingPrefab, position, rotation);
            pingObject.name = objectName;
            pingObject.transform.localScale *= scaleMultiplier;
        }
        else
        {
            pingObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pingObject.name = objectName;
            pingObject.transform.position = position;
            pingObject.transform.rotation = rotation;
            pingObject.transform.localScale = new Vector3(1f, 5f, 1f) * scaleMultiplier;

            Collider col = pingObject.GetComponent<Collider>();
            if (col != null)
                Destroy(col);

            Renderer rend = pingObject.GetComponent<Renderer>();
            if (rend != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = Color.yellow;
                mat.SetColor("_EmissionColor", Color.yellow * 2f);
                mat.EnableKeyword("_EMISSION");
                rend.material = mat;
            }
        }

        return pingObject;
    }

    private void ClearVRPing()
    {
        if (currentVRPing != null)
        {
            Destroy(currentVRPing);
            currentVRPing = null;
        }

        PingTargetManager.Instance?.ClearPingTarget();
    }

    private void ClearPCPing()
    {
        if (currentPCPing != null)
        {
            Destroy(currentPCPing);
            currentPCPing = null;
        }
    }
}