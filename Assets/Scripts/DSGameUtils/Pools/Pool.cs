using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSGameUtils.Pools
{
    public class Pool<T>
    {
        protected Stack<T> unused;
        protected List<T> inUse;
        public Pool()
        {
            unused = new Stack<T>();
            inUse = new List<T>();
        }
        public Pool(int capacity)
        {
            unused = new Stack<T>(capacity);
            inUse = new List<T>(capacity);
        }
        public Pool(int startCount, int capacity)
        {
            unused = new Stack<T>(capacity);
            for (int i = 0; i < startCount; i++)
            {
                unused.Push(CreateNewPooledObject());
            }
            inUse = new List<T>(capacity);
        }
        public virtual T Get()
        {
            T pooledObject;
            if (unused.Count > 0)
            {
                pooledObject = unused.Pop();
            }
            else
            {
                pooledObject = CreateNewPooledObject();
            }
            inUse.Add(pooledObject);
            return pooledObject;
        }
        protected virtual T CreateNewPooledObject()
        {
            return default;
        }
        public virtual void Return(T pooledObject)
        {
            if (pooledObject == null) return;

            if (inUse.Contains(pooledObject))
            {
                inUse.Remove(pooledObject);
                unused.Push(pooledObject);
                ReturnObject(pooledObject);
            }
            else
            {
                DestroyObject(pooledObject);
            }
        }
        public virtual void Remove(T pooledObject)
        {
            if (pooledObject == null) return;

            inUse.Remove(pooledObject);
        }
        protected virtual void ReturnObject(T pooledObject)
        {

        }
        protected virtual void DestroyObject(T pooledObject)
        {

        }
        public void Clear()
        {
            for (int i = 0; i < unused.Count; i++)
            {
                DestroyObject(unused.Pop());
            }
            for (int i = 0; i < inUse.Count; i++)
            {
                T inUseObject = inUse[i];
                inUse.Remove(inUseObject);
                DestroyObject(inUseObject);
            }
        }
    }
}
