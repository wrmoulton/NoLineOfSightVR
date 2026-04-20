using UnityEngine;

public class Objective : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectObjective();
            Destroy(gameObject);
        }
    }
}