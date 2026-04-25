using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public Transform player;
    public float revealRadius = 5f;
    public LayerMask fogLayer;

    private void Update()
    {
        Collider[] fogTiles = Physics.OverlapSphere(player.position, revealRadius, fogLayer);

        foreach (Collider fog in fogTiles)
        {
            fog.gameObject.SetActive(false);
        }
    }
}