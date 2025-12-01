using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = UserOptions.EnablePostProcessing;
    }
}
