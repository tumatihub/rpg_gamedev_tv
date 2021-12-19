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

        void Update()
        {
            transform.forward = camera.transform.forward;
        }
    }
}
