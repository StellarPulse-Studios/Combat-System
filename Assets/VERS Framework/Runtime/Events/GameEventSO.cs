using System.Collections.Generic;
using UnityEngine;

namespace VERS
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "VERS/Event/Game Event")]
    public class GameEventSO : ScriptableObject
    {
        private List<GameEventListener> m_Listeners = new List<GameEventListener>();

        public void RegisterListener(GameEventListener listener)
        {
            m_Listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            m_Listeners.Remove(listener);
        }

        public void Raise()
        {
            for (int i = m_Listeners.Count - 1; i >= 0; i--)
            {
                m_Listeners[i].OnEventRaised();
            }
        }
    }
}
