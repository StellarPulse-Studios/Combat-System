using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Dodge", menuName = "Player/State/Unarmed/Dodge")]
    public class UnarmedDodgeSO : StateSO
    {
        public bool snapToGround;

        public override void OnEnter(Blackboard board)
        {
            if (snapToGround)
                SnapToGround(board);

            board.animator.applyRootMotion = true;
            board.animator.SetTrigger("Roll");
        }

        public override void OnExit(Blackboard board)
        {
            board.animator.applyRootMotion = true;
            board.animator.applyRootMotion = false;
            board.isAnimationCompleted = false;
            board.isCrouched = false;
            board.jump = false;
            board.dodge = false;
        }

        public override void OnUpdate(Blackboard board)
        {

        }

        private void SnapToGround(Blackboard board)
        {
            Ray ray = new Ray(board.playerTransform.position + Vector3.up * 0.5f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, board.groundLayer))
            {
                Vector3 pos = board.playerTransform.position;
                float yOffset = hit.point.y - pos.y;
                board.characterController.Move(new Vector3(0.0f, yOffset, 0.0f));
            }
        }
    }
}
