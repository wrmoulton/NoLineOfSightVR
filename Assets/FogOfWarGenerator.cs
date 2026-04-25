using UnityEngine;

public class FogGridGenerator : MonoBehaviour
{
    public GameObject fogPlanePrefab;

    public int columns = 20;
    public int rows = 20;

    public float tileSize = 2f;
    public float yHeight = 3f;

    [ContextMenu("Generate Fog Grid")]
    public void GenerateFogGrid()
    {
        if (fogPlanePrefab == null)
        {
            Debug.LogError("Assign a fogPlanePrefab first.");
            return;
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 pos = transform.position + new Vector3(x * tileSize, yHeight, z * tileSize);

                GameObject tile = Instantiate(fogPlanePrefab, pos, Quaternion.identity, transform);
                tile.name = "FogPlane_" + x + "_" + z;

                // Unity planes are 10x10 by default
                float planeScale = tileSize / 10f;
                tile.transform.localScale = new Vector3(planeScale, 1f, planeScale);
            }
        }

        Debug.Log("Fog plane grid generated.");
    }
}