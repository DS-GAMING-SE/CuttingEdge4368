using DSGameUtils.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuttingEdge
{
    public static class CutManager
    {
        public const float CUT_RANGE = 50f;
        // It doesnt like making a mesh that's completely flat
        public const float CUT_WIDTH = 0.3f;
        public static GameObjectPool cutColliderPool;
        [Tooltip("Event for when a cut happens. Bool is whether an enemy was hit")]
        public static Action<bool> onCut;
        public static void Cut(Vector2 startScreenPosition, Vector2 endScreenPosition, float range = CUT_RANGE)
        {
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];

            Vector3 startWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(startScreenPosition.x, startScreenPosition.y, range));
            Vector3 endWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(endScreenPosition.x, endScreenPosition.y, range));
            Vector3 cameraWorldPosition = (Camera.main.ScreenToWorldPoint(new Vector3(startScreenPosition.x, startScreenPosition.y, 0)) + Camera.main.ScreenToWorldPoint(new Vector3(endScreenPosition.x, endScreenPosition.y, 0))) / 2;
            Vector3 cutCenter = (startWorldPosition + endWorldPosition) / 2;
            Vector3 cutNormal = Vector3.Cross(startWorldPosition - endWorldPosition, cameraWorldPosition - startWorldPosition).normalized;
            Debug.DrawRay(cutCenter, cutNormal, Color.red, 0.5f);

            meshData.SetVertexBufferParams(5, new UnityEngine.Rendering.VertexAttributeDescriptor(UnityEngine.Rendering.VertexAttribute.Position));
            NativeArray<Vector3> points = meshData.GetVertexData<Vector3>();
            points[0] = cameraWorldPosition;
            points[1] = startWorldPosition + (cutNormal * (CUT_WIDTH / 2));
            points[2] = endWorldPosition + (cutNormal * (CUT_WIDTH / 2)); ;
            points[3] = startWorldPosition - (cutNormal * (CUT_WIDTH / 2));
            points[4] = endWorldPosition - (cutNormal * (CUT_WIDTH / 2)); ;


            meshData.SetIndexBufferParams(18, UnityEngine.Rendering.IndexFormat.UInt16);
            NativeArray<ushort> indexBuffer = meshData.GetIndexData<ushort>();
            // top tri
            indexBuffer[0] = 0;
            indexBuffer[1] = 1;
            indexBuffer[2] = 2;
            // bottom tri
            indexBuffer[3] = 0;
            indexBuffer[4] = 3;
            indexBuffer[5] = 4;
            // start side
            indexBuffer[6] = 0;
            indexBuffer[7] = 3;
            indexBuffer[8] = 1;
            // end side
            indexBuffer[9] = 0;
            indexBuffer[10] = 2;
            indexBuffer[11] = 4;
            // front
            indexBuffer[12] = 1;
            indexBuffer[13] = 2;
            indexBuffer[14] = 3;
            indexBuffer[15] = 2;
            indexBuffer[16] = 3;
            indexBuffer[17] = 4;

            /*Debug.DrawLine(points[0], points[1], Color.white, 1f);
            Debug.DrawLine(points[0], points[2], Color.white, 1f);
            Debug.DrawLine(points[0], points[3], Color.white, 1f);
            Debug.DrawLine(points[0], points[4], Color.white, 1f);*/

            meshData.subMeshCount = 1;
            meshData.SetSubMesh(0, new UnityEngine.Rendering.SubMeshDescriptor(0, meshData.GetIndexData<ushort>().Length));

            CutCollider collider = CreateOrGetPooledCollider();
            Mesh mesh = new Mesh();
            mesh.name = "CutColliderMesh";
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh, UnityEngine.Rendering.MeshUpdateFlags.DontValidateIndices);
            Physics.BakeMesh(mesh.GetInstanceID(), true);
            collider.meshCollider.sharedMesh = mesh;
            collider.position = cutCenter;
            collider.normal = cutNormal;
            collider.direction = startWorldPosition - endWorldPosition;
            collider.gameObject.SetActive(true);
        }

        private static CutCollider CreateOrGetPooledCollider()
        {
            if (cutColliderPool == null)
            {
                cutColliderPool = new GameObjectPool(Resources.Load<GameObject>("CutCollider"));
            }
            return cutColliderPool.GetDeactivated().GetComponent<CutCollider>();
        }

        private static void PlayCutSound(bool hit)
        {
            if (hit) SoundEffectManager.PlaySound(Resources.Load<AudioClip>("Player/sfxCutHit1"), Resources.Load<AudioClip>("Player/sfxCutHit2"), Resources.Load<AudioClip>("Player/sfxCutHit3"));
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            SceneManager.activeSceneChanged += (scene, scene2) => { cutColliderPool?.Clear(); };
            CutManager.onCut += PlayCutSound;
        }
    }
}
