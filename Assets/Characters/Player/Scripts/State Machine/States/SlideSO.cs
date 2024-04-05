using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Slide", menuName = "Player/State/Slide")]
    public class SlideSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.move = Vector2.zero;
            board.PreviousVelocity = Vector3.zero;
            board.animator.SetFloat("MoveSpeed", 0.0f);
        }

        public override void OnExit(Blackboard board)
        {
            board.move = Vector2.zero;
        }

        public override void OnUpdate(Blackboard board)
        {
            Vector3 moveDir = Vector3.ProjectOnPlane(Vector3.down, board.slideNormal).normalized;

            board.playerTransform.forward = new Vector3(moveDir.x, 0.0f, moveDir.z).normalized;

            board.Velocity = moveDir * board.slideSpeed;
            //Vector3 motion = board.extendedCharacterController.GetStepDownMotion(board.Velocity * Time.deltaTime);
            //board.characterController.Move(motion);
            board.characterController.Move(board.Velocity * Time.deltaTime);
        }
    }
}
