using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            bool success = GameManager.Instance.TryActivateCheckpoint(checkpointNumber, transform);

            if (success)
            {
                activated = true;
                Debug.Log("Checkpoint " + checkpointNumber + " collected.");
            }
        }
    }
}