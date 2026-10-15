using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuttingEdge
{
    public static class TimeScaleManager
    {
        public static List<TimeScaleOverride> timeScaleInstances = new List<TimeScaleOverride>();

        [RuntimeInitializeOnLoadMethod()]
        private static void Initialize()
        {
            CuttingEdgeApplication.onUpdate += Update;
        }
        public static TimeScaleOverride AddTimeScaleOverride(float timeScale, float weight, float fadeInDuration, Priority priority)
        {
            TimeScaleOverride instance = new TimeScaleOverride { timeScale = timeScale, weight = weight, priority = priority };
            instance.StartFadeIn(fadeInDuration);
            timeScaleInstances.AddSorted(instance);

            return instance;
        }
        public static TimeScaleOverride AddTimeScaleOverrideOneShot(float timeScale, float weight, float fadeInDuration, float fadeOutDuration, Priority priority)
        {
            TimeScaleOverride instance = new TimeScaleOverride { timeScale = timeScale, weight = weight, priority = priority };
            instance.StartFadeIn(fadeInDuration);
            instance.StartFadeOut(fadeOutDuration, Time.realtimeSinceStartup + fadeInDuration);
            timeScaleInstances.AddSorted(instance);

            return instance;
        }
        public static TimeScaleOverride AddTimeScaleOverrideOneShot(float timeScale, float weight, float fadeInDuration, float duration, float fadeOutDuration, Priority priority)
        {
            TimeScaleOverride instance = new TimeScaleOverride { timeScale = timeScale, weight = weight, priority = priority };
            instance.StartFadeIn(fadeInDuration);
            instance.StartFadeOut(fadeOutDuration, Time.realtimeSinceStartup + duration + fadeInDuration);
            timeScaleInstances.AddSorted(instance);

            return instance;
        }
        public static void RemoveTimeScaleOverride(TimeScaleOverride timeScaleInstance, float fadeOutDuration = 0f)
        {
            if (fadeOutDuration == 0f) 
            {
                timeScaleInstances.Remove(timeScaleInstance);
            }
            else
            {
                timeScaleInstance.StartFadeOut(fadeOutDuration);
            }
        }

        private static void Update()
        {
            float timeScale = 1f;
            float lastWeight = 0f;
            bool hasEffect = true;
            for (int i = 0; i < timeScaleInstances.Count; i++)
            {
                if (hasEffect)
                {
                    timeScale *= Mathf.Lerp(timeScale, timeScaleInstances[i].timeScale * (1 - lastWeight), timeScaleInstances[i].weightWithFade);
                    lastWeight = timeScaleInstances[i].weightWithFade;
                }

                if (timeScaleInstances[i].weightWithFade == 1f)
                {
                    hasEffect = false;
                }

                if (Time.realtimeSinceStartup > timeScaleInstances[i].fadeOutEndTime)
                {
                    timeScaleInstances.RemoveAt(i);
                    i--;
                }
            }
            Time.timeScale = timeScale;
        }

        public class TimeScaleOverride : IComparable<TimeScaleOverride>
        {
            public float timeScale;
            public float weight;
            public float weightWithFade { get { return weight * fadeLerp; } }

            public float fadeInStartTime;
            public float fadeInDuration;
            public float fadeInEndTime { get { return fadeInStartTime + fadeInDuration; } }

            public float fadeOutStartTime = float.PositiveInfinity;
            public float fadeOutDuration;
            public float fadeOutEndTime { get { return fadeOutStartTime + fadeOutDuration; } }

            public float fadeLerp 
            { 
                get 
                { 
                    return Mathf.Clamp01(fadeInDuration != 0 ? ((Time.realtimeSinceStartup - fadeInStartTime) / fadeInDuration) : 1f) *
                        Mathf.Clamp01(fadeOutDuration != 0 ? 1 - ((Time.realtimeSinceStartup - fadeOutStartTime) / fadeOutDuration) : 1f);
                }
            }

            public Priority priority;

            public void StartFadeIn(float duration)
            {
                fadeInStartTime = Time.realtimeSinceStartup;
                fadeInDuration = duration;
            }
            public void StartFadeOut(float duration)
            {
                fadeOutStartTime = Time.realtimeSinceStartup;
                fadeOutDuration = duration;
            }
            public void StartFadeOut(float duration, float startTime)
            {
                fadeOutStartTime = startTime;
                fadeOutDuration = duration;
            }

            public int CompareTo(TimeScaleOverride other)
            {
                return priority.CompareTo(other.priority);
            }
        }

        public enum Priority
        {
            Low,
            High,
            Pause
        }
    }
}
