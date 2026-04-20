using UnityEngine;

public class TrapZone : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            other.transform.root.position = respawnPoint.position;

            if (cc != null)
                cc.enabled = true;
        }
    }
}