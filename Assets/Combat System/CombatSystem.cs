using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VERS;

namespace Combat
{
    public class CombatSystem : MonoBehaviour
    {
        // System ta akta enemy k randomly choose korbe

        // And hotochara ta k akta token debe....

        // Token ta niye suor ta attack korbe player k

        // Tarpor, token ta return kore debe system k

        // System sei used token ta k cooldown er jonne rakhbe

        // Jokhun cooldown time ta ses hoe jabe... tokhun token ta abar usuable hoe jabe

        // And abr sei token ta j kono akta random enemy use korte parbe

        [SerializeField] private int m_StartingToken = 1;
        [SerializeField] private IntReference m_AvailableTokens;
        [SerializeField] private float m_TokenCooldownTime = 5.0f;

        private List<float> m_CooldownTokens;

        private void Start()
        {
            m_CooldownTokens = new List<float>();
            m_AvailableTokens.Value = m_StartingToken;
        }

        private void Update()
        {
            UpdateCooldownTokens();
        }

        public void GiveTokenToEnemy()
        {
            if (m_AvailableTokens.Value <= 0)
            {
                Debug.LogWarning("No tokens are available");
                return;
            }

            m_AvailableTokens.Value -= 1;
        }

        public void ReturnTokenToSystem()
        {
            if (m_AvailableTokens.Value >= m_StartingToken)
                return;

            if (m_CooldownTokens.Count >= m_StartingToken)
                return;

            m_CooldownTokens.Add(m_TokenCooldownTime);
        }

        private void UpdateCooldownTokens()
        {
            if (m_CooldownTokens.Count == 0)
                return;

            float dt = Time.deltaTime;

            for (int i = m_CooldownTokens.Count - 1; i >= 0; i--)
            {
                m_CooldownTokens[i] -= dt;
                if (m_CooldownTokens[i] <= 0.0f)
                {
                    m_CooldownTokens.RemoveAt(i);
                    m_AvailableTokens.Value += 1;
                    Debug.Log("Tokens added to available token");
                }
            }
        }
    }
}
