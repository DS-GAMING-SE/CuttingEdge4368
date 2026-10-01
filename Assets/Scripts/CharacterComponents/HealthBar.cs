using CuttingEdge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CuttingEdge
{
    public class HealthBar : MonoBehaviour
    {
        [HideInInspector]
        public HealthComponent healthComponent;
        [SerializeField]
        private Transform healthBarRoot;
        [SerializeField]
        private Image healthBarFill;
        [SerializeField]
        private Vector3 positionOffset;

        private void LateUpdate()
        {
            if (healthComponent)
            {
                healthBarRoot.transform.position = Camera.main.WorldToScreenPoint(healthComponent.transform.position + positionOffset);
            }
        }

        public void SetFill(float fill)
        {
            healthBarFill.fillAmount = fill; 
        }
    }
}
