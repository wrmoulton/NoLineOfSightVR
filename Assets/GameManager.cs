using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int objectivesCollected = 0;

    public Transform startRespawnPoint;
    public Transform currentRespawnPoint;

    [Header("Checkpoint Progress")]
    public int nextCheckpointIndex = 1;
    public int totalCheckpoints = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentRespawnPoint = startRespawnPoint;
    }

    public void CollectObjective()
    {
        objectivesCollected++;
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        currentRespawnPoint = newCheckpoint;
        Debug.Log("Respawn updated to: " + newCheckpoint.name);
    }

    public bool TryActivateCheckpoint(int checkpointNumber, Transform checkpointTransform)
    {
        if (checkpointNumber == nextCheckpointIndex)
        {
            currentRespawnPoint = checkpointTransform;
            Debug.Log("Checkpoint " + checkpointNumber + " activated!");

            nextCheckpointIndex++;

            if (nextCheckpointIndex > totalCheckpoints)
            {
                Debug.Log("All checkpoints completed!");
            }

            return true;
        }
        else
        {
            Debug.Log("Wrong checkpoint. Need checkpoint " + nextCheckpointIndex + " next.");
            return false;
        }
    }

    public bool AllCheckpointsCompleted()
    {
        return nextCheckpointIndex > totalCheckpoints;
    }
}