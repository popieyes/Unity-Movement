using UnityEngine;
using Popieyes.Input;
using UnityEditor.Callbacks;

namespace Popieyes.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(InputProcessor))]
    public class Movement : MonoBehaviour
    {
        #region Variables
        // Configurable fields, private references, etc.
        [SerializeField] MovementData _movementData;
        InputProcessor _inputProcessor;
        Rigidbody _rb;
        float movementSpeed;
        Camera mainCamera;
        [SerializeField] Transform body; 
        
        #endregion

        #region Events
        // Actions and Delegates
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _inputProcessor = GetComponent<InputProcessor>();
            mainCamera = Camera.main;

            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        void Start()
        {
            _inputProcessor.OnSprintPerformed += Sprint;
            _inputProcessor.OnSprintCanceled += Walk;
            _inputProcessor.OnCrouchPerformed += Crouch;
            _inputProcessor.OnCrouchCanceled += StandUp;
            _inputProcessor.OnJumpPerformed += Jump;
            movementSpeed = _movementData.MoveSpeed;
        }

        void Update()
        {
            
        }

        void FixedUpdate()
        {
            Move();
        }

        void LateUpdate()
        {
            
        }
        #endregion

        #region Custom Functions
        void Move()
        {
            Vector3 _inputDirection = new Vector3(_inputProcessor.Input.x, 0, _inputProcessor.Input.y).normalized;
            
            if(_inputDirection.sqrMagnitude > 0.1f) Accelerate(_inputDirection);
            else Decelerate();
         
        }
        void Accelerate(Vector3 dir)
        {
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 desiredDirection = (forward * dir.z + right * dir.x).normalized;
            
            float targetAngle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(body.eulerAngles.y, targetAngle, ref _movementData.TurnSmoothing, 0.1f);
            body.rotation = Quaternion.Euler(0f,angle,0f); 

            Vector3 targetVelocity = desiredDirection * movementSpeed;
            targetVelocity.y = _rb.linearVelocity.y;
            _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, targetVelocity, _movementData.Acceleration * Time.fixedDeltaTime);
        }
        void Decelerate()
        {
            Vector3 stopVelocity = new Vector3(0,_rb.linearVelocity.y,0);
            _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, stopVelocity, _movementData.Deceleration * Time.fixedDeltaTime);
        }  
        void Sprint()
        {
            movementSpeed *= _movementData.RunMultiplier;
        }

        void Walk()
        {
            movementSpeed = _movementData.MoveSpeed;
        }
        void Crouch()
        {
            transform.localScale = new Vector3(1f,0.5f,1f);
        }
        void StandUp()
        {
            transform.localScale = new Vector3(1f,1f,1f);
        }
        void Jump()
        {
            _rb.linearVelocity += new Vector3(0f,_movementData.JumpForce,0f);
        }
        #endregion
    }
}