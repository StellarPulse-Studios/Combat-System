using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Unarmed Land", menuName = "Player/State/Unarmed/Land")]
    public class UnarmedLandSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.isAnimationCompleted = false;
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationCompleted = false;
            board.PreviousVelocity = Vector3.zero;
            board.Velocity = Vector3.zero;
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
