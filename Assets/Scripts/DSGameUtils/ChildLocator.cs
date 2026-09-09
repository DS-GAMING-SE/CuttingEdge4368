using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;


namespace DSGameUtils
{
    public class ChildLocator : MonoBehaviour
    {
        [SerializeField]
        private NameChild[] children = new NameChild[0];

        public GameObject FindChild(string name)
        {
            return GetChild(FindChildIndex(name));
        }
        public int FindChildIndex(string name)
        {
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        public GameObject GetChild(int index)
        {
            if (index < 0 || index >= children.Length) return null;
            return children[index].gameObject;
        }

        [Serializable]
        private struct NameChild
        {
            public string name;
            public GameObject gameObject;
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(NameChild), true)]
        public class NameChildDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.BeginProperty(position, label, property);
                var nameField = property.FindPropertyRelative("name");
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height - EditorGUIUtility.singleLineHeight), nameField);
                EditorGUI.BeginChangeCheck();
                var gameObjectField = property.FindPropertyRelative("gameObject");
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight), gameObjectField);
                if (EditorGUI.EndChangeCheck() && gameObjectField.objectReferenceValue)
                {
                    nameField.stringValue = gameObjectField.objectReferenceValue.name;
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
