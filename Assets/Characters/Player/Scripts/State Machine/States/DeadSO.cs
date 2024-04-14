using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Dead", menuName = "Player/State/Dead")]
    public class DeadSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            board.animator.SetTrigger("Dead");

            board.walkCamera.enabled = false;
            board.runCamera.enabled = false;
            board.deadCamera.enabled = true;
        }

        public override void OnExit(Blackboard board)
        {

        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
