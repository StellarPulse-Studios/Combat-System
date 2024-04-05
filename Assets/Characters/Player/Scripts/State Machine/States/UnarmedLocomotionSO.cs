using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Locomotion", menuName = "Player/State/Unarmed/Locomotion")]
    public class UnarmedLocomotionSO : StateSO
    {
        public bool defaultRunning = true;

        public override void OnEnter(Blackboard board)
        {
            board.fallingTime = 0.0f;
            board.Velocity = board.PreviousVelocity;
            board.isAnimationCompleted = false;
            board.animator.SetBool("IsArmed", false);
            board.animator.SetBool("IsGrounded", true);
            board.animator.SetFloat("MoveSpeed", board.Velocity.magnitude);

            // Weapons
            board.swordInHand.SetActive(false);
            board.shieldInHand.SetActive(false);
        }

        public override void OnExit(Blackboard board)
        {
            board.PreviousVelocity = board.Velocity;
            board.PreviousSpeed = board.PreviousVelocity.magnitude;
        }

        public override void OnUpdate(Blackboard board)
        {
            // It may happen that animation exit event call later, thus we are checking for completion status and resetting it.
            if (board.isAnimationCompleted)
                board.isAnimationCompleted = false;

            UpdateMovement(board);
        }

        private void UpdateMovement(Blackboard board)
        {
            Vector2 moveInput = board.move;
            float moveInputMagnitude = moveInput.magnitude;
            Vector3 moveDir = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
            moveInputMagnitude = Mathf.Clamp01(moveInputMagnitude);

            Vector3 currentVelocity = board.Velocity;
            float currentSpeed = currentVelocity.magnitude;
            float targetSpeed = GetTargetSpeed(board, moveInputMagnitude);

            board.CurrentSpeed = currentSpeed;

            currentSpeed = Accelerate(currentSpeed, targetSpeed, board);

            UpdateRotation(moveDir, board);
            UpdateSliding(board);

            moveDir = Quaternion.Euler(0.0f, board.playerTransform.eulerAngles.y, 0.0f) * Vector3.forward;

            board.Velocity = currentSpeed * moveDir;

            Vector3 stepDownMotion = board.extendedCharacterController.GetStepDownMotion(board.Velocity * Time.deltaTime);
            Vector3 slopeMotion = board.extendedCharacterController.GetSlopeMotion(stepDownMotion);

            //board.characterController.Move(stepDownMotion);
            board.characterController.Move(slopeMotion);

            board.animator.SetFloat("MoveSpeed", currentSpeed);
        }

        public override void DrawGizmos(Blackboard board)
        {
            base.DrawGizmos(board);

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(board.playerTransform.position, board.Velocity);
        }

        private float GetTargetSpeed(Blackboard board, float inputMagnitude)
        {
            if (board.move == Vector2.zero)
                return 0.0f;

            float speed = defaultRunning ? board.runSpeed : board.walkSpeed;

            speed *= (inputMagnitude <= 0.708) ? 0.5f : 1.0f;

            if (board.sprint)
                speed = board.sprintSpeed;

            return speed;
        }

        private float Accelerate(float currentSpeed, float targetSpeed, Blackboard board)
        {
            const float threshold = 0.1f;

            // acceleration and deceleration
            if (currentSpeed < targetSpeed - threshold || currentSpeed > targetSpeed + threshold)
            {
                currentSpeed += Mathf.Sign(targetSpeed - currentSpeed) * board.acceleration * Time.deltaTime;
                currentSpeed = Mathf.Round(currentSpeed * 1000.0f) * 0.001f;
            }
            else
            {
                currentSpeed = targetSpeed;
            }

            return currentSpeed;
        }

        private void UpdateRotation(Vector3 moveDir, Blackboard board)
        {
            if (board.move == Vector2.zero)
                return;

            float targetYRot = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + board.cameraTransform.eulerAngles.y;
            Quaternion targetRot = Quaternion.Euler(0.0f, targetYRot, 0.0f);
            board.playerTransform.rotation = Quaternion.RotateTowards(board.playerTransform.rotation, targetRot, board.angularSpeed * Time.deltaTime);
        }

        private void UpdateSliding(Blackboard board)
        {
            if (Physics.Raycast(board.playerTransform.position + Vector3.up, Vector3.down, out RaycastHit hit, 1.0f))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                if (angle >= board.characterController.slopeLimit)
                {
                    board.isSliding = true;

                }
            }
        }
    }
}
