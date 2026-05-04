using UnityEngine;

public class PingTargetManager : MonoBehaviour
{
    public static PingTargetManager Instance { get; private set; }

    public Transform CurrentPing { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetPingTarget(Transform pingTransform)
    {
        CurrentPing = pingTransform;
    }

    public void ClearPingTarget()
    {
        CurrentPing = null;
    }
}