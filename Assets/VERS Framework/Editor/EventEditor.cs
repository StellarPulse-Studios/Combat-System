using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
                EditorGUILayout.LabelField(listeners[i].gameObject.name);
            }
        }
    }
}
