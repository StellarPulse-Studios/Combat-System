using UnityEngine;

namespace Player
{
    public class Blackboard : MonoBehaviour
    {
        [Header("Physics")]
        public float gravity = -9.81f;
        public float acceleration = 10.0f;
        public float angularSpeed = 240.0f;
        public float walkSpeed = 2.0f;
        public float runSpeed = 4.0f;
        public float sprintSpeed = 6.0f;

        [Header("Ground Check")]
        public float groundCheckRadiusOffset = 0.01f;
        public float groundCheckYOffset = 0.0f;
        public LayerMask groundLayer;

        [Header("Components")]
        public Transform cameraTransform;
        public Transform playerTransform;
        public CharacterController characterController;
        public ExtendedCharacterController extendedCharacterController;
        public Animator animator;

        [Header("Inputs")]
        public Vector2 move;
        public Vector2 look;
        public bool sprint;
        public bool jump;

        [Header("State Variables")]
        public bool isGrounded;

        [Header("Debug Variables")]
        public Vector3 Velocity;
        public float CurrentSpeed;
    }
}