using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DSGameUtils 
{
    public static class Util
    {
        public static T EnsureComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out var component))
            {
                return component;
            }
            else
            {
                return gameObject.AddComponent<T>();
            }
        }
        
        public static void AddSorted<T>(this List<T> list, T item) where T : IComparable<T>
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (item.CompareTo(list[i]) > 0)
                {
                    list.Insert(i, item);
                    return;
                }
            }
            list.Add(item);
        }

        public static string ToStringPercent(this float num)
        {
            return (Mathf.Floor(num * 100)).ToString() + "%";
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ToVector3XY(this Vector2 vector2)
        {
            return new Vector3(vector2.x, vector2.y, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ToVector3XZ(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }
    }
}
