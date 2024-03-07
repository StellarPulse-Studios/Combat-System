using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Sprint Roll", menuName = "Player/State/Unarmed/Sprint Roll")]
    public class UnarmedSprintRollSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.animator.applyRootMotion = true;
            board.animator.SetTrigger("Roll");
        }

        public override void OnExit(Blackboard board)
        {
            board.animator.applyRootMotion = true;
            board.animator.applyRootMotion = false;
            board.isAnimationCompleted = false;
            board.isCrouched = false;
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
