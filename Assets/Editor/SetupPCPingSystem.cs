using UnityEngine;
using UnityEditor;

public class SetupPCPingSystem
{
    public static void Execute()
    {
        GameObject mapCameraObj = GameObject.Find("MapCamera");
        if (mapCameraObj != null)
        {
            PCPingSystem pingSystem = mapCameraObj.GetComponent<PCPingSystem>();
            if (pingSystem == null)
            {
                pingSystem = mapCameraObj.AddComponent<PCPingSystem>();
            }
            
            pingSystem.pcCamera = mapCameraObj.GetComponent<Camera>();
            
            // Create a nice ping prefab
            GameObject pingPrefab = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pingPrefab.name = "PCPingPrefab";
            pingPrefab.transform.localScale = new Vector3(2f, 10f, 2f);
            
            // Remove collider
            Object.DestroyImmediate(pingPrefab.GetComponent<Collider>());
            
            // Create material
            Material pingMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            pingMat.color = Color.cyan;
            pingMat.SetColor("_EmissionColor", Color.cyan * 3f);
            pingMat.EnableKeyword("_EMISSION");
            
            // Make it transparent
            pingMat.SetFloat("_Surface", 1); // Transparent
            pingMat.SetFloat("_Blend", 0); // Alpha
            pingMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            pingMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            pingMat.SetInt("_ZWrite", 0);
            pingMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            
            Color colorWithAlpha = Color.cyan;
            colorWithAlpha.a = 0.6f;
            pingMat.color = colorWithAlpha;
            
            AssetDatabase.CreateAsset(pingMat, "Assets/PCPingMaterial.mat");
            
            pingPrefab.GetComponent<Renderer>().sharedMaterial = pingMat;
            
            // Save as prefab
            GameObject savedPrefab = PrefabUtility.SaveAsPrefabAsset(pingPrefab, "Assets/PCPingPrefab.prefab");
            Object.DestroyImmediate(pingPrefab);
            
            pingSystem.pingPrefab = savedPrefab;
            
            EditorUtility.SetDirty(mapCameraObj);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(mapCameraObj.scene);
            
            Debug.Log("PCPingSystem setup complete.");
        }
        else
        {
            Debug.LogError("MapCamera not found.");
        }
    }
}
