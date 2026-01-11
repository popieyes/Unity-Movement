using UnityEngine;



namespace Kronos.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class MovementController : MonoBehaviour
    {
        #region Variables
        // Configurable fields, private references, etc.
        [Header("Configuration")]
        [SerializeField] private MovementData _data;
        public MovementData Data => _data;
        [Header("References")]
        [SerializeField] Transform _orientation;
        [SerializeField] Transform body;
        [Header("Rotation Settings")]
        [SerializeField] bool _turnBodyWithAimDirection = true; 
        Rigidbody _rb;

        public float Speed => _rb.linearVelocity.magnitude;
        public float NormalizedSpeed => Mathf.Clamp01((_rb.linearVelocity.magnitude - Data.MoveSpeed) / 
        (Data.MoveSpeed * Data.RunMultiplier  - Data.MoveSpeed));
        
        float TurnSmoothing = 0.1f;
        private float _currentSpeed; 
        #endregion

        #region Events
        // Actions and Delegates
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        
            _currentSpeed = Data.MoveSpeed;
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        #endregion

        #region Custom Functions
        public void Move(Vector2 _inputDir, bool useCamera = true)
        {
            Vector3 _inputDirection = new Vector3(_inputDir.x, 0, _inputDir.y).normalized;
            
            if(_inputDirection.sqrMagnitude > 0.1f) Accelerate(_inputDirection, useCamera);
            else Decelerate();
           /*  if(_turnBodyWithAimDirection) RotateBody(_inputDir);
            else if(_inputDirection.sqrMagnitude > 0.1f) RotateBody(_inputDir); */
         
        }
        public void Accelerate(Vector3 dir, bool useCamera)
        {
            Vector3 forward = _orientation != null ? _orientation.forward : Camera.main.transform.forward;
            Vector3 right = _orientation != null ? _orientation.right : Camera.main.transform.right;
            forward.y = 0;
            right.y = 0;

            Vector3 desiredDirection = useCamera ? (forward * dir.z + right * dir.x).normalized : dir.normalized;
            float targetAngle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(body.eulerAngles.y, targetAngle, ref TurnSmoothing, 0.1f);
            body.rotation = Quaternion.Euler(0f,angle,0f); 

            Vector3 targetVelocity = desiredDirection * _currentSpeed;
            Vector3 velocityChange = targetVelocity - _rb.linearVelocity;
            velocityChange.y = 0f;
            _rb.AddForce(velocityChange * Data.Acceleration, ForceMode.Acceleration);
        }
        public void Decelerate()
        {
            Vector3 stopVelocity = new Vector3(0,_rb.linearVelocity.y,0);
            _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, stopVelocity, Data.Deceleration * Time.fixedDeltaTime);
        } 
        private void RotateBody(Vector3 dir)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0;
            right.y = 0;

            Vector3 desiredDirection = (forward * dir.z + right * dir.x).normalized;

            float targetAngle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(body.eulerAngles.y, targetAngle, ref TurnSmoothing, 0.1f);
            body.rotation = Quaternion.Euler(0f,angle,0f); 
        }
        public void RotateBody()
        {
            RotateBody(Vector3.one);
        }
        public void Crouch()
        {
            
        }
        public void StandUp()
        {
           
        }

        public bool IsGrounded() => Physics.CheckSphere(transform.position, Data.GroundSphereSize, Data.GroundLayers);
        public void Jump()
        {
            if(IsGrounded())
                _rb.AddForce(Vector3.up * Data.JumpForce, ForceMode.VelocityChange);
        }
        public void SetSpeed(float targetSpeed){_currentSpeed = targetSpeed;}
        #endregion
    }
}