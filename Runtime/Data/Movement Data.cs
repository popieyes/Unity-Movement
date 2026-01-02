using UnityEngine;

namespace Popieyes.Movement
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "Popieyes/Movement/Data")]
    public class MovementData : ScriptableObject
    {
        public float MoveSpeed = 5f;
        public float JumpForce = 7f;
        public float GravityScale = 1f;
        public float Acceleration = 10f;
        public float Deceleration = 10f;
        public float RunMultiplier = 1.5f;
    }
}
