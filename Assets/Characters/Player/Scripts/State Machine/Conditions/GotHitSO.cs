using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Got Hit", menuName = "Player/Condition/Got Hit")]
    public class GotHitSO : ConditionSO
    {
        public bool comparedValue;

        public override bool Evaluate(Blackboard board)
        {
            return board.gotHit == comparedValue;
        }
    }
}
