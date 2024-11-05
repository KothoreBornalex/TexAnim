using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TexAnim.Data.Settings
{
    using TexAnim.Enumerations;
    [System.Serializable]
    public class TexAnim_AnimsSettings
    {
        public TexAnim_SettingsType settingsType;

        #region Handle Float Type
        [SerializeField] private float _floatValue = 0.0f;
        public void SetFloatValue(float value)
        {
            _floatValue = value;
        }
        public float GetFloatValue()
        {
            return _floatValue;
        }
        #endregion

        #region Handle Int Type
        [SerializeField] private int _intValue = 0;
        public void SetIntValue(int value)
        {
            _intValue = value;
        }
        public int GetIntValue()
        {
            return _intValue;
        }
        #endregion

        #region Handle Bool Type
        [SerializeField] private bool _boolValue = false;
        public void SetBoolValue(bool value)
        {
            _boolValue = value;
        }
        public bool GetBoolValue()
        {
            return _boolValue;
        }
        #endregion

        #region Handle Trigger Type
        [SerializeField] private bool _triggerValue = false;
        public void SetTriggerValue(bool value)
        {
            _triggerValue = value;
        }
        public bool GetTriggerValue()
        {
            return _triggerValue;
        }
        #endregion


        public TexAnim_AnimsSettings(TexAnim_SettingsType settings)
        {
            settingsType = settings;
        }

        //Float Declaration
        public TexAnim_AnimsSettings(TexAnim_SettingsType settings, float value)
        {
            settingsType = settings;
            _floatValue = value;
        }

        //Int Declaration
        public TexAnim_AnimsSettings(TexAnim_SettingsType settings, int value)
        {
            settingsType = settings;
            _intValue = value;
        }

        //Bool & Trigger Declaration
        public TexAnim_AnimsSettings(TexAnim_SettingsType settings, bool value)
        {
            settingsType = settings;

            switch (settings)
            {
                case TexAnim_SettingsType.Bool:
                    _boolValue = value;
                    break;

                case TexAnim_SettingsType.Trigger:
                    _triggerValue = value;
                    break;
            }
        }

        
    }
}
