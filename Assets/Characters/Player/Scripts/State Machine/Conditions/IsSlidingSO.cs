using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Is Sliding", menuName = "Player/Condition/Is Sliding")]
    public class IsSlidingSO : ConditionSO
    {
        public bool compareValue;

        public override bool Evaluate(Blackboard board)
        {
            return board.isSliding == compareValue;
        }
    }
}
