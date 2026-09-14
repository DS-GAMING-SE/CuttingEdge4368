using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        [HideInInspector]
        public UnityEngine.Object[] serializedObjects = new UnityEngine.Object[0];
        [HideInInspector]
        public int[] serializedInts = new int[0];
        [HideInInspector]
        public float[] serializedFloats = new float[0];
        [HideInInspector]
        public Vector3[] serializedVector3s = new Vector3[0];
        [HideInInspector]
        public string[] serializedStrings = new string[0];
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

            newObject.stateType = ((MonoScript)Selection.activeObject).GetClass();
            newObject.UpdateConfig();

            AssetDatabase.CreateAsset(newObject, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = newObject;
        }
        [ContextMenu("Update Config")]
        void UpdateConfig()
        {
            IEnumerable<FieldInfo> fields = stateType.type.GetFields(BindingFlags.Public | BindingFlags.NonPublic).OrderBy(f => f.Name);
            foreach (FieldInfo field in fields)
            {
                ConfigSerializeFieldAttribute attribute = field.GetCustomAttribute<ConfigSerializeFieldAttribute>();
                if (attribute != null)
                {
                    // edit the script to assign indices to all attributes
                    // replaces field calls with getter properties that get from serialized object array
                }
            }
        }
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConfigSerializeFieldAttribute : Attribute
    {
        public int index;
        public ConfigSerializeFieldAttribute()
        {
            index = -1;
        }
        public ConfigSerializeFieldAttribute(int index)
        {
            this.index = index;
        }
    }
    /*[CustomEditor(typeof(EntityStateConfiguration))]
    public class EntityStateConfigurationEditor : Editor
    {
    }*/
}
