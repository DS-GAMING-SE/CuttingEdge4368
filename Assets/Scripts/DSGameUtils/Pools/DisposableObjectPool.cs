using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSGameUtils.Pools
{
    public class DisposableObjectPool<T> : Pool<T> where T : IDisposable
    {
        protected override void DestroyObject(T pooledObject)
        {
            pooledObject.Dispose();
        }
    }
}
