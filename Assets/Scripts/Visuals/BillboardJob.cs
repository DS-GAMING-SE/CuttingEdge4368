using CuttingEdge;
using DSGameUtils;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace CuttingEdge.Visuals
{
    // Most complicated Billboard ever (for learning purposes (it doesn't actually work yet))
    /*public class BillboardJob : MonoBehaviour
    {
        private void OnEnable()
        {
            InstanceTracker.Add(this);
        }
        private void OnDisable()
        {
            InstanceTracker.Remove(this);
        }
    }
    public static class BillboardManager
    {
        [RuntimeInitializeOnLoadMethod()]
        private static void Initialize()
        {
            CuttingEdgeApplication.onLateUpdate -= UpdateBillboards;
            CuttingEdgeApplication.onLateUpdate += UpdateBillboards;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
                {
                    billboardTransforms.Dispose();
                }
            };
#endif
            billboardTransforms = new TransformAccessArray(0);
        }

        private static TransformAccessArray billboardTransforms;
        private static void UpdateBillboards()
        {
            List<BillboardJob> list = InstanceTracker.GetList<BillboardJob>();
            if (list.Count > 0)
            {
                billboardTransforms.capacity = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    if (i < billboardTransforms.length)
                    {
                        billboardTransforms[i] = list[i].transform;
                    }
                    else
                    {
                        billboardTransforms.Add(list[i].transform);
                    }
                }
                new BillboardRotateJob()
                {
                    transforms = billboardTransforms,
                    cameraRotation = Camera.main.transform.rotation
                }.Schedule(100, 1).Complete();
            }
        }
    }

    public struct BillboardRotateJob : IJobParallelFor
    {
        public TransformAccessArray transforms;
        [ReadOnly]
        public Quaternion cameraRotation;

        public void Execute(int i)
        {
            transforms[i].LookAt(transforms[i].position + cameraRotation * Vector3.forward, cameraRotation * Vector3.up);
        }
    }*/
}
