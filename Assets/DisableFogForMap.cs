using UnityEngine;
using UnityEngine.Rendering;

public class DisableFogForCamera : MonoBehaviour
{
    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCamera;
        RenderPipelineManager.endCameraRendering += OnEndCamera;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCamera;
        RenderPipelineManager.endCameraRendering -= OnEndCamera;
    }

    void OnBeginCamera(ScriptableRenderContext context, Camera camera)
    {
        if (camera == GetComponent<Camera>()) RenderSettings.fog = false;
    }

    void OnEndCamera(ScriptableRenderContext context, Camera camera)
    {
        if (camera == GetComponent<Camera>()) RenderSettings.fog = true;
    }
}