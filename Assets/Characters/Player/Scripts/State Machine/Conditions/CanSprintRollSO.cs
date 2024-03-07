using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Can Sprint Roll", menuName = "Player/Condition/Can Sprint Roll")]
    public class CanSprintRollSO : ConditionSO
    {
        public override bool Evaluate(Blackboard board)
        {
            return board.sprint && board.isCrouched;
        }
    }
}
