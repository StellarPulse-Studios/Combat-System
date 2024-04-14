using System.Collections.Generic;
using UnityEngine;
using VERS;

namespace Player
{
    [CreateAssetMenu(fileName = "New Light Attack", menuName = "Player/State/Armed/Light Attack")]
    public class LightAttackSO : StateSO
    {
        [SerializeField] private GameEventSO m_HitEvent;

        private GameObject[] m_Enemies;

        public override void OnEnter(Blackboard board)
        {
            board.PreviousVelocity = Vector3.zero;
            board.PreviousSpeed = 0.0f;
            board.attack = false;
            board.lightAttack = false;
            board.animator.SetTrigger("LightAttack");

            m_Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            List<GameObject> enemies = new List<GameObject>();
            foreach (var enemy in m_Enemies)
            {
                if (enemy.TryGetComponent(out Collider col))
                    enemies.Add(enemy);
            }
            m_Enemies = enemies.ToArray();

            GameObject closestEnemy = null;
            float minDistanceFromPlayer = float.MaxValue;

            foreach (GameObject enemy in m_Enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, board.playerTransform.position);
                if (distance < minDistanceFromPlayer)
                {
                    minDistanceFromPlayer = distance;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null && minDistanceFromPlayer <= board.enemyMaxRangeThreshold)
            {
                board.closestEnemy = closestEnemy.transform;

                Vector3 enemyDir = closestEnemy.transform.position - board.playerTransform.position;
                enemyDir.y = 0.0f;
                enemyDir.Normalize();
                board.playerTransform.rotation = Quaternion.LookRotation(enemyDir);
            }
            else
            {
                board.closestEnemy = null;
            }
        }

        public override void OnExit(Blackboard board)
        {
            board.isAnimationStateMachineExited = false;
        }

        public override void OnUpdate(Blackboard board)
        {
            if (board.lightAttack)
            {
                GameObject closestEnemy = null;
                float minDistanceFromPlayer = float.MaxValue;

                foreach (GameObject enemy in m_Enemies)
                {
                    float distance = Vector3.Distance(enemy.transform.position, board.playerTransform.position);
                    if (distance < minDistanceFromPlayer)
                    {
                        minDistanceFromPlayer = distance;
                        closestEnemy = enemy;
                    }
                }

                if (closestEnemy != null && minDistanceFromPlayer <= board.enemyMaxRangeThreshold)
                {
                    board.closestEnemy = closestEnemy.transform;

                    Vector3 enemyDir = closestEnemy.transform.position - board.playerTransform.position;
                    enemyDir.y = 0.0f;
                    enemyDir.Normalize();
                    board.playerTransform.rotation = Quaternion.LookRotation(enemyDir);
                }
                else
                {
                    board.closestEnemy = null;
                }

                board.lightAttack = false;
                board.animator.SetTrigger("LightAttack");
            }
        }

        public override void DrawGizmos(Blackboard board)
        {
            base.DrawGizmos(board);

            Gizmos.color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
            Gizmos.DrawWireSphere(board.playerTransform.position, board.enemyMaxRangeThreshold);

            UnityEditor.Handles.color = new Color(1.0f, 0.0f, 1.0f, 0.05f);
            UnityEditor.Handles.DrawSolidArc(board.playerTransform.position, Vector3.up, board.playerTransform.forward, 360.0f, board.enemyMaxRangeThreshold);
        }
    }
}
