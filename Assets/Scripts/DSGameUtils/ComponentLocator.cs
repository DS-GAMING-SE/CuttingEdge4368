using System;
using UnityEditor;
using UnityEngine;

namespace DSGameUtils
{
    public class ComponentLocator : MonoBehaviour
    {
        [SerializeField]
        private NameComponent[] components = new NameComponent[0];

        public T FindComponent<T>(string name) where T : Component
        {
            return GetComponent<T>(FindComponentIndex(name));
        }
        public int FindComponentIndex(string name)
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        public T GetComponent<T>(int index) where T : Component
        {
            if (index >= components.Length) return null;
            if (components[index].component is T castedComponent)
            {
                return castedComponent;
            }
            return null;
        }

        [Serializable]
        private struct NameComponent
        {
            public string name;
            public Component component;
        }
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(NameComponent), true)]
        public class NameComponentDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.BeginProperty(position, label, property);
                var nameField = property.FindPropertyRelative("name");
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height - EditorGUIUtility.singleLineHeight), nameField);
                EditorGUI.BeginChangeCheck();
                var componentField = property.FindPropertyRelative("component");
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight), componentField);
                if (EditorGUI.EndChangeCheck() && componentField.objectReferenceValue)
                {
                    nameField.stringValue = componentField.objectReferenceValue.GetType().Name;
                }
                EditorGUI.indentLevel--;
                EditorGUI.EndProperty();
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUIUtility.singleLineHeight * 2f;
            }
        }
#endif
    }
}
