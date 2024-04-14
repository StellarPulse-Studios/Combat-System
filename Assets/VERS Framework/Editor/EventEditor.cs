using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace VERS.Editor
{
    [CustomEditor(typeof(GameEventSO))]
    public class EventEditor : UnityEditor.Editor
    {
        private FieldInfo m_Listeners;

        private void OnEnable()
        {
            Type type = typeof(GameEventSO);
            m_Listeners = type.GetField("m_Listeners", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameEventSO e = (GameEventSO)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Raise Event", GUILayout.Height(30.0f)))
            {
                e.Raise();
            }

            EditorGUILayout.Space();

            List<GameEventListener> listeners = m_Listeners.GetValue(e) as List<GameEventListener>;

            EditorGUILayout.LabelField($"Registered Listeners (Count : {listeners.Count})", EditorStyles.boldLabel);

            for (int i = 0; i < listeners.Count; i++)
            {
                GameEventListener listener = listeners[i];
                if (EditorGUILayout.LinkButton(listener.gameObject.name))
                    EditorGUIUtility.PingObject(listener.gameObject);

                UnityEvent response = listener.Response;
                if (response == null)
                    continue;

                EditorGUI.indentLevel++;

                int responseCount = response.GetPersistentEventCount();
                for (int j = 0; j < responseCount; j++)
                {
                    string methodName = response.GetPersistentMethodName(j);
                    UnityEngine.Object target = response.GetPersistentTarget(j);

                    EditorGUILayout.LabelField($"{target.GetType().Name} . {methodName}");
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}
