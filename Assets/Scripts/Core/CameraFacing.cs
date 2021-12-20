using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        Camera camera;

        void Awake()
        {
            camera = Camera.main;
        }

        void LateUpdate()
        {
            transform.forward = camera.transform.forward;
        }
    }
}
