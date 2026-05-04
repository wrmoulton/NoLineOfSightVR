using UnityEngine;

public class CompassPingPointer : MonoBehaviour
{
    [Header("References")]
    public Transform playerHead;   // XR Origin/Main Camera
    public Transform needlePivot;  // NeedlePivot, not the Cylinder

    [Header("Rotation Settings")]
    public bool hideNeedleWhenNoPing = true;
    public float rotationSmoothSpeed = 10f;

    [Tooltip("Use this if the needle points sideways/backwards. Try 0, 90, -90, or 180.")]
    public float needleForwardOffset = 0f;

    void Update()
    {
        if (playerHead == null || needlePivot == null)
            return;

        Transform ping = null;

        if (PingTargetManager.Instance != null)
        {
            ping = PingTargetManager.Instance.CurrentPing;
        }

        if (ping == null)
        {
            if (hideNeedleWhenNoPing)
                needlePivot.gameObject.SetActive(false);

            return;
        }

        needlePivot.gameObject.SetActive(true);

        Vector3 directionToPing = ping.position - playerHead.position;

        // Ignore vertical height difference so the compass only rotates flat.
        directionToPing.y = 0f;

        if (directionToPing.sqrMagnitude < 0.001f)
            return;

        // Convert the world-space ping direction into this compass object's local space.
        Vector3 localDirection = transform.InverseTransformDirection(directionToPing.normalized);

        // Angle around the compass.
        float targetAngle = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;

        targetAngle += needleForwardOffset;

        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

        needlePivot.localRotation = Quaternion.Slerp(
            needlePivot.localRotation,
            targetRotation,
            Time.deltaTime * rotationSmoothSpeed
        );
    }
}