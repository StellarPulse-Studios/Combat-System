using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Hit", menuName = "Player/State/Armed/Hit")]
    public class HitSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.gotHit = false;

            int randomHitID = Random.Range(1, 5);
            board.animator.SetInteger("HitID", randomHitID);
            board.animator.SetTrigger("Hit");
        }

        public override void OnExit(Blackboard board)
        {
            board.gotHit = false;
            board.isAnimationStateMachineExited = false;
        }

        public override void OnUpdate(Blackboard board)
        {
            if (board.gotHit)
            {
                board.gotHit = false;

                int randomHitID = Random.Range(1, 5);
                board.animator.SetInteger("HitID", randomHitID);
                board.animator.SetTrigger("Hit");
            }
        }
    }
}
