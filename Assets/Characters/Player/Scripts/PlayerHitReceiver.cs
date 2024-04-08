using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VERS;

namespace Player
{
    public class PlayerHitReceiver : MonoBehaviour, IDamagable
    {

        [SerializeField] private Vector3Reference m_HitPoint;
        [SerializeField] private Blackboard m_Blackboard;

        public void OnDamage()
        {
            int randomHitID = Random.Range(1, 5);
            m_Blackboard.animator.SetInteger("HitID", randomHitID);
            m_Blackboard.animator.SetTrigger("Hit");
            print(randomHitID);
        }
    }
}
