﻿using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public class ConfigComponent : GameFrameworkComponent
    {
        [SerializeField]
        private DeviceModelConfig m_DeviceModelConfig = null;

        [SerializeField]
        private TextAsset m_BuildInfoTextAsset = null;

        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;

        private BuildInfo m_BuildInfo = null;

        public DeviceModelConfig DeviceModelConfig
        {
            get
            {
                return m_DeviceModelConfig;
            }
        }

        public BuildInfo BuildInfo
        {
            get
            {
                return m_BuildInfo;
            }
        }

        public void InitBuildInfo()
        {
            if (m_BuildInfoTextAsset == null || string.IsNullOrEmpty(m_BuildInfoTextAsset.text))
            {
                Log.Info("Build info can not be found or empty.");
                return;
            }

            m_BuildInfo = Utility.Json.ToObject<BuildInfo>(m_BuildInfoTextAsset.text);
            if (m_BuildInfo == null)
            {
                Log.Warning("Parse build info failure.");
                return;
            }

            ARKGameEntry.Base.GameVersion = ARKGameEntry.Config.BuildInfo.GameVersion;
            ARKGameEntry.Base.InternalApplicationVersion = ARKGameEntry.Config.BuildInfo.InternalVersion;
        }

        public void InitDefaultDictionary()
        {
            if (m_DefaultDictionaryTextAsset == null || string.IsNullOrEmpty(m_DefaultDictionaryTextAsset.text))
            {
                Log.Info("Default dictionary can not be found or empty.");
                return;
            }

            if (!ARKGameEntry.Localization.ParseDictionary(m_DefaultDictionaryTextAsset.text))
            {
                Log.Warning("Parse default dictionary failure.");
                return;
            }
        }
    }
}
