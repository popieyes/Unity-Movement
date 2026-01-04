using UnityEngine;
using Popieyes.Input;


namespace Popieyes.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Movement : MonoBehaviour
    {
        #region Variables
        // Configurable fields, private references, etc.
        [Header("Configuration")]
        [SerializeField] MovementData _movementData;
        [Header("References")]
        [SerializeField] Transform body;
        [Header("Settings")]
        [SerializeField] bool _turnBodyWithAimDirection = true; 
        Rigidbody _rb;
        Collider _collider;
        public float Speed => _rb.linearVelocity.magnitude;
        bool IsSprinting = false;
        Camera mainCamera;
        
        #endregion

        #region Events
        // Actions and Delegates
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            
            mainCamera = Camera.main;

            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        #endregion

        #region Custom Functions
        void Move(Vector2 _inputDir)
        {
            Vector3 _inputDirection = new Vector3(_inputDir.x, 0, _inputDir.y).normalized;
            
            if(_inputDirection.sqrMagnitude > 0.1f) Accelerate(_inputDirection);
            else Decelerate();
            if(_turnBodyWithAimDirection) RotateBody();
            else if(_inputDirection.sqrMagnitude > 0.1f) RotateBody();
         
        }
        void Accelerate(Vector3 dir)
        {
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;
            forward.y = 0;
            right.y = 0;

            Vector3 desiredDirection = (forward * dir.z + right * dir.x).normalized;

            Vector3 targetVelocity = IsSprinting ? desiredDirection * (_movementData.MoveSpeed * _movementData.RunMultiplier) : desiredDirection * _movementData.MoveSpeed;
            targetVelocity.y = _rb.linearVelocity.y;
            _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, targetVelocity, _movementData.Acceleration * Time.fixedDeltaTime);
        }
        void Decelerate()
        {
            Vector3 stopVelocity = new Vector3(0,_rb.linearVelocity.y,0);
            _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, stopVelocity, _movementData.Deceleration * Time.fixedDeltaTime);
        } 
        void RotateBody()
        {
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;
            forward.y = 0;
            right.y = 0;

            Vector3 desiredDirection = (forward + right).normalized;

            float targetAngle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(body.eulerAngles.y, targetAngle, ref _movementData.TurnSmoothing, 0.1f);
            body.rotation = Quaternion.Euler(0f,angle,0f); 
        }
        void Sprint()
        {
            IsSprinting = true;
        }

        void Walk()
        {
            IsSprinting = false;
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