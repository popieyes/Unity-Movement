using UnityEngine;

namespace Popieyes.Movement
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Popieyes/Movement/CameraSettings")]
    public class CameraSettings : ScriptableObject
    {
        public float MouseSensitivity = 100f;
        public float MinY = -90f;
        public float MaxY = 90f;

    }
}
