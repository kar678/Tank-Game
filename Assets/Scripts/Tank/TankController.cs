using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cogwheel.CustomInput;
using static UnityEngine.InputSystem.InputAction;

namespace Cogwheel.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioSource))]
    public class TankController : MonoBehaviour
    {
        #region VARS

        //Movement Vars
        [Header("Physics Movement System")]
        public bool newMovementSystem = false;
        private float horizontalInput;
        private float verticalInput;
        public WheelCollider[] leftWheelColliders;
        public WheelCollider[] rightWheelColliders;
        public Transform[] leftWheelTransforms;
        public Transform[] rightWheelTransforms;
        public Quaternion leftWheelRotOffset;
        public Quaternion rightWheelRotOffset;
        public float motorForce = 7000;
        public float maxWheelRPM = 150;
        public float maxWheelReverseRPM = -75;
        public float wheelTurnMovingMultiplier = 2f;
        public float wheelTurnMovingDivider = 2f;

        [Header("Controls")]
        public bool disableControls = false;

        Rigidbody rb;

        #region Input
        PlayerInputActions inputActions;
        #endregion

        //Health Vars
        [Header("Health")]
        public float maxHitPoints = 100;
        [HideInInspector]
        public float currentHitPoints;
        public GameObject explosionEffect;
        public Slider healthSlider;

        //Turret Vars
        [Header("Turret")]
        public Transform tankTurretTransform;

        /* Speed at which a full rotation is performed */
        [Range(0, 360)]public int rotationSpeed = 200;

        //Sound Vars
        [Header("Sound")]
        public bool oneEngineSound;
        AudioSource sourceSound;
        public AudioSource weaponSourceSound;
        public AudioClip engineSound;
        public AudioClip idleEngineSound;
        public AudioClip activeEngineSound;

        //Game Vars
        [HideInInspector]
        public Camera gameCamera;
        [Header("Game")]
        public bool useReticle = true;
        public LayerMask reticleLayerMask;
        public Transform cameraOrbitPoint;
        Transform reticleTransform;

        //Gun Vars
        [Header("Gun")]
        public Transform firePoint;
        public GameObject shellPrefab;
        public float shellMaxDamage = 70f;
        public float maxShellDistance = 20f;
        public float minimumShellDistance = 4f;
        [HideInInspector]
        public float shellVelocity;
        public int maxAmmo = 1;
        [HideInInspector]
        public int currentAmmo;
        public float reloadTime = 4.0f;
        [HideInInspector]
        public float reloadTimeLeft;
        [HideInInspector]
        public bool isReloading = false;
        public float timeBetweenShots = 1.0f;
        private float timeSinceLastShot;
        public AudioClip weaponSound;
        public Slider reloadSlider;

        #endregion

        #region Properties
        private Vector3 reticlePosition;
        private Vector3 mousePosition;
        public Vector3 ReticlePosition
        {
            get { return reticlePosition; }
        }

        private Vector3 reticleNormal;
        public Vector3 ReticleNormal
        {
            get { return ReticleNormal; }
        }
        #endregion

        private void Awake()
        {
            inputActions = new PlayerInputActions();
            if (GameObject.FindGameObjectWithTag("Reticle") && !disableControls) { reticleTransform = GameObject.FindGameObjectWithTag("Reticle").transform; };
            if (GameObject.FindGameObjectWithTag("MainCamera") && !disableControls) { gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); };
            sourceSound = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();

            SetupInput();
        }

        // Start is called before the first frame update
        void Start()
        {
            weaponSourceSound.clip = weaponSound;
            currentAmmo = maxAmmo;
            currentHitPoints = maxHitPoints;
        }

        void SetupInput()
        {
            inputActions.Player.Fire.performed += InputFire;
            inputActions.Player.Fire.Enable();
            inputActions.Player.MoveForwardBackward.Enable();
            inputActions.Player.TurnLeftRight.Enable();
        }

        // Update is called once per frame
        void Update()
        {
            if (!disableControls)
            {
                HandleInput();
                HandleSound();
                HandleHealth();
                UpdateUI();

                if (currentAmmo <= 0 && !isReloading && !disableControls)
                {
                    StartCoroutine(Reload());
                }

                if (isReloading)
                {
                    reloadTimeLeft -= Time.deltaTime;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!disableControls)
            {
                HandleTurret();

                if(newMovementSystem && rb)
                {
                    HandlePhysicsMovement();
                    UpdateWheelPoses();
                }

                if (useReticle && gameCamera)
                {
                    HandleReticle();
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(mousePosition, 0.5f);
        }

        private void OnDisable()
        {
            inputActions.Player.Fire.Disable();
            inputActions.Player.MoveForwardBackward.Disable();
            inputActions.Player.TurnLeftRight.Disable();
        }

        #region Custom Methods
        protected virtual void HandlePhysicsMovement()
        {
            if (leftWheelColliders[0].rpm <= maxWheelRPM && rightWheelColliders[0].rpm <= maxWheelRPM && leftWheelColliders[0].rpm >= maxWheelReverseRPM && rightWheelColliders[0].rpm >= maxWheelReverseRPM)
            {
                if(horizontalInput > 0 && verticalInput > 0 || horizontalInput > 0 && verticalInput < 0)
                {
                    for (int i = 0; i < leftWheelColliders.Length; i++)
                    {
                        leftWheelColliders[i].motorTorque = ((verticalInput * motorForce) * Time.fixedDeltaTime) * wheelTurnMovingMultiplier;
                    }

                    for (int i = 0; i < rightWheelColliders.Length; i++)
                    {
                        rightWheelColliders[i].motorTorque = ((verticalInput * motorForce) * Time.fixedDeltaTime) / wheelTurnMovingDivider;
                    }
                    //Debug.Log(1);
                }
                else if(horizontalInput < 0 && verticalInput > 0 || horizontalInput < 0 && verticalInput < 0)
                {
                    for (int i = 0; i < leftWheelColliders.Length; i++)
                    {
                        leftWheelColliders[i].motorTorque = ((verticalInput * motorForce) * Time.fixedDeltaTime) / wheelTurnMovingDivider;
                    }

                    for (int i = 0; i < rightWheelColliders.Length; i++)
                    {
                        rightWheelColliders[i].motorTorque = ((verticalInput * motorForce) * Time.fixedDeltaTime) * wheelTurnMovingMultiplier;
                    }
                }
                else if (horizontalInput > 0)
                {
                    for (int i = 0; i < leftWheelColliders.Length; i++)
                    {
                        leftWheelColliders[i].motorTorque = (horizontalInput * motorForce) * Time.fixedDeltaTime;
                    }

                    for (int i = 0; i < rightWheelColliders.Length; i++)
                    {
                        rightWheelColliders[i].motorTorque = ((horizontalInput * motorForce) * Time.fixedDeltaTime) * -1;
                    }
                    //Debug.Log(horizontalInput);
                }
                else if (horizontalInput < 0)
                {
                    for (int i = 0; i < leftWheelColliders.Length; i++)
                    {
                        leftWheelColliders[i].motorTorque = (horizontalInput * motorForce) * Time.fixedDeltaTime;
                    }

                    for (int i = 0; i < rightWheelColliders.Length; i++)
                    {
                        rightWheelColliders[i].motorTorque = ((horizontalInput * motorForce) * Time.fixedDeltaTime) * -1;
                    }
                    //Debug.Log(10);
                }
                else
                {
                    for (int i = 0; i < leftWheelColliders.Length; i++)
                    {
                        leftWheelColliders[i].motorTorque = (verticalInput * motorForce) * Time.fixedDeltaTime;
                    }

                    for (int i = 0; i < rightWheelColliders.Length; i++)
                    {
                        rightWheelColliders[i].motorTorque = (verticalInput * motorForce) * Time.fixedDeltaTime;
                    }
                    //Debug.Log(3);
                }
            }
            else if (leftWheelColliders[0].rpm > maxWheelRPM || rightWheelColliders[0].rpm > maxWheelRPM || leftWheelColliders[0].rpm < maxWheelReverseRPM || rightWheelColliders[0].rpm < maxWheelReverseRPM)
            {
                for (int i = 0; i < leftWheelColliders.Length; i++)
                {
                    leftWheelColliders[i].motorTorque = 0;
                }

                for (int i = 0; i < rightWheelColliders.Length; i++)
                {
                    rightWheelColliders[i].motorTorque = 0;
                }
                //Debug.Log(4);
            }
            //Debug.Log(leftWheelColliders[0].rpm);
        }

        protected virtual void HandleInput()
        {
            inputActions.Player.MoveForwardBackward.performed += ctx =>
            {
                verticalInput = ctx.ReadValue<float>();
            };

            inputActions.Player.TurnLeftRight.performed += ctx =>
            {
                horizontalInput = ctx.ReadValue<float>();
            };

            Ray screenRay = gameCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if(Physics.Raycast(screenRay, out hit, float.PositiveInfinity, reticleLayerMask))
            {
                mousePosition = hit.point;
                CalculateShellVelocity();

            }
        }

        protected virtual void HandleSound()
        {
            if (newMovementSystem)
            {
                if (!oneEngineSound)
                {
                    if (activeEngineSound)
                    {
                        if(sourceSound.clip != activeEngineSound)
                        {
                            sourceSound.clip = activeEngineSound;
                            sourceSound.Play();
                        }

                        //if (moveSpeedReverse > 0 && !moveForward)
                        //{
                            //sourceSound.pitch = (moveSpeedReverse / moveSpeedForwardMax - 1) * -1;
                        //}

                        //if (moveSpeed > 0 && !moveReverse)
                        //{
                            //sourceSound.pitch = moveSpeed / moveSpeedForwardMax + 1;
                        //}
                    }
                }
                else if (oneEngineSound)
                {
                    if (engineSound && !newMovementSystem)
                    {
                        if (!sourceSound.isPlaying)
                        {
                            sourceSound.clip = engineSound;
                            sourceSound.Play();
                        }
                    }
                    else if(engineSound && newMovementSystem)
                    {
                        if (!sourceSound.isPlaying)
                        {
                            sourceSound.clip = engineSound;
                            sourceSound.Play();
                        }

                        if (leftWheelColliders[0].rpm > 0f)
                        {
                            sourceSound.pitch = (leftWheelColliders[0].rpm / maxWheelRPM) + 1;
                        }

                        if (leftWheelColliders[0].rpm <= 0f)
                        {
                            sourceSound.pitch = ((leftWheelColliders[0].rpm * -1) / maxWheelRPM) + 1;
                        }
                    }
                }
            }
            else
            {
                if (!oneEngineSound)
                {
                    if (idleEngineSound && sourceSound.clip != idleEngineSound)
                    {
                        sourceSound.clip = idleEngineSound;
                        sourceSound.Play();
                        sourceSound.pitch = 1;
                    }
                }
                else if (oneEngineSound)
                {
                    if (engineSound)
                    {
                        if (!sourceSound.isPlaying || sourceSound.clip != engineSound)
                        {
                            sourceSound.clip = engineSound;
                            sourceSound.Play();
                        }

                        if (sourceSound.pitch != 1)
                        {
                            sourceSound.pitch = 1;
                        }
                    }
                }
            }
        }

        protected virtual void HandleTurret()
        {
            if(tankTurretTransform)
            {
                //Vector3 aimVector = ReticlePosition - tankTurretTransform.position;
                //aimVector.y = 0.0f;
                //Quaternion newRotation = Quaternion.LookRotation(aimVector, transform.up);
                // newAngle = Quaternion.Slerp(tankTurretTransform.rotation, newRotation, Time.deltaTime * rotationSpeed);
                //tankTurretTransform.localEulerAngles = new Vector3(0, newAngle.eulerAngles.y, 0);
                //Quaternion.Slerp(tankTurretTransform.rotation, newRotation, Time.deltaTime * rotationSpeed)

                Vector3 horizontalPlanarDirection = Vector3.ProjectOnPlane(mousePosition - tankTurretTransform.position, tankTurretTransform.up);
                float horizontalAngleToTarget = Vector3.SignedAngle(tankTurretTransform.forward, horizontalPlanarDirection, tankTurretTransform.up);

                float finalAmount = Mathf.Clamp(Mathf.Abs(horizontalAngleToTarget), 0, rotationSpeed * Time.deltaTime) * Mathf.Sign(horizontalAngleToTarget);
                tankTurretTransform.RotateAround(tankTurretTransform.position, tankTurretTransform.up, finalAmount);
            }
        }
        protected virtual void HandleReticle()
        {
            Vector3 hit = ExtraFunctions.GetHitPosition(firePoint.position, firePoint.forward, shellVelocity);
            if (hit != null)
            {
                reticlePosition = hit;
            }

            if (reticleTransform)
            {
                reticleTransform.position = ReticlePosition;
            }
        }

        protected virtual void HandleHealth()
        {
            if(currentHitPoints <= 0)
            {
                StartCoroutine(DestoryTank());
            }
        }

        IEnumerator DestoryTank()
        {
            disableControls = true;
            if(explosionEffect)
            {
                Instantiate(explosionEffect, transform);
            }
            yield return new WaitForSeconds(1);
            gameObject.SetActive(false);
        }

        public virtual void HandleTakeDamage(float damage)
        {
            currentHitPoints = (currentHitPoints > 0) ? currentHitPoints - damage : 0;
        }

        protected virtual void HandleFireShot()
        {
            currentAmmo--;

            //Shooting Logic
            GameObject activeBullet = Instantiate(shellPrefab, firePoint.position, firePoint.rotation) as GameObject;

            Rigidbody rb = activeBullet.GetComponent<Rigidbody>();
            rb.AddForce(activeBullet.transform.forward * shellVelocity, ForceMode.Impulse);
            TankShell script = activeBullet.GetComponent<TankShell>();
            script.maxDamage = shellMaxDamage;
        }

        protected virtual void CalculateShellVelocity()
        {
            Vector3 direction = mousePosition - firePoint.position;
            float yOffset = -direction.y;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up);
            float distance = Mathf.Clamp(direction.magnitude, minimumShellDistance, maxShellDistance);
            float angle = Vector3.Angle(firePoint.forward, Vector3.up) * Mathf.Deg2Rad;

            shellVelocity = ExtraFunctions.CalculateLaunchSpeed(distance, yOffset, Physics.gravity.magnitude, angle);
        }

        IEnumerator Reload()
        {
            isReloading = true;
            reloadTimeLeft = reloadTime;
            reloadSlider.gameObject.SetActive(true);
            yield return new WaitForSeconds(reloadTime);

            currentAmmo = maxAmmo;
            isReloading = false;
            reloadSlider.gameObject.SetActive(false);
        }

        protected virtual void UpdateUI()
        {
            if(healthSlider)
            {
                healthSlider.value = currentHitPoints / maxHitPoints;
            }

            if(reloadSlider && reloadSlider.IsActive())
            {
                reloadSlider.value = reloadTimeLeft / reloadTime;
            }
        }

        protected virtual void UpdateWheelPoses()
        {
            for (int i = 0; i < leftWheelColliders.Length; i++)
            {
                UpdateWheelPose(leftWheelColliders[i], leftWheelTransforms[i], true);
            }

            for (int i = 0; i < rightWheelColliders.Length; i++)
            {
                UpdateWheelPose(rightWheelColliders[i], rightWheelTransforms[i], false);
            }
        }

        protected virtual void UpdateWheelPose(WheelCollider _collider, Transform _transform, bool leftWheel)
        {
            Vector3 _pos = _transform.position;
            Quaternion _quat = _transform.rotation;

            _collider.GetWorldPose(out _pos, out _quat);

            _transform.position = _pos;
            if (leftWheel)
            {
                _transform.rotation = _quat * leftWheelRotOffset;
            }
            else
            {
                _transform.rotation = _quat * rightWheelRotOffset;
            }
        }
        #endregion

        #region Input Callbacks
        protected virtual void InputFire(CallbackContext ctx)
        {
            if (!isReloading && Time.timeScale == 1 && !disableControls && firePoint && shellPrefab && timeSinceLastShot < Time.time)
            {
                HandleFireShot();
                weaponSourceSound.Play();
                timeSinceLastShot = Time.time + timeBetweenShots;
            }
        }
        #endregion
    }
}
