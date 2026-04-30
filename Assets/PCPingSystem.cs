using UnityEngine;
using UnityEngine.InputSystem;

public class PCPingSystem : MonoBehaviour
{
    [Header("Settings")]
    public Camera pcCamera;
    public GameObject pingPrefab;
    public float pingDuration = 3f;
    public float pingYPosition = 0f; 
    public float pingPCYPosition = 1f; 
    public float pingPCScale = 1f; // Scale multiplier for the ping
    public LayerMask raycastLayerMask = ~0; // All layers by default

    private GameObject currentPing;

    void Update()
    {
        // Check for left mouse click
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (pcCamera == null) return;

            Vector3 mousePos = Mouse.current.position.ReadValue();
            
            // Handle multiple displays
            if (Display.displays.Length > 1)
            {
                Vector3 relativeMousePos = Display.RelativeMouseAt(mousePos);
                if (relativeMousePos != Vector3.zero)
                {
                    // relativeMousePos.z contains the display index
                    if ((int)relativeMousePos.z == pcCamera.targetDisplay)
                    {
                        mousePos = new Vector3(relativeMousePos.x, relativeMousePos.y, 0);
                    }
                    else
                    {
                        // Click was on a different display
                        return;
                    }
                }
            }

            Ray ray = pcCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, raycastLayerMask))
            {
                SpawnPing(hit.point);
                SpawnDifferentPing(hit.point);
            }
        }
    }

    void SpawnPing(Vector3 position)
    {
        // Keep X and Z from hit point, use fixed Y coordinate
        Vector3 pingPosition = new Vector3(position.x, pingYPosition, position.z);
        Quaternion pingRotation = Quaternion.Euler(-90f, 0f, 0f);

        if (currentPing != null)
        {
            Destroy(currentPing);
        }

        if (pingPrefab != null)
        {
            currentPing = Instantiate(pingPrefab, pingPosition, pingRotation);
                currentPing.transform.localScale *= pingPCScale;
            Destroy(currentPing, pingDuration);
        }
        else
        {
            // Fallback if no prefab is assigned: create a primitive cylinder
            currentPing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            currentPing.transform.position = pingPosition;
            currentPing.transform.rotation = pingRotation;
            currentPing.transform.localScale = new Vector3(1f, 5f, 1f);
            
            // Remove collider so it doesn't interfere with player movement
            Destroy(currentPing.GetComponent<Collider>());

            // Try to make it look like a ping (e.g., yellow color)
            Renderer rend = currentPing.GetComponent<Renderer>();
            if (rend != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = Color.yellow;
                mat.SetColor("_EmissionColor", Color.yellow * 2f);
                mat.EnableKeyword("_EMISSION");
                rend.material = mat;
            }

            Destroy(currentPing, pingDuration);
        }
    }

    void SpawnDifferentPing(Vector3 position)
    {
        // Keep X and Z from hit point, use fixed Y coordinate
        Vector3 pingPosition = new Vector3(position.x, pingPCYPosition, position.z);
        Quaternion pingRotation = Quaternion.Euler(0f, 0f, 0f);

        if (currentPing != null)
        {
            Destroy(currentPing);
        }

        if (pingPrefab != null)
        {
            currentPing = Instantiate(pingPrefab, pingPosition, pingRotation);
            currentPing.transform.localScale *= pingPCScale;
            Destroy(currentPing, pingDuration);
        }
    }
}
