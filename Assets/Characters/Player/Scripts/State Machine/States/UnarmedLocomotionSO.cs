using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Locomotion", menuName = "Player/State/Unarmed/Locomotion")]
    public class UnarmedLocomotionSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {

        }

        public override void OnExit(Blackboard board)
        {

        }

        public override void OnUpdate(Blackboard board)
        {
            UpdateMovement(board);
        }

        private void UpdateMovement(Blackboard board)
        {
            Vector2 moveInput = board.move;
            float moveInputMagnitude = moveInput.magnitude;
            Vector3 moveDir = (moveInputMagnitude > 1E-05f) ? new Vector3(moveInput.x, 0.0f, moveInput.y) / moveInputMagnitude : Vector3.zero;
            moveInputMagnitude = Mathf.Clamp01(moveInputMagnitude);

            Vector3 currentVelocity = board.Velocity;
            float currentSpeed = currentVelocity.magnitude;
            float targetSpeed = GetTargetSpeed(board);

            board.CurrentSpeed = currentSpeed;

            currentSpeed = Accelerate(currentSpeed, targetSpeed, board);

            UpdateRotation(moveDir, board);

            moveDir = Quaternion.Euler(0.0f, board.playerTransform.eulerAngles.y, 0.0f) * Vector3.forward;

            if (Mathf.Approximately(moveInputMagnitude, 0.0f) && currentSpeed > 0.0f)
                moveInputMagnitude = 1.0f;

            float moveSpeed = moveInputMagnitude * currentSpeed;
            board.Velocity = moveSpeed * moveDir;

            Vector3 stepDownMotion = board.extendedCharacterController.GetStepDownMotion(board.Velocity * Time.deltaTime);

            board.characterController.Move(stepDownMotion);

            board.animator.SetFloat("MoveSpeed", moveSpeed);
        }

        private float GetTargetSpeed(Blackboard board)
        {
            if (board.move == Vector2.zero)
                return 0.0f;

            float speed = board.runSpeed;

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
    }
}
