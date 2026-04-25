using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject winCanvasVR;
    public GameObject winCanvasPC;
    public Collider barrierCollider;

    private bool hasWon = false;

    private void Start()
    {
        if (winCanvasVR != null)
        {
            winCanvasVR.SetActive(false);
        }

        if (winCanvasPC != null)
        {
            winCanvasPC.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null && GameManager.Instance.AllCheckpointsCompleted())
        {
            hasWon = true;

            Debug.Log("You Win!");

            if (winCanvasVR != null)
            {
                winCanvasVR.SetActive(true);
            }

            if (winCanvasPC != null)
            {
                winCanvasPC.SetActive(true);
            }

            if (barrierCollider != null)
            {
                barrierCollider.enabled = false;
            }
        }
        else
        {
            Debug.Log("Complete all checkpoints before using the exit.");
        }
    }
}