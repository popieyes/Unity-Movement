
using UnityEngine;
using Popieyes.Input;

namespace Popieyes.Movement
{
    [RequireComponent(typeof(InputProcessor))]
    public class CameraFPS : MonoBehaviour
    {
        #region Variables
        [SerializeField] Transform _head;
        [SerializeField] CameraSettings _cameraSettings;
        InputProcessor _inputProcessor;
        Camera _camera;
        

        float _mouseX, _mouseY;
        #endregion

        #region Events
        // Actions and Delegates
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            _inputProcessor = GetComponent<InputProcessor>();
            _camera = Camera.main;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Start()
        {
            
        }

        void Update()
        {
            _mouseX += _inputProcessor.Look.x * _cameraSettings.MouseSensitivity * Time.deltaTime;
            _mouseY -= _inputProcessor.Look.y * _cameraSettings.MouseSensitivity * Time.deltaTime;
            _mouseY = Mathf.Clamp(_mouseY, _cameraSettings.MinY, _cameraSettings.MaxY);
            transform.localRotation = Quaternion.Euler(0f, _mouseX, 0f);
            _head.transform.localRotation = Quaternion.Euler(_mouseY, 0f, 0f);
        }

        void FixedUpdate()
        {

        }

        void LateUpdate()
        {
            
        }
        #endregion

        #region Custom Functions
        // Your logic here
        #endregion
    }
}