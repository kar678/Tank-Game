using UnityEngine;
using System.Collections;
using Cogwheel.CustomInput;

namespace Cogwheel.Player
{
    public class DragMouseOrbit : MonoBehaviour
    {
        [Header("Game")]
        public Transform target;
        public bool rotateOnTheSpot;
        public bool inGame = false;
        PlayerInputActions inputActions;

        [Header("Camera Adjustments")]
        public float distance = 2.0f;
        public float xSpeed = 20.0f;
        public float ySpeed = 20.0f;
        public float yMinLimit = -90f;
        public float yMaxLimit = 90f;
        public float distanceMin = 10f;
        public float distanceMax = 10f;
        public float smoothTime = 2f;
        float rotationYAxis = 0.0f;
        float rotationXAxis = 0.0f;
        float velocityX = 0.0f;
        float velocityY = 0.0f;
        public LayerMask cameraDistanceAdjustmentLayerMask;

        private void Awake()
        {
            inputActions = new PlayerInputActions();
        }

        // Use this for initialization
        void Start()
        {
            Vector3 angles = transform.eulerAngles;
            rotationYAxis = angles.y;
            rotationXAxis = angles.x;
            // Make the rigid body not change rotation
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().freezeRotation = true;
            }

            if (inGame)
            {
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<TankController>().cameraOrbitPoint;
            }

            inputActions.UI.DragCamera.Enable();
            inputActions.Player.RotateCameraH.Enable();
            inputActions.Player.RotateCameraV.Enable();
            inputActions.Player.CameraZoom.Enable();
        }

        void FixedUpdate()
        {
            if (target)
            {
                if (inputActions.UI.DragCamera.ReadValue<float>() > 0f || inGame)
                {
                    velocityX += xSpeed * inputActions.Player.RotateCameraH.ReadValue<float>() * 0.02f;
                    velocityY += ySpeed * inputActions.Player.RotateCameraV.ReadValue<float>() * 0.02f;
                }

                rotationYAxis += velocityX;
                rotationXAxis -= velocityY;
                rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
                Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
                Quaternion rotation = toRotation;

                if (!rotateOnTheSpot)
                {
                    distance = Mathf.Clamp(distance - inputActions.Player.CameraZoom.ReadValue<float>(), distanceMin, distanceMax);
                    RaycastHit hit;
                    if (Physics.Linecast(target.position, transform.position, out hit, cameraDistanceAdjustmentLayerMask))
                    {
                        float distanceToCamera = Vector3.Distance(hit.transform.position, transform.position);
                        distance -= distanceToCamera;
                    }
                    Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                    Vector3 position = rotation * negDistance + target.position;

                    transform.position = position;
                }

                transform.rotation = rotation;
                velocityX = Mathf.Lerp(velocityX, 0, smoothTime * Time.deltaTime);
                velocityY = Mathf.Lerp(velocityY, 0, smoothTime * Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            inputActions.UI.DragCamera.Disable();
            inputActions.Player.RotateCameraH.Disable();
            inputActions.Player.RotateCameraV.Disable();
            inputActions.Player.CameraZoom.Disable();
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
