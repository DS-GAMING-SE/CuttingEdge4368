using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates
{
    public class EntityState
    {
        public EntityStateMachine outer;
        public GameObject gameObject => outer.gameObject;
        public Transform transform => outer.transform;
        public float fixedAge { get; private set; }
        public float age { get; private set; }
        public virtual void OnEnter()
        {

        }
        public virtual void OnExit()
        {

        }
        public virtual void FixedUpdate()
        {
            fixedAge += Time.fixedDeltaTime;
        }
        public virtual void Update()
        {
            age += Time.deltaTime;
        }
        public virtual void LateUpdate()
        {

        }
        public virtual InterruptPriority GetInterruptPriority()
        {
            return InterruptPriority.Any;
        }
        public T AddComponent<T>() where T : Component
        {
            return outer.gameObject.AddComponent<T>();
        }
        public T GetComponent<T>() where T : Component
        {
            return outer.GetComponent<T>();
        }
        public bool TryGetComponent<T>(out T component) where T : Component
        {
            return outer.TryGetComponent<T>(out component);
        }
        public bool TryGetComponent(Type type, out Component component)
        {
            return outer.TryGetComponent(type, out component);
        }
        public T EnsureComponent<T>() where T : Component
        {
            return outer.gameObject.EnsureComponent<T>();
        }
    }
    public enum InterruptPriority
    {
        Any,
        Skill,
        PrioritySkill,
        Death
    }
}
