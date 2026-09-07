using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DSGameUtils
{
    [Serializable]
    public struct SerializableType
    {
        public SerializableType(Type type)
        {
            _type = type;
            _typeName = _type.AssemblyQualifiedName;
            _lastTypeName = _typeName;
        }
        public Type type
        {
            get 
            { 
                if (_typeName != _lastTypeName)
                {
                    _type = Type.GetType(_typeName);
                    _lastTypeName = _typeName;
                }
                return _type;
            }
            set
            {
                _type = value;
                if (_type == null)
                {
                    _typeName = "";
                    return;
                }
                _typeName = _type.AssemblyQualifiedName;
            }
        }
        private Type _type;
        public string typeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                if (string.IsNullOrEmpty(value))
                {
                    _type = null;
                    return;
                }
                _type = Type.GetType(_typeName);
            }
        }
        [SerializeField]
        private string _typeName;
        private string _lastTypeName;

        public object CreateInstanceOfType()
        {
            return Activator.CreateInstance(type);
        }

        public T CreateInstanceOfType<T>()
        {
            return (T)Activator.CreateInstance(type);
        }


        public static implicit operator Type(SerializableType x) => x.type;

        public static implicit operator SerializableType(Type x) => new SerializableType(x);
    }
#if UNITY_EDITOR
    // TODO: Make an attribute that lets you specify a base type the serializabletype must be
    // Include a field that can change defaultNamespace? (For EntityState convenience)
    [CustomPropertyDrawer(typeof(SerializableType), true)]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        string typeName = "";
        SerializedProperty typeNameProperty;
        string defaultNamespace = "CuttingEdge";
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        bool init;
        const float BUTTON_WIDTH = 0.2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            typeNameProperty = property.FindPropertyRelative("_typeName");
            int namespaceLength = string.IsNullOrEmpty(defaultNamespace) ? 0 : defaultNamespace.Length + 1;
            if (!init) typeName = string.IsNullOrEmpty(typeNameProperty.stringValue) ? "" : typeNameProperty.stringValue.Substring(namespaceLength, typeNameProperty.stringValue.IndexOf(", ") - namespaceLength);
            init = true;
            typeName = EditorGUI.DelayedTextField(new Rect(position.x, position.y, position.width * (1-BUTTON_WIDTH), position.height - EditorGUIUtility.singleLineHeight), label, typeName);

            EditorGUI.indentLevel = 1;
            defaultNamespace = EditorGUI.DelayedTextField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight),
                "Namespace", defaultNamespace);
            EditorGUI.indentLevel = 0;

            bool update = false;
            update = EditorGUI.EndChangeCheck();
            update = update || GUI.Button(new Rect(position.x + (position.width * (1 - BUTTON_WIDTH)), position.y, position.width * BUTTON_WIDTH, position.height - EditorGUIUtility.singleLineHeight), "Validate", buttonStyle);

            if (update)
            {
                Type type = Type.GetType(defaultNamespace + "." + typeName);
                if (type != null)
                {
                    typeNameProperty.stringValue = type.AssemblyQualifiedName;
                    buttonStyle.normal.textColor = Color.green;
                    buttonStyle.hover.textColor = Color.green;
                }
                else
                {
                    buttonStyle.normal.textColor = Color.red;
                    buttonStyle.hover.textColor = Color.red;
                }
            }
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2f;
        }
    }
#endif
}
