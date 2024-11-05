using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TexAnim
{
    [System.Serializable]
    public class TexAnimClip : ScriptableObject
    {
        private Texture2D _motion;
        private float _motionLength;

        public Texture2D Motion { get => _motion;}
        public float MotionLength { get => _motionLength;}


        public TexAnimClip(Texture2D motion, float length)
        {
            _motion = motion;
            _motionLength = length;
        }
    }
}
