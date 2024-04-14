using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Heavy Attack", menuName = "Player/State/Armed/Heavy Attack")]
    public class HeavyAttackSO : StateSO
    {
        private GameObject[] m_Enemies;

        public override void OnEnter(Blackboard board)
        {
            board.PreviousVelocity = Vector3.zero;
            board.PreviousSpeed = 0.0f;
            board.attack = false;
            board.heavyAttack = false;
            board.animator.SetTrigger("HeavyAttack");

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
            board.hasStartedCharging = false;
            board.isCharging = false;
            board.isCharged = false;
            board.animator.SetFloat("ChargeMultiplier", 1.0f);
        }

        public override void OnUpdate(Blackboard board)
        {
            if (board.heavyAttack)
            {
                board.heavyAttack = false;
                board.animator.SetTrigger("HeavyAttack");

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

            //if (board.isCharged)
            //{
            //    if (!board.isCharging)
            //    {
            //        board.isCharged = false;
            //        board.animator.SetFloat("ChargeMultiplier", 1.0f);
            //    }
            //}

            if (board.hasStartedCharging)
            {
                if (!board.isCharging)
                {
                    board.hasStartedCharging = false;
                    board.animator.SetFloat("ChargeMultiplier", 1.0f);
                }
            }
        }
    }
}
