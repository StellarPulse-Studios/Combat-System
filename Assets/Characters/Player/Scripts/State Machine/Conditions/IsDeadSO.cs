using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Dead", menuName = "Player/Condition/Is Dead")]
    public class IsDeadSO : ConditionSO
    {
        public bool compareValue;

        public override bool Evaluate(Blackboard board)
        {
            return board.isDead == compareValue;
        }
    }
}
