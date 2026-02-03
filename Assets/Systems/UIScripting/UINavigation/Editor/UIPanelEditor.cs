using UnityEditor;
using UnityEngine;

namespace UIScripting
{
    [CustomEditor(typeof(UIPanel))]
    public class UIPanelEditor : Editor
    {
        private readonly string[] eventProps =
        {
        "OnStartedOpening",
        "OnPanelOpened",
        "OnStartedClosing",
        "OnPanelClosed"
    };

        private const string EventsFoldoutKey = "UIPanelEditor_EventsFoldout";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("animationType"));

            SerializedProperty paramsProp = serializedObject.FindProperty("animationParams");
            if (paramsProp.managedReferenceValue != null)
            {
                EditorGUILayout.PropertyField(paramsProp, true);
            }

            EditorGUILayout.Space(10);

            bool eventsFoldout = EditorPrefs.GetBool(EventsFoldoutKey, false);
            eventsFoldout = EditorGUILayout.Foldout(eventsFoldout, "Events", true);
            EditorPrefs.SetBool(EventsFoldoutKey, eventsFoldout);

            if (eventsFoldout)
            {
                EditorGUI.indentLevel++;

                foreach (string propName in eventProps)
                {
                    SerializedProperty prop = serializedObject.FindProperty(propName);
                    if (prop != null)
                    {
                        EditorGUILayout.PropertyField(prop);
                    }
                }

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
