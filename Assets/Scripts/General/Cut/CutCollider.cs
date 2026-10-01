using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace CuttingEdge
{
    [RequireComponent(typeof(MeshCollider), typeof(PooledGameObject))]
    public class CutCollider : MonoBehaviour
    {
        public MeshCollider meshCollider;
        [SerializeField]
        private PooledGameObject pooledGameObject;
        [HideInInspector]
        public Vector3 normal;
        [HideInInspector]
        public Vector3 position;
        [HideInInspector]
        public Vector3 direction;
        private bool cut;

        private const float DURATION = 0.1f;
        private void OnEnable()
        {
            cut = false;
            StartCoroutine(DisableCollider());
        }

        private IEnumerator DisableCollider()
        {
            EffectManager.SimpleEffect(Resources.Load<GameObject>("CutMissEffect"), position, Quaternion.LookRotation(direction)).transform.localScale = new Vector3(1, 1, direction.magnitude);
            yield return new WaitForSeconds(DURATION);
            Destroy(meshCollider.sharedMesh);
            CutManager.onCut?.Invoke(cut);
            cut = false;
            pooledGameObject.ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!this.isActiveAndEnabled) return;
            if (other.TryGetComponent<ICuttable>(out var cuttable) && cuttable.TryCut(position, normal))
            {
                cut = true;
                Debug.Log("Cut");
            }
        }
    }
}