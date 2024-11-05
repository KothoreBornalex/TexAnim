using System.Collections;
using System.Collections.Generic;
using TexAnim.Enumerations;
using UnityEngine;

namespace TexAnim.Data
{
    using TexAnim.Editor.Utilities;


    [System.Serializable]
    public class TexAnim_SavedNode
    {
        public string nodeName;
        public float speed;
        public TexAnimAnimationNodeType nodeType;
        public List<string> choices;
        public Vector2 position;
        public TexAnimClip texAnimClip;

        //public Animation animation;


        public TexAnim_SavedNode() { }

        public TexAnim_SavedNode(string name, TexAnimAnimationNodeType type, Vector2 basePos)
        {
            nodeName = name;
            nodeType = type;
            choices = new List<string>();
            position = basePos;
            speed = 1.0f;
        }
    }
}
