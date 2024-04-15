using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VERS;

namespace Player
{
    [CreateAssetMenu(fileName = "New Dead", menuName = "Player/State/Dead")]
    public class DeadSO : StateSO
    {
        public BoolReference isPlayerDead;
        public GameEventSO deadEvent;

        public override void OnEnter(Blackboard board)
        {
            isPlayerDead.Value = true;

            board.animator.SetTrigger("Dead");

            board.walkCamera.enabled = false;
            board.runCamera.enabled = false;
            board.deadCamera.enabled = true;
            board.armedCamera.enabled = false;

            if (deadEvent)
                deadEvent.Raise();
        }

        public override void OnExit(Blackboard board)
        {
            isPlayerDead.Value = false;
        }

        public override void OnUpdate(Blackboard board)
        {

        }
    }
}
