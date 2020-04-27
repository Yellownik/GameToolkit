using UnityEngine;
using System.Collections;

//orbox: TODO: choose appropriate namespace
namespace Orbox.DebugTools
{
    //AddComponentMenu("Camera-Control/Mouse Orbit")
    public class MouseOrbit : MonoBehaviour
    {

        public Transform Target;
        public float Distance = 10.0f;

        public float xSpeed = 250.0f;
        public float ySpeed = 120.0f;

        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;

        private float x = 0.0f;
        private float y = 0.0f;

        // Use this for initialization
        void Start()
        {
            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            // Make the rigid body not change rotation
            if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;

            MoveCamera();
        }

        void LateUpdate()
        {
            if (Target == null) return;

            const int right = 1;

            bool isButtonDown = Input.GetMouseButton(right);
            var mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
            Distance = Distance - mouseScrollWheel * 5;

            if (isButtonDown || mouseScrollWheel != 0) MoveCamera();

        }

        void MoveCamera()
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -Distance) + Target.position;

            transform.rotation = rotation;
            transform.position = position;
        }

        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360) angle += 360;
            if (angle > 360) angle -= 360;

            return Mathf.Clamp(angle, min, max);
        }
    }
}



