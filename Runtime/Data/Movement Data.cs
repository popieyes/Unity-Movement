using UnityEngine;

namespace Kronos.Movement
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "Popieyes/Movement/Data")]
    public class MovementData : ScriptableObject
    {
        [Header("Movement Configuration")]
        public float MoveSpeed = 5f;
        public float Acceleration = 10f;
        public float Deceleration = 10f;
        public float RunMultiplier = 1.5f;
        public float AimMultiplier = 0.8f;
        [Header("Jump Configuration")]
        public float JumpForce = 7f;
        public float GravityScale = 1f;
        public float GroundSphereSize = 0.2f;
        public LayerMask GroundLayers;
   
    }
}
