using UnityEngine;

public class TrapZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.currentRespawnPoint == null)
            {
                Debug.LogWarning("No respawn point set.");
                return;
            }

            CharacterController cc = other.GetComponentInParent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            Transform playerRoot = other.transform.root;
            playerRoot.position = GameManager.Instance.currentRespawnPoint.position;

            if (cc != null)
                cc.enabled = true;
        }
    }
}