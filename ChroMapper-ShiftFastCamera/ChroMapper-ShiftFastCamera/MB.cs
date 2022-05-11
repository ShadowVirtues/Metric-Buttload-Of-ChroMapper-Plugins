using UnityEngine;

namespace ChroMapper_ShiftFastCamera
{
    public class MB : MonoBehaviour
    {
        private float InitialSpeed;

        void Start()
        {
            InitialSpeed = Settings.Instance.Camera_MovementSpeed;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) Settings.Instance.Camera_MovementSpeed = InitialSpeed * 4;
            if (Input.GetKeyUp(KeyCode.LeftShift)) Settings.Instance.Camera_MovementSpeed = InitialSpeed;
        }

    }
}
