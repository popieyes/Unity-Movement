using UnityEngine;
using Popieyes.Input;

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
        #endregion

        #region Events
        // Actions and Delegates
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _inputProcessor = GetComponent<InputProcessor>();

        }

        void Start()
        {
            _inputProcessor.OnSprintPerformed += Sprint;
            _inputProcessor.OnSprintCancelled += Walk;

            movementSpeed = _movementData.MoveSpeed;
        }

        void Update()
        {
            
        }

        void FixedUpdate()
        {
            Vector3 _inputDirection = new Vector3(_inputProcessor.Input.x, 0, _inputProcessor.Input.y).normalized;
            _rb.linearVelocity = transform.forward * _inputDirection.z * movementSpeed + transform.right * _inputDirection.x * movementSpeed;
        }

        void LateUpdate()
        {
            
        }
        #endregion

        #region Custom Functions
        void Sprint()
        {
            movementSpeed *= _movementData.RunMultiplier;
        }

        void Walk()
        {
            movementSpeed = _movementData.MoveSpeed;
        }
        #endregion
    }
}