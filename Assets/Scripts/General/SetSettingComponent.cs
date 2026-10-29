using CuttingEdge.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CuttingEdge
{
    public class SetSettingComponent : MonoBehaviour
    {
        public SettingsManager.Setting targetSetting;
        public UnityEvent<float> updateSettingOnStart;
        private void Start()
        {
            updateSettingOnStart?.Invoke(SettingsManager.GetSetting(targetSetting));
        }
        public void SetSetting(float value)
        {
            SettingsManager.SetSetting(value, targetSetting);
        }
    }
}