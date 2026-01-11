
using UnityEngine;
using Kronos.Input;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Kronos.Core.Data;

namespace Popieyes.Movement
{
    [RequireComponent(typeof(InputSystem_ActionsProcessor))]
    public class CameraFPS : MonoBehaviour
    {
        #region Variables
        [SerializeField] bool ExecuteLocally = true;
        [SerializeField] Transform _head;
        [SerializeField] CameraSettings _cameraSettings;
        InputSystem_ActionsProcessor _inputProcessor;
       
        CinemachineCamera _cinemachineCam;
        public CinemachineCamera CinemachineCam => _cinemachineCam;


        float _mouseX, _mouseY;
        #endregion

        #region Events
        // Actions and Delegates
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            _inputProcessor = GetComponent<InputSystem_ActionsProcessor>();
        }

        void Start()
        {
            if(!ExecuteLocally) return;
            SpawnCinemachineCam();
        }

        void Update()
        {
            _mouseX += _inputProcessor.Look.x * _cameraSettings.CurrentMouseSensitivity * Time.deltaTime;
            _mouseY -= _inputProcessor.Look.y * _cameraSettings.CurrentMouseSensitivity * Time.deltaTime;
            _mouseY = Mathf.Clamp(_mouseY, _cameraSettings.MinY, _cameraSettings.MaxY);
/*             transform.localRotation = Quaternion.Euler(0f, _mouseX, 0f); */
            _head.transform.localRotation = Quaternion.Euler(_mouseY, _mouseX, 0f);
            if(SceneManager.GetActiveScene().name != "Main Menu")
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void FixedUpdate()
        {

        }

        void LateUpdate()
        {
            
        }
        #endregion

        #region Custom Functions
        public void SpawnCinemachineCam ()
        {
            var cinemachineCam = Instantiate(new GameObject("Cinemachine Camera"), transform.position, Quaternion.identity);
            _cinemachineCam = cinemachineCam.AddComponent<CinemachineCamera>();
            _cinemachineCam.Lens.FieldOfView = 75f;
            var target = new CameraTarget();
            target.TrackingTarget = _head;
            _cinemachineCam.Target = target;
            
            var follow = cinemachineCam.AddComponent<CinemachineThirdPersonFollow>();
            follow.ShoulderOffset = _cameraSettings.ShoulderOffset;
            follow.CameraDistance = _cameraSettings.CameraDistance;
            follow.CameraSide = _cameraSettings.CameraSide;
            cinemachineCam.AddComponent<CinemachineImpulseListener>();
            DontDestroyOnLoad(cinemachineCam);
        }
        #endregion
    }
}