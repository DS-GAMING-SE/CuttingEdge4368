using CuttingEdge.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace CuttingEdge
{
    public static class SettingsManager
    {
        public static float sfxVolume = 1f;
        public static float musicVolume = 0.5f;
        public static void SetSetting(float value, Setting setting)
        {
            switch (setting)
            {
                case Setting.SFXVolume:
                    sfxVolume = value;
                    break;
                case Setting.MusicVolume:
                    MusicManager.instance.volume = value;
                    break;
            }
        }
        public static float GetSetting(Setting setting)
        {
            switch (setting)
            {
                case Setting.SFXVolume:
                    return sfxVolume;
                case Setting.MusicVolume:
                    return musicVolume;
            }
            return -1f;
        }
        public enum Setting
        {
            SFXVolume,
            MusicVolume,
        }
    }
}
