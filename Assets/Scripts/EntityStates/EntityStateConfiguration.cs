using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

namespace CuttingEdge.EntityStates
{
    [CreateAssetMenu(fileName = "EntityStateConfiguration", menuName = "EntityStateConfiguration")]
    public class EntityStateConfiguration : ScriptableObject
    {
        [SerializableType.RequiredType(typeof(EntityState), "CuttingEdge.EntityStates")]
        public SerializableType stateType;
#if UNITY_EDITOR
        public MonoScript source;
#endif

        //[HideInInspector]
        public UnityEngine.Object[] serializedObjects = new UnityEngine.Object[0];
        //[HideInInspector]
        public int[] serializedInts = new int[0];
        //[HideInInspector]
        public float[] serializedFloats = new float[0];
        //[HideInInspector]
        public Vector3[] serializedVector3s = new Vector3[0];
        //[HideInInspector]
        public string[] serializedStrings = new string[0];
        [HideInInspector]
        public readonly Dictionary<Type, FieldInfo> typeToSerializedArray = new Dictionary<Type, FieldInfo>
        {
            { typeof(UnityEngine.Object), typeof(EntityStateConfiguration).GetField(nameof(serializedObjects)) },
            { typeof(int), typeof(EntityStateConfiguration).GetField(nameof(serializedInts)) },
            { typeof(float), typeof(EntityStateConfiguration).GetField(nameof(serializedFloats)) },
            { typeof(Vector3), typeof(EntityStateConfiguration).GetField(nameof(serializedVector3s)) },
            { typeof(string), typeof(EntityStateConfiguration).GetField(nameof(serializedStrings)) },
        };
#if UNITY_EDITOR
        [MenuItem("Assets/Create/EntityStateConfiguration/From EntityState", validate = true)]
        static bool Validate()
        {
            return Selection.activeObject is MonoScript script && typeof(EntityState).IsAssignableFrom(script.GetClass());
        }
        [MenuItem("Assets/Create/EntityStateConfiguration/From EntityState")]
        static void CreateFromEntityState()
        {
            EntityStateConfiguration newObject = CreateInstance<EntityStateConfiguration>();
            string selectedObjectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string path = AssetDatabase.GenerateUniqueAssetPath(selectedObjectPath.Substring(0, selectedObjectPath.LastIndexOf(".")) + "Config.asset");

            MonoScript entityStateScript = ((MonoScript)Selection.activeObject);
            newObject.stateType = entityStateScript.GetClass();
            newObject.source = entityStateScript;
            newObject.UpdateConfig();

            AssetDatabase.CreateAsset(newObject, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = newObject;
        }
        private void BakeSerializedFieldGetters(Dictionary<string, FieldTypeAndArrayIndex> fieldNameToType)
        {
            if (!source)
            {
                Debug.LogError("Source file is missing");
                return;
            }

            string filePath = AssetDatabase.GetAssetPath(source);
            if (!File.Exists(filePath))
            {
                Debug.LogError("Source file " + source.name + " could not be found");
            }
            string[] lines = File.ReadAllLines(filePath);
            bool searchingForClosestField = false;
            using (var writer = new StreamWriter(filePath, false))
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (searchingForClosestField)
                    {
                        string[] fieldParts = lines[i].Replace(";", "").Split(" ");
                        string currentField = fieldParts.Intersect(fieldNameToType.Keys).FirstOrDefault();
                        if (!string.IsNullOrEmpty(currentField))
                        {
                            searchingForClosestField = false;
                            StringBuilder fieldBuilder = new StringBuilder();
                            fieldBuilder.AppendJoin(" ", fieldParts);
                            if (fieldNameToType.TryGetValue(currentField, out FieldTypeAndArrayIndex field))
                            {
                                fieldBuilder.Append(" { get { return ");
                                if (typeof(UnityEngine.Object).IsAssignableFrom(field.type))
                                {
                                    fieldBuilder.Append($"({field.type.Name})");
                                }
                                fieldBuilder.Append("config.");
                                fieldBuilder.Append(GetSerializedArrayNameFromType(field.type));
                                fieldBuilder.Append($"[{field.index}]");
                                fieldBuilder.Append("; } }");

                                writer.WriteLine(fieldBuilder.ToString());
                                continue;
                            }
                            else
                            {
                                Debug.LogError("Failed to find field " + currentField);
                            }
                        }
                    }
                    writer.WriteLine(lines[i]);

                    if (lines[i].Contains("[ConfigSerializeField]"))
                    {
                        searchingForClosestField = true;
                    }
                }
            }
            AssetDatabase.Refresh();
        }
        private string GetSerializedArrayNameFromType(Type type)
        {
            if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                type = typeof(UnityEngine.Object);
            }
            if (typeToSerializedArray.TryGetValue(type, out var field))
            {
                return field.Name;
            }
            return "SerializableTypeNotFoundERROR";
        }
