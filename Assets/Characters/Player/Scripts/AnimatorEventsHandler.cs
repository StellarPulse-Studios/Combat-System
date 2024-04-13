using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VERS;

namespace Player
{
    public class AnimatorEventsHandler : MonoBehaviour
    {
        public float m_ChargeMultiplier = 0.1f;
        public BoxCollider m_BoxCollider;
        public Blackboard m_Blackboard;
        public LayerMask m_HitBoxLayer;
        public float m_MoveSpeed = 2.0f;
        public AnimationCurve m_MoveSpeedCurve;
        public Vector3Reference m_HitPoint;
        public GameEventSO m_HitEvent;
        public IntReference m_AttackID;

        private HashSet<Collider> m_ColliderSet;
        private bool m_IsBoxCasting;
        private bool m_CanMoveTowardTarget;
        private Vector3 m_GizmosHitPoint;
        private Vector3 m_PreviousHitBoxPosition;

        private void Start()
        {
            m_ColliderSet = new HashSet<Collider>();
            m_IsBoxCasting = false;
        }

        public void EnableHitBox()
        {
            m_ColliderSet.Clear();
            m_IsBoxCasting = true;
            m_PreviousHitBoxPosition = m_BoxCollider.transform.TransformPoint(m_BoxCollider.center); ;
        }

        public void DisableHitBox()
        {
            m_IsBoxCasting = false;
        }

        public void EnableCharging()
        {
            m_Blackboard.hasStartedCharging = true;

            if (m_Blackboard.isCharging)
            {
                m_Blackboard.animator.SetFloat("ChargeMultiplier", m_ChargeMultiplier);
            }
        }

        public void DisableCharging()
        {
            m_Blackboard.hasStartedCharging = false;

            if (m_Blackboard.isCharging)
            {
                m_Blackboard.isCharged = true;
            }

            m_Blackboard.animator.SetFloat("ChargeMultiplier", 1.0f);
        }

        private float m_MoveSpeedTime;

        public void EnableMoving()
        {
            m_MoveSpeedTime = 0.0f;
            m_CanMoveTowardTarget = true;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        public void DisableMoving()
        {
            m_CanMoveTowardTarget = false;
        }

        private void MoveTowardTarget()
        {
            if (m_Blackboard.closestEnemy && Vector3.Distance(m_Blackboard.playerTransform.position, m_Blackboard.closestEnemy.position) <= m_Blackboard.enemyMinRangeThreshold)
                return;

            float moveSpeed = m_MoveSpeed * m_MoveSpeedCurve.Evaluate(m_MoveSpeedTime * 10.0f);
            m_MoveSpeedTime += Time.deltaTime;

            Vector3 motion = m_Blackboard.extendedCharacterController.GetStepDownMotion(Time.deltaTime * moveSpeed * m_Blackboard.transform.forward);
            m_Blackboard.characterController.Move(motion);
        }

        private void Update()
        {
            if (m_CanMoveTowardTarget)
            {
                MoveTowardTarget();
            }

            if (m_IsBoxCasting)
            {
                DoDamage();
            }
        }

        private void DoDamage()
        {
            Vector3 center = m_BoxCollider.transform.TransformPoint(m_BoxCollider.center);
            Collider[] colliders = Physics.OverlapBox(center, m_BoxCollider.size * 0.5f, m_BoxCollider.transform.rotation, m_HitBoxLayer);

            foreach (Collider collider in colliders)
            {
                if (m_ColliderSet.Contains(collider))
                    continue;

                m_ColliderSet.Add(collider);

                m_HitPoint.Value = collider.ClosestPoint(center);

                Vector3 boxCastOffset = center - m_PreviousHitBoxPosition;
                Vector3 boxCastDirection = boxCastOffset.normalized;
                if (Physics.BoxCast(m_PreviousHitBoxPosition, m_BoxCollider.size * 0.5f, boxCastDirection, out RaycastHit hitInfo, m_BoxCollider.transform.rotation, boxCastOffset.magnitude, m_HitBoxLayer))
                {
                    m_HitPoint.Value = hitInfo.point;
                    //UnityEditor.EditorApplication.isPaused = true;
                }

                if (m_AttackID.Value == 2)
                {
                    m_HitPoint.Value = collider.ClosestPoint(m_Blackboard.playerTransform.position + Vector3.up);
                }

                m_GizmosHitPoint = m_HitPoint.Value;

                if (m_HitEvent)
                    m_HitEvent.Raise();

                if (collider.TryGetComponent(out IDamagable damagable))
                {
                    damagable.OnDamage(10.0f);
                }
            }

            m_PreviousHitBoxPosition = center;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
            Matrix4x4 prevMatrix = Gizmos.matrix;
            Gizmos.matrix = m_BoxCollider.transform.localToWorldMatrix;
            Gizmos.DrawCube(m_BoxCollider.center, m_BoxCollider.size);
            Gizmos.matrix = prevMatrix;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(m_GizmosHitPoint, 0.1f);
        }
    }
}
