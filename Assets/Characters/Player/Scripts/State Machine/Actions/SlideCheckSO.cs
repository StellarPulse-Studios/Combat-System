using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Slide Check", menuName = "Player/Action/Slide Check")]
    public class SlideCheckSO : ActionSO
    {
        public float maxSlideCheckDistance = 1.0f;

        public override void Evaluate(Blackboard board)
        {
            if (Physics.Raycast(board.playerTransform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, maxSlideCheckDistance))
            {
                float angle = Vector3.Angle(hitInfo.normal, Vector3.up);
                if (angle >= board.characterController.slopeLimit)
                {
                    board.isSliding = true;
                    board.slideNormal = hitInfo.normal;
                    board.Velocity = Vector3.zero;
                    return;
                }
            }

            board.isSliding = false;
        }
    }
}
