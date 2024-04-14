using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VERS;

namespace Player
{
    public class PlayerHitReceiver : MonoBehaviour, IDamagable
    {

        [SerializeField] private Vector3Reference m_HitPoint;
        [SerializeField] private Blackboard m_Blackboard;
        [SerializeField] private UnityEvent<float> m_OnDamageReceived;

        public void OnDamage(float damage)
        {
            m_Blackboard.gotHit = true;

            m_OnDamageReceived?.Invoke(damage);
        }
    }
}
