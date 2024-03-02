using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New In Air Locomotion", menuName = "Player/State/Unarmed/In Air")]
    public class UnarmedInAirSO : StateSO
    {
        public override void OnEnter(Blackboard board)
        {
            Debug.Log("Unarmed In Air Enter...");
        }

        public override void OnExit(Blackboard board)
        {
            Debug.Log("Unarmed In Air Exit...");
        }

        public override void OnUpdate(Blackboard board)
        {
            board.Velocity.y += board.gravity * Time.deltaTime;
            board.characterController.Move(board.Velocity * Time.deltaTime);
        }
    }
}