#endif
        [ContextMenu("Update Config")]
        public void UpdateConfig()
        {
            IEnumerable<PropertyInfo> properties = stateType.type.GetProperties();
            // Do something about existing baked config fields. Probably store count int as a bool array instead so it's easy to keep track of used and unused array indices

            IEnumerable<FieldInfo> fields = stateType.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).OrderBy(f => f.Name);
            Dictionary<string, FieldTypeAndArrayIndex> fieldNameToType = new Dictionary<string, FieldTypeAndArrayIndex>();
            Dictionary<Type, int> typeCounter = new Dictionary<Type, int>();
            foreach (FieldInfo field in fields)
            {
                ConfigSerializeFieldAttribute attribute = field.GetCustomAttribute<ConfigSerializeFieldAttribute>();
                if (attribute != null && TypeIsSerializable(field.FieldType))
                {
                    if (typeCounter.TryGetValue(field.FieldType, out int typeCount))
                    {
                        fieldNameToType.Add(field.Name, new FieldTypeAndArrayIndex(field.FieldType, typeCount));
                        typeCounter[field.FieldType] = typeCount + 1;
                    }
                    else
                    {
                        fieldNameToType.Add(field.Name, new FieldTypeAndArrayIndex(field.FieldType, 0));
                        typeCounter.Add(field.FieldType, 1);
                    }
                    Debug.Log("Found ConfigSerializeField " + field.Name);
                }
            }
            if (fieldNameToType.Count > 0)
            {
                foreach (var fieldType in fieldNameToType.Values)
                {
                    if (typeCounter.TryGetValue(fieldType.type, out int arrayLength))
                    {
                        if (typeof(UnityEngine.Object).IsAssignableFrom(fieldType.type))
                        {
                            Array.Resize(ref serializedObjects, arrayLength);
                        }
                        if (fieldType.type == typeof(int))
                        {
                            Array.Resize(ref serializedInts, arrayLength);
                        }
                        if (fieldType.type == typeof(float))
                        {
                            Array.Resize(ref serializedFloats, arrayLength);
                        }
                        if (fieldType.type == typeof(Vector3))
                        {
                            Array.Resize(ref serializedVector3s, arrayLength);
                        }
                        if (fieldType.type == typeof(string))
                        {
                            Array.Resize(ref serializedStrings, arrayLength);
                        }
                    }
                }
#if UNITY_EDITOR
                BakeSerializedFieldGetters(fieldNameToType);
#endif
            }
            else
            {
                Debug.LogWarning("No ConfigSerializeFields found");
            }
        }
        private bool TypeIsSerializable(Type type)
        {
            return typeToSerializedArray.ContainsKey(type) || typeof(UnityEngine.Object).IsAssignableFrom(type);
        }
        private struct FieldTypeAndArrayIndex
        {
            public Type type;
            public int index;
            public FieldTypeAndArrayIndex(Type type, int index)
            {
                this.type = type;
                this.index = index;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConfigSerializeFieldAttribute : Attribute
    {
        public ConfigSerializeFieldAttribute()
        {
        }
    }
    /*[CustomEditor(typeof(EntityStateConfiguration))]
    public class EntityStateConfigurationEditor : Editor
    {
    }*/
}
