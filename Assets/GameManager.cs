using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int objectivesCollected = 0;

    [Header("Respawn")]
    public Transform startRespawnPoint;
    public Transform currentRespawnPoint;

    [Header("Checkpoint Progress")]
    public int nextCheckpointIndex = 1;
    public int totalCheckpoints = 5;

    [Header("Timer")]
    public float timeLimit = 180f;
    private float currentTime;
    private bool gameOver = false;

    [Header("Game Over UI")]
    public GameObject vrGameOverCanvas;
    public GameObject pcGameOverCanvas;

    [Header("Optional Timer Text")]
    public TMP_Text vrTimerText;
    public TMP_Text pcTimerText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentRespawnPoint = startRespawnPoint;
        currentTime = timeLimit;

        if (vrGameOverCanvas != null)
            vrGameOverCanvas.SetActive(false);

        if (pcGameOverCanvas != null)
            pcGameOverCanvas.SetActive(false);
    }

    private void Update()
    {
        if (gameOver)
            return;

        currentTime -= Time.deltaTime;

        UpdateTimerUI();

        if (currentTime <= 0)
        {
            currentTime = 0;
            TriggerGameOver();
        }
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
        if (gameOver)
            return false;

        if (checkpointNumber == nextCheckpointIndex)
        {
            currentRespawnPoint = checkpointTransform;
            Debug.Log("Checkpoint " + checkpointNumber + " activated!");

            nextCheckpointIndex++;

            if (nextCheckpointIndex > totalCheckpoints)
            {
                Debug.Log("All checkpoints completed!");
                TriggerWin();
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

    private void TriggerGameOver()
    {
        gameOver = true;

        if (vrGameOverCanvas != null)
            vrGameOverCanvas.SetActive(true);

        if (pcGameOverCanvas != null)
            pcGameOverCanvas.SetActive(true);

        Debug.Log("Game Over!");
    }

    private void TriggerWin()
    {
        gameOver = true;

        Debug.Log("You Win!");

        // Later  show win canvases here too.
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        string timerString = minutes.ToString("00") + ":" + seconds.ToString("00");

        if (vrTimerText != null)
            vrTimerText.text = timerString;

        if (pcTimerText != null)
            pcTimerText.text = timerString;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}