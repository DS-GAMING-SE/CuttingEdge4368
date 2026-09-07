using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSGameUtils
{
    public static class InstanceTracker
    {
        public static void Add<T>(T instance)
        {
            TypeData<T>.Add(instance);
        }
        public static void Remove<T>(T instance)
        {
            TypeData<T>.Remove(instance);
        }
        public static List<T> GetList<T>()
        {
            return TypeData<T>.instances;
        }
        public static T GetFirstOrDefault<T>()
        {
            if (TypeData<T>.instances.Count == 0) return default;

            return TypeData<T>.instances[0];
        }
        public static bool Any<T>()
        {
            return TypeData<T>.instances.Count > 0;
        }
        private static class TypeData<T>
        {
            public static readonly List<T> instances;
            static TypeData()
            {
                instances = new List<T>();
            }
            public static void Add(T instance)
            {
                instances.Add(instance);
            }
            public static void Remove(T instance)
            {
                instances.Remove(instance);
            }
        }
    }
}
